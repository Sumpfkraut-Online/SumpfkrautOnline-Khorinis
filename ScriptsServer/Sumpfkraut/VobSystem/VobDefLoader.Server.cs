using GUC.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities.Threading;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.EffectSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;

namespace GUC.Scripts.Sumpfkraut.VobSystem
{
    public partial class VobDefLoader
    {

        public static readonly Dictionary<string, List<DBTables.ColumnGetTypeInfo>> DBStructure =
            new Dictionary<string, List<DBTables.ColumnGetTypeInfo>>()
        {
            {
                "DefEffect", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetType.GetInt32),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetType.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetType.GetDateTime),
                }
            },
            {
                "DefChange", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("DefChangeID", SQLiteGetType.GetInt32),
                    new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetType.GetInt32),
                    new DBTables.ColumnGetTypeInfo("Func", SQLiteGetType.GetInt32),
                    new DBTables.ColumnGetTypeInfo("Params", SQLiteGetType.GetString),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetType.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetType.GetDateTime),
                }
            },
            {
                "VobDef", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("VobDefID", SQLiteGetType.GetInt32),
                    new DBTables.ColumnGetTypeInfo("IsStatic", SQLiteGetType.GetBoolean),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetType.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetType.GetDateTime),
                }
            },
            {
                "VobDefEffect", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("VobDefID", SQLiteGetType.GetInt32),
                    new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetType.GetInt32),
                }
            },
            {
                "StaticDynamicJob", new List<DBTables.ColumnGetTypeInfo>
                {
                    new DBTables.ColumnGetTypeInfo("JobID", SQLiteGetType.GetInt32),
                    new DBTables.ColumnGetTypeInfo("TableName", SQLiteGetType.GetString),
                    new DBTables.ColumnGetTypeInfo("Task", SQLiteGetType.GetString),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetType.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetType.GetDateTime),
                }
            },
        };

        // fixed load order when accessing the database tables
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "DefEffect", "DefChange", "VobDef", "VobDefEffect", "StaticDynamicJob",
        };

        // uses DBTableLoadOrder and arranges the GetTypeInfos for result-data-conversion for later reusability
        protected static List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo = null;
        public static List<List<DBTables.ColumnGetTypeInfo>> ColGetTypeInfo { get { return colGetTypeInfo; } }



        protected object loadLock;

        protected string dbFilePath = null;
        public string DBFilePath { get { return dbFilePath; } }

        protected string effectTableName = null;
        public string GetEffectTableName () { return effectTableName; }
        public void SetEffectTableName (string value)
        {
            lock (loadLock) { effectTableName = value; }
        }

        protected string changeTableName = null;
        public string GetChangeTableName () { return changeTableName; }
        public void SetChangeTableName (string value)
        {
            lock (loadLock) { changeTableName = value; }
        }

        protected List<List<List<object>>> sqlResults;
        public List<List<List<object>>> GetLastSQLResults ()
        {
            lock (loadLock) { return sqlResults; }
        }



        partial void pLoad ()
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
            
            // send out a asynchronous / parallel working DBAgent which informs back when finished with the queue
            DBAgent dbAgent = new DBAgent(DBFilePath, commandQueue, false, false);
            dbAgent.SetObjName(GetObjName() + "-DBAgent");
            dbAgent.FinishedQueue += VobDefFromSQLResults;
            dbAgent.Start();
        }

        protected void VobDefFromSQLResults (AbstractRunnable sender, DBAgent.FinishedQueueEventHandlerArgs e)
        {
            try
            {
                // convert the received database-results
                List<List<List<object>>> sqlResults = e.GetSQLResults();
                DBTables.ConvertSQLResults(sqlResults, ColGetTypeInfo);

                // database tables: "DefEffect", "DefChange", "VobDef", "VobDefEffect", "StaticDynamicJob"
                //int i_DefEffect = DBTableLoadOrder.IndexOf("DefEffect");
                //List<DBTables.ColumnGetTypeInfo> cgt_DefEffect = ColGetTypeInfo[i_DefEffect];
                //List<List<object>> tableDefEffect = sqlResults[ DBTableLoadOrder.IndexOf("DefEffect") ];

                //int i_DefChange = DBTableLoadOrder.IndexOf("DefChange");
                //List<DBTables.ColumnGetTypeInfo> cgt_DefChange = ColGetTypeInfo[i_DefChange];
                //List<List<object>> tableDefChange = sqlResults[ DBTableLoadOrder.IndexOf("DefChange") ];

                int i_VobDef = DBTableLoadOrder.IndexOf("VobDef");
                List<DBTables.ColumnGetTypeInfo> cgt_VobDef = ColGetTypeInfo[i_VobDef];
                List<List<object>> tableVobDef = sqlResults[DBTableLoadOrder.IndexOf("VobDef")];

                int i_VobDefEffect = DBTableLoadOrder.IndexOf("VobDefEffect");
                List<DBTables.ColumnGetTypeInfo> cgt_VobDefEffect = ColGetTypeInfo[i_VobDefEffect];
                List<List<object>> tableVobDefEffect = sqlResults[DBTableLoadOrder.IndexOf("VobDefEffect")];

                //int i_StaticDynamicJob = DBTableLoadOrder.IndexOf("StaticDynamicJob");
                //List<DBTables.ColumnGetTypeInfo> cgt_StaticDynamicJob = ColGetTypeInfo[i_StaticDynamicJob];
                //List<List<object>> tableStaticDynamicJob = sqlResults[ DBTableLoadOrder.IndexOf("StaticDynamicJob") ];


                List<Effect> effects = new List<Effect>();
                EffectLoader effectLoader = new EffectLoader(dbFilePath, "Vob")


                //Dictionary<int, List<Tuple<int, Effect>>> hostIDToEffectInfo = 
                //    new Dictionary<int, List<Tuple<int, Effect>>>(tableVobDef.Count);
                //// effectID --> Tuple<changeID, changeRowIndex>
                //Dictionary<int, List<Tuple<int, int>>> effectIDToChangeInfo = 
                //    new Dictionary<int, List<Tuple<int, int>>> (tableDefEffect.Count);
                //List<Tuple<int, Effect>> effectInfo;
                //List<Tuple<int, int>> changeInfo;
                //int hostID, effectID, changeID;

                //for (int i = 0; i < tableVobDefEffect.Count; i++)
                //{
                //    hostID = (int) tableVobDefEffect[i][0];
                //    effectID = (int) tableVobDefEffect[i][1];
                //    if (hostIDToEffectInfo.TryGetValue(hostID, out effectInfo))
                //    {
                //        effectInfo.Add(Tuple.Create(effectID, new Effect(null)));
                //    }
                //    else
                //    {
                //        effectInfo = new List<Tuple<int, Effect>>() { Tuple.Create(effectID, new Effect(null)) };
                //        hostIDToEffectInfo.Add(hostID, effectInfo);
                //    }
                //}

                //for (int i = 0; i < tableDefChange.Count; i++)
                //{
                //    effectID = (int) tableDefChange[i][0];
                //    changeID = (int) tableDefChange[i][1];
                //    if (effectIDToChangeInfo.TryGetValue(effectID, out changeInfo))
                //    {
                //        changeInfo.Add(Tuple.Create(changeID, i));
                //    }
                //    else
                //    {
                //        changeInfo = new List<Tuple<int, int>>() { Tuple.Create(changeID, i) };
                //        effectIDToChangeInfo.Add(effectID, changeInfo);
                //    }
                //}


                //List<Change> changes;
                //List<int> failedChangeIndices;
                //Effect effect;
                //List<Effect> effects;
                //VobDef vobDef;
                //int vdr;                                // row-index in tableVobDef
                //int vobDefID;                           // column-entry VoDefID
                //List<int> effectIDs = null;             // effectIDs that belong to the VobDef
                //List<List<object>> changeRows = null;   // rows from ChangeDef-table that belong to the VobDef

                //for (vdr = 0; vdr < tableVobDef.Count; vdr++)
                //{
                //    vobDefID = (int) tableVobDef[vdr][0];

                //    // search for ids od effects that belong to the VobDef with the given id
                //    if (!TryFilterEffectIDsBySearchID(vobDefID, tableVobDefEffect, out effectIDs))
                //    {
                //        MakeLogWarning("No effects found to define VobDef with VobDefID: " + vobDefID);
                //        continue;
                //    }

                //    // create effects from their changes in succession
                //    int i;
                //    for (i = 0; i < effectIDs.Count; i++)
                //    {
                //        // search respective rows from DefChange which are related to the found effects with their ids
                //        if (!TryFilterChangeRowsByEffectID(effectIDs[i], tableDefChange, out changeRows))
                //        {
                //            MakeLogWarning("No changes found to define VobDef with VobDefID: " + vobDefID);
                //            continue;
                //        }

                //        // create changes
                //        changes = null;
                //        failedChangeIndices = ChangesFromRows(changeRows, out changes);
                //        if (failedChangeIndices.Count > 0)
                //        {
                //            MakeLogWarning(string.Format("Failed to initialize {0} of {1} changes from changeRows "
                //                + "for VobDefID = {2}! Their ChangeTypes or parameters might be insufficient or not supported", 
                //                failedChangeIndices.Count, changeRows.Count, vobDefID));
                //        }

                //        // create VobDef and apply all changes
                //        effect = new Effect(null);
                        
                //    }
                    
                //}
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to finish generating VodDef-objects from sqlResults: " + ex);
            }
        }

        protected List<int> ChangesFromRows (List<List<object>> changeRows, out List<Change> changes)
        {
            List<int> failedIndices = new List<int>();
            changes = new List<Change>();

            int i;
            for (i = 0; i < changeRows.Count; i++)
            {

            }

            return failedIndices;
        }

        // search all "rows" in searchTable which begin with the provided searchID 
        // and output all effectIDs which are paired with it
        protected bool TryFilterEffectIDsBySearchID (int searchID, List<List<object>> searchTable, out List<int> effectIDs)
        {
            effectIDs = new List<int>();

            int i;
            for (i = 0; i < searchTable.Count; i++)
            {
                if (((int) searchTable[i][0]) == searchID) { effectIDs.Add((int) searchTable[i][1]); }
            }

            if (effectIDs.Count < 1) { return false; }
            return true;
        }

        protected bool TryFilterChangeRowsByEffectIDs (List<int> effectIDs, List<List<object>> changeTable, 
            out List<List<object>> changeRows)
        {
            changeRows = new List<List<object>>();
            List<List<object>> tempChanges = null;

            int i;
            for (i = 0; i < effectIDs.Count; i++)
            {
                if (TryFilterChangeRowsByEffectID(effectIDs[i], changeTable, out tempChanges))
                {
                    changeRows.AddRange(tempChanges);
                }
            }

            if (changeRows.Count < 1) { return false; }
            return true;
        }

        protected bool TryFilterChangeRowsByEffectID (int effectID, List<List<object>> changeTable, 
            out List<List<object>> changeRows)
        {
            changeRows = new List<List<object>>();

            int i;
            for (i = 0; i < changeTable.Count; i++)
            {
                // expect EffectID to be 2nd column
                if (((int) changeTable[i][1]) == effectID) { changeRows.Add(changeTable[i]); }
            }

            if (changeRows.Count < 1) { return false; }
            return true;
        }

        protected bool TryFindVobDefType (List<List<object>> changeRows, out VobDefType vobDefType)
        {
            vobDefType = VobDefType.Undefined;

            return true;
        }

    }
}
