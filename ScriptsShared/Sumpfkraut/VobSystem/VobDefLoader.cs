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
using GUC.Utilities;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem
{
    public partial class VobDefLoader : BaseLoader
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

        public static List<ChangeType> DependencyChangeTypes = new List<ChangeType>()
        {
            ChangeType.Effect_Parent_Add,
        };

        public partial struct IDAndEffectIDs
        {
            public int ID;
            public List<int> EffectIDs;

            public IDAndEffectIDs (int id, List<int> effectIDs)
            {
                ID = id;
                EffectIDs = effectIDs ?? new List<int>();
            }
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

                List<IDAndEffectIDs> idAndEffectIDList = null;
                if (!TryGenerateIDAndEffectIDList(tableVobDefEffect, out idAndEffectIDList))
                {
                    MakeLogWarning("The provided parameter tableVobDefEffect was either null "
                        + "or didn't contain any elements!");
                }

                List<int> failedIndices;
                if (!TryGenerateVobDefs(idAndEffectIDList, effectByID, out vobDefByID, out failedIndices))
                {
                    MakeLogError("Generation of VobDef failed!");

                    StringBuilder sb = new StringBuilder();
                    sb.Append("Failed to generate VobDef with [temporary index | VobDefID]: ");
                    for (int i = 0; i < failedIndices.Count; i++)
                    {
                        sb.Append("[");
                        sb.Append(i);
                        sb.Append("|");
                        if ((failedIndices[i] < 0) || (failedIndices[i] > (idAndEffectIDList.Count - 1)))
                        {
                            sb.Append("?");
                        }
                        else
                        {
                            sb.Append(failedIndices[i]);
                        }
                        sb.Append("],");
                    }
                    MakeLogError(sb.ToString());

                    return;
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to finish generating VodDef-objects from sqlResults: " + ex);
            }
        }

        /// <summary>
        /// Create VobDef-objects from Effect definitions aas well as assigning them ids.
        /// </summary>
        /// <param name="idAndEffectIDList"></param>
        /// <param name="effectByID"></param>
        /// <param name="vobDefByID"></param>
        /// <param name="failedIndices"></param>
        /// <returns></returns>
        protected bool TryGenerateVobDefs (
            List<IDAndEffectIDs> idAndEffectIDList, 
            Dictionary<int, Effect> effectByID, 
            out Dictionary<int, VobDef> vobDefByID, 
            out List<int> failedIndices)
        {
            // find out type of VobDef, dependencies and maybe create it and add all the Effects or postpone
            vobDefByID = new Dictionary<int, VobDef>(idAndEffectIDList.Count);
            failedIndices = null;

            int lastRemainingCount = int.MaxValue;
            List<int> remainingIndices = null;
            do
            {
                remainingIndices = GenerateVobDef(vobDefByID, idAndEffectIDList, 
                    effectByID, remainingIndices);
            }
            while ((remainingIndices.Count > 0) && (remainingIndices.Count < lastRemainingCount));

            // the still remaining indices failed to be resolved
            if (remainingIndices.Count > 0)
            {
                failedIndices = remainingIndices;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create a VobDef-object from its Effect definitions.
        /// </summary>
        /// <param name="vobDefByID"></param>
        /// <param name="idAndEffectIDList"></param>
        /// <param name="effectByID"></param>
        /// <param name="targetIndices"></param>
        /// <returns></returns>
        protected List<int> GenerateVobDef (Dictionary<int, VobDef> vobDefByID,
            List<IDAndEffectIDs> idAndEffectIDList, Dictionary<int, Effect> effectByID,
            List<int> targetIndices)
        {
            List<int> failedIndices = new List<int>();

            if (targetIndices != null)
            {
                // use specified indices to iterate over
                int index;
                for (int i = 0; i < targetIndices.Count; i++)
                {
                    index = targetIndices[i];
                    if ((index < 0) || (index > (idAndEffectIDList.Count - 1)))
                    {
                        MakeLogWarning("Out of bounds index " + index + " in GenerateVobDef!");
                        failedIndices.Add(index);
                        continue;
                    }
                    if (!TryGenerateVobDef(vobDefByID, idAndEffectIDList[index], 
                        effectByID)) { failedIndices.Add(index); }
                }
            }
            else
            {
                // whole List will be iterated without any target indices specified
                for (int i = 0; i < idAndEffectIDList.Count; i++)
                {
                    if (!TryGenerateVobDef(vobDefByID, idAndEffectIDList[i], 
                        effectByID))
                    {
                        failedIndices.Add(i);
                    }
                }
            }

            return failedIndices;
        }

        /// <summary>
        /// Try to create a VobDef-object from its Effect definitions.
        /// </summary>
        /// <param name="vobDefByID"></param>
        /// <param name="idAndEffectIDs"></param>
        /// <param name="effectByID"></param>
        /// <returns></returns>
        protected bool TryGenerateVobDef (Dictionary<int, VobDef> vobDefByID,
            IDAndEffectIDs idAndEffectIDs, Dictionary<int, Effect> effectByID)
        {
            Effect effect;
            List<Effect> effects = new List<Effect>();
            foreach (var effectID in idAndEffectIDs.EffectIDs)
            {
                if (effectByID.TryGetValue(effectID, out effect))
                {
                    if (ContainsUnresolvedDependencies(effect)) { return false; }
                    effects.Add(effect);
                }
            }

            VobType vobType;
            if (!TryFindVobDefType(effectByID, idAndEffectIDs.EffectIDs, out vobType))
            {
                MakeLogError("Couldn't find VobDefType for VobDef of id: " + idAndEffectIDs.ID);
            }

            VobDef vobDef = null;
            switch (vobType)
            {
                case VobType.Vob:
                    vobDef = new VobDef();
                    break;
                case VobType.NamedVob:
                    vobDef = new NamedVobDef();
                    break;
                case VobType.Item:
                    vobDef = new ItemDef();
                    break;
                case VobType.NPC:
                    vobDef = new NPCDef();
                    break;
                default:
                    MakeLogError("The VobType " + vobType + " is not supported by the loading process!");
                    return false;
            }

            // get the effectHander and hand all effects to it
            vobDef.EffectHandler.AddEffects(effects, true);

            return true;
        }

        protected bool ContainsUnresolvedDependencies (List<Effect> dependencies)
        {
            // TO DO when VobDef are able to inherit from each other

            //if ((dependencies == null) || (dependencies.Count < 1)) { return false; }

            //foreach (var effect in dependencies)
            //{
            //    if (ContainsUnresolvedDependencies(effect.GetChanges())) { return true; }
            //}

            return false;
        }

        protected bool ContainsUnresolvedDependencies (Effect effect)
        {
            if (effect == null) { return false; }
            return ContainsUnresolvedDependencies(effect.GetChanges());
        } 

        protected bool ContainsUnresolvedDependencies (List<Change> dependencies)
        {
            //if ((dependencies == null) || (dependencies.Count < 1)) { return false; }

            //ChangeType changeType;
            //try
            //{
            //    for (int i = 0; i < dependencies.Count; i++)
            //    {
            //        changeType = dependencies[i].GetChangeType();
            //        switch (changeType)
            //        {
            //            // TO DO when VobDef are able to inherit from each other
            //            case ChangeType.:
            //                string parentGlobalID = (string) dependencies[i].GetParameters()[0];
            //                if (!Effect.GlobalEffectExists(parentGlobalID)) { return true; }
            //                break;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MakeLogError(ex);
            //    return true;
            //}

            return false;
        }

        protected bool TryGenerateIDAndEffectIDList (List<List<object>> tableVobDefEffect, 
            out List<IDAndEffectIDs> idAndEffectIDs)
        {
            idAndEffectIDs = new List<IDAndEffectIDs>(tableVobDefEffect.Count);
            if ((tableVobDefEffect == null) || (tableVobDefEffect.Count < 1)) { return false; }

            int id;
            int effectID;
            try
            {
                lock (loadLock)
                {
                    for (int i = 0; i < tableVobDefEffect.Count; i++)
                    {
                        id = (int) tableVobDefEffect[i][0];
                        effectID = (int) tableVobDefEffect[i][1];
                        
                        if ((idAndEffectIDs.Count > 0) && (idAndEffectIDs[idAndEffectIDs.Count - 1].ID == id))
                        {
                            idAndEffectIDs[idAndEffectIDs.Count - 1].EffectIDs.Add(effectID);
                        }
                        else
                        {
                            idAndEffectIDs.Add(new IDAndEffectIDs(id, new List<int>() { effectID }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
                return false;
            }

            return true;
        }

        protected bool TryFindVobType (Dictionary<int, Effect> effectByID, out VobType vobType)
        {
            vobType = VobType.Undefined;
            if (effectByID == null)
            {
                MakeLogWarning("Provided null-value for effectByID in TryFindVobType!");
                return false;
            }
            return TryFindVobDefType(effectByID, effectByID.Keys.ToList(), out vobType);
        }
        
        protected bool TryFindVobDefType (Dictionary<int, Effect> effectByID, List<int> effectIDs, 
            out VobType vobType)
        {
            // checks, checks, checks
            vobType = VobType.Undefined;
            if (effectByID == null)
            {
                MakeLogWarning("Provided null-value for effectByID in TryFindVobType!");
                return false;
            }
            if (effectByID.Count < 1)
            {
                MakeLogWarning("Provided empty effectByID in TryFindVobType!");
                return false;
            }
            if (effectIDs == null)
            {
                MakeLogWarning("Provided null-value for effectIDs in TryFindVobType!");
                return false;
            }
            if (effectIDs.Count < 1)
            {
                MakeLogWarning("Provided empty effectIDs in TryFindVobType!");
                return false;
            }

            int effectID;
            Effect effect = null;
            for (int e = 0; e < effectIDs.Count; e++)
            {
                effectID = effectIDs[e];
                if (!effectByID.TryGetValue(effectID, out effect)) { continue; }
                if (TryFindVobDefType(effect, out vobType)) { break; }
            }

            return true;
        }

        protected bool TryFindVobDefType (Effect effect, out VobType vobType)
        {
            vobType = VobType.Undefined;
            foreach (var change in effect.GetChanges())
            {
                if (change.GetChangeType() == ChangeType.Vob_VobType_Set)
                {
                    try
                    {
                        vobType = (VobType) change.GetParameters()[0];
                        break;
                    }
                    catch (Exception ex)
                    {
                        MakeLogError(ex);
                    }
                }
            }

            return true;
        }

        public override void Save (bool useAsyncMode)
        {
            throw new NotImplementedException();
        }
    }
}
