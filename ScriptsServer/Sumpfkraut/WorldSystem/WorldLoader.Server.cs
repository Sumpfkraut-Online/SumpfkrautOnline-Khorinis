using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Database;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldLoader
    {

        protected string dbFilePath = null;
        public string DBFilePath { get { return this.dbFilePath; } }

        protected List<List<List<object>>> sqlResults = null;
        protected bool sqlResultInUse = false;
        public void DropSQLResult () { if (!sqlResultInUse) { sqlResults = null; } }

        public static readonly Dictionary<string, List<DBTables.ColumnGetTypeInfo>> DBStructure =
            new Dictionary<string, List<DBTables.ColumnGetTypeInfo>>()
            {
                {
                    "WorldEffect", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("WorldEffectID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "WorldChange", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("WorldChangeID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("WorldEffectID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("Func", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("Params", SQLiteGetTypeEnum.GetString),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "InstEffect", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("InstEffectID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "InstChange", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("InstChangeID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("InstEffectID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("Func", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("Params", SQLiteGetTypeEnum.GetString),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "VobInst", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("VobInstID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("IsStatic", SQLiteGetTypeEnum.GetBoolean),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "VobInstEffect", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("VobInstID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("InstEffectID", SQLiteGetTypeEnum.GetInt32),
                    }
                },
                {
                    "StaticDynamicJob", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("JobID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("TableName", SQLiteGetTypeEnum.GetString),
                        new DBTables.ColumnGetTypeInfo("Task", SQLiteGetTypeEnum.GetString),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
            };

        //public static readonly List<string> DBTableLoadOrder = DBStructure.Keys.ToList();
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "WorldEffect", "WorldChange", "StaticDynamicJob",
        };

        protected static List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo = null;
        public static List<List<DBTables.ColumnGetTypeInfo>> ColGetTypeInfo { get { return colGetTypeInfo; } }

        protected WorldDef worldDef = null;
        public WorldDef WorldDef { get { return worldDef; } }



        public WorldLoader (string dbFilePath)
            : this("WorldDef (default)", dbFilePath)
        { }

        public WorldLoader (string name, string dbFilePath)
        {
            SetObjName(name);
            this.dbFilePath = dbFilePath;
        }



        // load World from database
        public void Load () { pLoad(); }
        partial void pLoad ()
        {
            // prepare data conversion parameters if it's still not done yet
            if (colGetTypeInfo == null)
            {
                colGetTypeInfo = new List<List<DBTables.ColumnGetTypeInfo>>();

                for (int t = 0; t < DBTableLoadOrder.Count; t++)
                {
                    colGetTypeInfo.Add(DBStructure[DBTableLoadOrder[t]]);
                }
            }

            // fill the queue of commands / subsequent sql-database-requests
            List<string> commandQueue = new List<string>();
            for (int t = 0; t < ColGetTypeInfo.Count; t++)
            {
                StringBuilder commandSB = new StringBuilder();

                // select columns in order (by their names) --> SELECT col1, col2, ... coln
                commandSB.Append("SELECT ");
                int lastColumnIndex = ColGetTypeInfo[t].Count - 1;
                for (int c = 0; c < ColGetTypeInfo[t].Count; c++)
                {
                    if (c != lastColumnIndex)
                    {
                        commandSB.Append(ColGetTypeInfo[t][c].colName + ",");
                    }
                    else
                    {
                        commandSB.Append(ColGetTypeInfo[t][c].colName);
                    }
                }

                // always sort by <nameOfTable>ID --> e.g. FROM WorldEffect WHERE 1 ORDER BY WorldEffectID;
                commandSB.AppendFormat(" FROM {0} WHERE 1 ORDER BY {1}ID;", DBTableLoadOrder[t], 
                    DBTableLoadOrder[t]);
                commandQueue.Add(commandSB.ToString());
            }

            foreach (string cmd in commandQueue) { MakeLog(cmd); }


            // send out a parallel working DBAgent which informs back when finished with the queue
            DBAgent dbAgent = new DBAgent(DBFilePath, commandQueue, false);
            dbAgent.SetObjName(GetObjName() + "-DBAgent");
            //dbAgent.FinishedQueue += WorldDefFromSQLResults;
            dbAgent.Start();
        }

        // generate a WorldDef-object from the retrieved sql-results
        protected void WorldDefFromSQLResults (GUC.Utilities.Threading.AbstractRunnable sender,
            DBAgent.FinishedQueueEventHandlerArgs e)
        {
            // convert the sql-query-results from string to their respective datatypes
            sqlResultInUse = true;
            sqlResults = e.GetSQLResults();
            DBTables.ConvertSQLResults(ref sqlResults, ref colGetTypeInfo, false);

            // construct the WorldDef-object according to the converted sql-data
            worldDef = new WorldDef(this);
            ApplyWorldEffects(ref worldDef, ref sqlResults, ref colGetTypeInfo);

            // initalize VobInst-objects from database via loaders of the VobSystem
            //VobSystem.VobInstLoader vobInstLoader = new VobSystem.VobInstLoader(ref worldDef);
            //vobInstLoader.Start();
            //vobInstLoader.FinishedLoading += delegate (object sender, FinishedLoadingArgs e)
            //{
            //    // release control over sqlResults on finishing line
            //    sqlResultInUse = false;
            //};
        }

        // actually apply all the world-parameters defined in the database 
        // (like current and future weather, global effects, etc.)
        protected static void ApplyWorldEffects (ref WorldDef worldDef, ref List<List<List<object>>> sqlResults, 
            ref List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo)
        {
            // !!! TO DO !!!
        }

    }
}
