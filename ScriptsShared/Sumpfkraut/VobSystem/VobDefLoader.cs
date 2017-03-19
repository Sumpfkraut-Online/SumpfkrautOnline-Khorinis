﻿using GUC.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities.Threading;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.EffectSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.VobSystem
{
    public partial class VobDefLoader : BaseObjectLoader
    {

        public static readonly Dictionary<string, List<DBTables.ColumnGetTypeInfo>> DBStructure =
            new Dictionary<string, List<DBTables.ColumnGetTypeInfo>>()
        {
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
            "VobDef", "VobDefEffect",
        };

        public delegate void FinishedLoadingVobDefHandler (object sender, FinishedLoadingVobDefsArgs e);
        public partial class FinishedLoadingVobDefsArgs : FinishedLoadingArgs
        {
            public Dictionary<int, VobDef> VobDefByID;
        }



        public static readonly string DefaultEffectTableName = "DefEffect";
        protected string effectTableName = null;
        public string GetEffectTableName () { return effectTableName; }
        public void SetEffectTableName (string value)
        {
            lock (loadLock) { effectTableName = value; }
        }

        public static readonly string DefaultChangeTableName = "DefChange";
        protected string changeTableName = null;
        public string GetChangeTableName () { return changeTableName; }
        public void SetChangeTableName (string value)
        {
            lock (loadLock) { changeTableName = value; }
        }

        protected Dictionary<int, VobDef> vobDefByID;
        public Dictionary<int, VobDef> GetLastVobDefByID ()
        {
            lock (loadLock) { return vobDefByID; }
        }



        public VobDefLoader (string dbFilePath, 
            string effectTableName = null, string changeTableName = null) 
            : base ("VobDefLoader", dbFilePath, DBStructure, DBTableLoadOrder)
        {
            this.dbFilePath = dbFilePath;
            this.effectTableName = effectTableName ?? DefaultEffectTableName;
            this.changeTableName = changeTableName ?? DefaultChangeTableName;
        }



        public void Load (bool useAsyncMode, FinishedLoadingVobDefHandler handler)
        {
            lock (loadLock)
            {
                var e = new FinishedLoadingVobDefsArgs();
                e.StartTime = DateTime.Now;
                
                Load(useAsyncMode);

                e.SqlResults = sqlResults;
                e.VobDefByID = vobDefByID;
                e.EndTime = DateTime.Now;
                handler?.Invoke(this, e);
            }
        }

        public override void Load (bool useAsyncMode)
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
            var commandQueue = new List<string>();
            var cgi = GetColGetTypeInfo();
            for (int t = 0; t < cgi.Count; t++)
            {
                StringBuilder commandSB = new StringBuilder();

                // select columns in order (by their names) --> SELECT col1, col2, ... coln
                commandSB.Append("SELECT ");
                int lastColumnIndex = cgi[t].Count - 1;
                for (int c = 0; c < cgi[t].Count; c++)
                {
                    if (c != lastColumnIndex)
                    {
                        commandSB.Append(cgi[t][c].colName + ",");
                    }
                    else
                    {
                        commandSB.Append(cgi[t][c].colName);
                    }
                }

                // always sort by <nameOfTable>ID --> e.g. FROM WorldEffect WHERE 1 ORDER BY WorldEffectID;
                commandSB.AppendFormat(" FROM {0} WHERE 1 ORDER BY {1}ID;", DBTableLoadOrder[t], 
                    DBTableLoadOrder[t]);
                commandQueue.Add(commandSB.ToString());
            }
            
            // send out a asynchronous / parallel working DBAgent which informs back when finished with the queue
            DBAgent dbAgent = new DBAgent(GetDBFilePath(), commandQueue, false, useAsyncMode);
            dbAgent.SetObjName(GetObjName() + "-DBAgent");
            dbAgent.FinishedQueue += VobDefFromSQLResults;
            dbAgent.Start();
        }

        protected void VobDefFromSQLResults (AbstractRunnable s, DBAgent.FinishedQueueEventHandlerArgs e)
        {
            try
            {
                var cgi = GetColGetTypeInfo();
                // convert the received database-results
                List<List<List<object>>> sqlResults = e.GetSQLResults();
                DBTables.ConvertSQLResults(sqlResults, cgi);

                int i_VobDef = DBTableLoadOrder.IndexOf("VobDef");
                List<DBTables.ColumnGetTypeInfo> cgt_VobDef = cgi[i_VobDef];
                List<List<object>> tableVobDef = sqlResults[DBTableLoadOrder.IndexOf("VobDef")];

                int i_VobDefEffect = DBTableLoadOrder.IndexOf("VobDefEffect");
                List<DBTables.ColumnGetTypeInfo> cgt_VobDefEffect = cgi[i_VobDefEffect];
                List<List<object>> tableVobDefEffect = sqlResults[DBTableLoadOrder.IndexOf("VobDefEffect")];

                Dictionary<int, Effect> effectByID = null;
                //EffectLoader effectLoader = new EffectLoader(dbFilePath, "Vob");
                EffectLoader effectLoader = new EffectLoader(GetDBFilePath(), "DefEffect", "DefChange");
                effectLoader.Load(true, (object sender, EffectLoader.FinishedLoadingEffectsArgs eff) =>
                {
                    effectByID = eff.EffectsByID;
                });

                if ((effectByID == null) || (effectByID.Count < 1))
                {
                    MakeLogError("Aborting generation of VobDef because no Effects were loaded!");
                    return;
                }

                // find out type of VobDef, dependencies and maybe create it and add all the Effects or postpone
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to finish generating VodDef-objects from sqlResults: " + ex);
            }
        }

        public bool TryFindVobDefType (Dictionary<int, Effect> effectByID, out VobDefType vobDefType)
        {
            vobDefType = VobDefType.Undefined;
            if (effectByID == null)
            {
                MakeLogWarning("Provided null-value for effectByID in TryFindVobDefType!");
                return false;
            }
            return TryFindVobDefType(effectByID, effectByID.Keys.ToList(), out vobDefType);
        }
        
        public bool TryFindVobDefType (Dictionary<int, Effect> effectByID, List<int> effectIDs, 
            out VobDefType vobDefType)
        {
            // checks, checks, checks
            vobDefType = VobDefType.Undefined;
            if (effectByID == null)
            {
                MakeLogWarning("Provided null-value for effectByID in TryFindVobDefType!");
                return false;
            }
            if (effectByID.Count < 1)
            {
                MakeLogWarning("Provided empty effectByID in TryFindVobDefType!");
                return false;
            }
            if (effectIDs == null)
            {
                MakeLogWarning("Provided null-value for effectIDs in TryFindVobDefType!");
                return false;
            }
            if (effectIDs.Count < 1)
            {
                MakeLogWarning("Provided empty effectIDs in TryFindVobDefType!");
                return false;
            }
            
            

            return true;
        }

    }
}
