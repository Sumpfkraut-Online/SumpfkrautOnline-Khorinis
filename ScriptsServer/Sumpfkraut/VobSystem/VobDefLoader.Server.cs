using GUC.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities.Threading;

namespace GUC.Scripts.Sumpfkraut.VobSystem
{
    public partial class VobDefLoader
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
                    new DBTables.ColumnGetTypeInfo("Func", SQLiteGetTypeEnum.GetInt32),
                    new DBTables.ColumnGetTypeInfo("Params", SQLiteGetTypeEnum.GetString),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                }
            },
            {
                "VobDef", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("VobDefID", SQLiteGetTypeEnum.GetInt32),
                    new DBTables.ColumnGetTypeInfo("IsStatic", SQLiteGetTypeEnum.GetBoolean),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                }
            },
            {
                "VobDefEffect", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("VobDefID", SQLiteGetTypeEnum.GetInt32),
                    new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetTypeEnum.GetInt32),
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

        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "DefEffect", "DefChange", "VobDef", "VobDefEffect", "StaticDynamicJob",
        };

        protected static List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo = null;
        public static List<List<DBTables.ColumnGetTypeInfo>> ColGetTypeInfo { get { return colGetTypeInfo; } }



        partial void pLoad ()
        {
            // prepare data conversion parameters if it's still not done yet
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
            
            // send out a parallel working DBAgent which informs back when finished with the queue
            DBAgent dbAgent = new DBAgent(DBFilePath, commandQueue, false);
            dbAgent.SetObjName(GetObjName() + "-DBAgent");
            dbAgent.FinishedQueue += VobDefFromSQLResults;
            dbAgent.Start();
        }

        private void VobDefFromSQLResults (AbstractRunnable sender, DBAgent.FinishedQueueEventHandlerArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
