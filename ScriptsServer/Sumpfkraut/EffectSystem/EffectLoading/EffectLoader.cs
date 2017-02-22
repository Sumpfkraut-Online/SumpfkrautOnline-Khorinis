using GUC.Scripts.Sumpfkraut.Database;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Utilities;
using GUC.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static GUC.Scripts.Sumpfkraut.Database.DBAgent;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public class EffectLoader : ExtendedObject
    {

        new public static readonly string _staticName = "EffectLoader (static)";

        public static readonly Dictionary<string, List<DBTables.ColumnGetTypeInfo>> DBStructure =
            new Dictionary<string, List<DBTables.ColumnGetTypeInfo>>()
        {
            {
                "DefEffect", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetTypeEnum.GetInt32),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                }
            },
            {
                "DefChange", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("DefChangeID", SQLiteGetTypeEnum.GetInt32),
                    new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetTypeEnum.GetInt32),
                    new DBTables.ColumnGetTypeInfo("ChangeType", SQLiteGetTypeEnum.GetInt32),
                    new DBTables.ColumnGetTypeInfo("Params", SQLiteGetTypeEnum.GetString),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                }
            },
        };

        // fixed load order when accessing the database tables
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "DefEffect", "DefChange"
        };

        // uses DBTableLoadOrder and arranges the GetTypeInfos for result-data-conversion for later reusability
        protected static List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo = null;
        public static List<List<DBTables.ColumnGetTypeInfo>> ColGetTypeInfo { get { return colGetTypeInfo; } }

        public delegate void FinishedLoadingHandler (FinishedLoadingHandlerArgs e);
        public class FinishedLoadingHandlerArgs
        {
            public DateTime startTime;
            public DateTime endTime;

            public List<List<List<object>>> sqlResults;
            public Dictionary<int, Effect> effectsByID;
        }



        protected object loadLock;

        protected string dbFilePath = null;
        public string GetDBFilePath () { return this.dbFilePath; }
        public void SetDBFilePath (string value)
        {
            lock (loadLock) { dbFilePath = value; }
        }

        protected Dictionary<int, Effect> effectsByID;
        public Dictionary<int, Effect> GetLastEffectsByID ()
        {
            lock (loadLock) { return effectsByID; }
        }

        protected List<List<List<object>>> sqlResults;
        public List<List<List<object>>> GetLastSQLResults ()
        {
            lock (loadLock) { return sqlResults; }
        }



        public EffectLoader (string dbFilePath)
        {
            SetObjName("EffectLoader");
            SetDBFilePath(dbFilePath);
        }



        public void DropResults ()
        {
            lock (loadLock)
            {
                sqlResults = null;
                effectsByID = null;
            }
        }

        public void Load (string dbFilePath, bool useAsyncMode = false, FinishedLoadingHandler handler = null)
        {
            FinishedLoadingHandlerArgs e = new FinishedLoadingHandlerArgs();
            e.startTime = DateTime.Now;
   
            lock (loadLock)
            {
                // prepare data conversion parameters if it's still not done yet (done only once per server run)
                if (colGetTypeInfo == null)
                {
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

                // send out DBAgent which might work asynchronously
                // ...use AutoResetEvent for that case (forces thread to wait until continue is signalled)
                DBAgent dbAgent = new DBAgent(dbFilePath, commandQueue, false, useAsyncMode);
                dbAgent.SetObjName("DBAgent (EffectLoader)");
                dbAgent.FinishedQueue += EffectsFromSQLResults;
                dbAgent.waitHandle.WaitOne();
                dbAgent.Start();

                // hand all loadResults to a possibly defined handler-function
                e.sqlResults = sqlResults;
                e.effectsByID = effectsByID;
                e.endTime = DateTime.Now;
                handler?.Invoke(e);
            }
        }

        public void EffectsFromSQLResults (AbstractRunnable sender, FinishedQueueEventHandlerArgs e)
        {
            try
            {
                lock (loadLock)
                {
                    sqlResults = e.GetSQLResults();

                    // return if there is nothing to process
                    if ((sqlResults == null) || (sqlResults.Count < 2)) { return; }

                    // convert the data-strings to their respective types
                    DBTables.ConvertSQLResults(sqlResults, colGetTypeInfo);

                    int i_DefEffect = DBTableLoadOrder.IndexOf("DefEffect");
                    List<DBTables.ColumnGetTypeInfo> cgt_DefEffect = ColGetTypeInfo[i_DefEffect];
                    List<List<object>> tableEffect = sqlResults[ DBTableLoadOrder.IndexOf("DefEffect") ];

                    int i_DefChange = DBTableLoadOrder.IndexOf("DefChange");
                    List<DBTables.ColumnGetTypeInfo> cgt_DefChange = ColGetTypeInfo[i_DefChange];
                    List<List<object>> tableChange = sqlResults[ DBTableLoadOrder.IndexOf("DefChange") ];

                    // create the Effects without assigning them an EffectHandler (must be done in external routine)
                    effectsByID = new Dictionary<int, Effect>();
                    Effect effect = null;
                    Change change = null;
                    int effectID;
                    for (int i = 0; i < tableChange.Count; i++)
                    {
                        effectID = (int) tableChange[i][1];
                        change = Change.Create();

                        if (effectsByID.TryGetValue(effectID, out effect))
                        {
                            effect.AddChange(change);
                        }
                        else
                        {
                            effect = new Effect(null, new List<Change>() { change });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Error while converting sqlResults to Effects: " + ex);
            }
        }

    }

}
