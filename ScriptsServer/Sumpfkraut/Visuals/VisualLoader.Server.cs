using GUC.Scripts.Sumpfkraut.Database;
using GUC.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GUC.Scripts.Sumpfkraut.Database.DBAgent;
using static GUC.Scripts.Sumpfkraut.Database.DBTables;

namespace GUC.Scripts.Sumpfkraut.Visuals
{

    public partial class VisualLoader : BaseLoader
    {

        public static readonly Dictionary<string, List<ColumnGetTypeInfo>> DBStructure =
            new Dictionary<string, List<ColumnGetTypeInfo>>()
        {
            {
                "ScriptAni", new List<ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniID",        SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("FPS",                SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("StartFrame",         SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("EndFrame",           SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("SpecialFrames",      SQLiteGetType.GetString),
                }
            },
            {
                "ScriptOverlay", new List<ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptOverlayID",    SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("CodeName",           SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("ScriptOverlayName",  SQLiteGetType.GetString),
                }
            },
            {
                "ScriptAniJob", new List<ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniJobID",     SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("DefaultAniID",       SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("AniName",            SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("CodeName",           SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("NextScriptAniJobID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("Layer",              SQLiteGetType.GetInt32),
                }
            },
            {
                "ModelDef", new List<ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ModelDefID",         SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ModelDefName",       SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("Visual",             SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("AniCatalog",         SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("Radius",             SQLiteGetType.GetFloat),
                    new ColumnGetTypeInfo("Height",             SQLiteGetType.GetFloat),
                    new ColumnGetTypeInfo("FistRange",          SQLiteGetType.GetFloat),
                }
            },
            {
                "OverlayAniJobRelation", new List<ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptOverlayID",    SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptAniJobID",     SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptAniID",        SQLiteGetType.GetInt32),
                }
            },
            {
                "ScriptOverlayModelDef", new List<ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptOverlayID",    SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ModelDefID",         SQLiteGetType.GetInt32),
                }
            },
            {
                "ScriptAniJobModelDef", new List<ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniJobID",     SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ModelDefID",         SQLiteGetType.GetInt32),
                }
            },
        };

        // fixed load order when accessing the database tables
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "ScriptAni", "ScriptOverlay", "ScriptAniJob", "ModelDef", "OverlayAniJobRelation",
            "ScriptOverlayModelDef", "ScriptAniJobModelDef"
        };



        public VisualLoader (string dbFilePath, 
            Dictionary<string, List<DBTables.ColumnGetTypeInfo>> dbStructure, List<string> dbTableLoadOrder) 
            : base("VisualLoader", dbFilePath, dbStructure, dbTableLoadOrder)
        { }



        public List<string> PrepareLoadCommandQueue ()
        {
            // assumes that colGetTypeInfo[0] and colGetTypeInfo[1] represent
            // the effect- and the change-tables respectively
            // returns the data well sorted by the database framework (according to sql-order by)
            return new List<string>()
            {
                
                PrepareLoadCommand("ScriptAni", 
                    colGetTypeInfo[DBTableLoadOrder.IndexOf("ScriptAni")], 
                    "1", "ScriptAniID ASC"),
                PrepareLoadCommand("ScriptOverlay", 
                    colGetTypeInfo[DBTableLoadOrder.IndexOf("ScriptOverlay")], 
                    "1", "ScriptOverlayID ASC"),
                PrepareLoadCommand("ScriptAniJob", 
                    colGetTypeInfo[DBTableLoadOrder.IndexOf("ScriptAniJob")], 
                    "1", "ScriptAniJobID ASC"),
                PrepareLoadCommand("ModelDef", 
                    colGetTypeInfo[DBTableLoadOrder.IndexOf("ModelDef")], 
                    "1", "ModelDefID ASC"),
                PrepareLoadCommand("OverlayAniJobRelation", 
                    colGetTypeInfo[DBTableLoadOrder.IndexOf("OverlayAniJobRelation")],
                    "1", "ScriptAniJobID ASC"),
                PrepareLoadCommand("ScriptOverlayModelDef", 
                    colGetTypeInfo[DBTableLoadOrder.IndexOf("ScriptOverlayModelDef")],
                    "1", "ModelDefID ASC"),
                PrepareLoadCommand("ScriptAniJobModelDef", 
                    colGetTypeInfo[DBTableLoadOrder.IndexOf("ScriptAniJobModelDef")],
                    "1", "ModelDefID ASC"),
            };
        }



        partial void pLoad (bool useAsyncMode)
        {
            lock (loadLock)
            {
                InitLoad();

                // fill the queue of commands / subsequent sql-database-requests
                List<string> commandQueue = PrepareLoadCommandQueue();
                //foreach (var cmd in commandQueue) { Log.Logger.Log(cmd); }
                
                // send out DBAgent which might work asynchronously
                // ...use AutoResetEvent for that case (forces thread to wait until continue is signalled)
                DBAgent dbAgent = new DBAgent(dbFilePath, commandQueue, false, useAsyncMode);
                dbAgent.SetObjName("DBAgent (VisualLoader)");
                dbAgent.FinishedQueue += VisualsFromSQLResults;
                dbAgent.Start();
            }
        }


        /// <summary>
        /// Event handler used to create visuals from completed database query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VisualsFromSQLResults (AbstractRunnable sender, FinishedQueueEventHandlerArgs e)
        {
            lock (loadLock)
            {
                sqlResults = e.GetSQLResults();

                if (sqlResults == null) { return; }

                // convert the data-strings to their respective types
                ConvertSQLResults(sqlResults, colGetTypeInfo);

                var tableOverlayAniJob      = sqlResults[DBTableLoadOrder.IndexOf("OverlayAniJobRelation")];
                var tableOverlayModelDef    = sqlResults[DBTableLoadOrder.IndexOf("ScriptOverlayModelDef")];
                var tableAniJobModelDef     = sqlResults[DBTableLoadOrder.IndexOf("ScriptAniJobModelDef")];

                var tableScriptAni          = sqlResults[DBTableLoadOrder.IndexOf("ScriptAni")];
                var tableScriptOverlay      = sqlResults[DBTableLoadOrder.IndexOf("ScriptOverlay")];
                var tableScriptAniJob       = sqlResults[DBTableLoadOrder.IndexOf("ScriptAniJob")];
                var tableModelDef           = sqlResults[DBTableLoadOrder.IndexOf("ModelDef")];

                List<ScriptOverlayAniJobRelation> overlayAniJobRelations = MapOverlayAniJobRelations(tableOverlayAniJob);
                Dictionary<int, List<int>> overlayIDByModelDefID = MapOverlayByModelDef(tableOverlayModelDef);
                Dictionary<int, List<int>> aniJobIDByModelDefID = MapAniJobByModelDef(tableAniJobModelDef);

                Dictionary<int, ScriptAni> aniByID;
                if (!TryGenerateScriptAnis(tableScriptAni, out aniByID))
                {
                    MakeLogError("Failed to produce ScriptAni-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                Dictionary<int, ScriptOverlay> overlayByID;
                if (!TryGenerateScriptOverlays(tableScriptOverlay, 
                    out overlayByID))
                {
                    MakeLogError("Failed to produce ScriptOverlay-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                Dictionary<int, int> nextIDByAniJobID;
                Dictionary<int, ScriptAniJob> aniJobByID;
                if (!TryGenerateScriptAniJobs(tableScriptAniJob, aniByID, 
                    overlayByID, overlayAniJobRelations,
                    out nextIDByAniJobID, out aniJobByID))
                {
                    MakeLogError("Failed to produce ScriptAniJob-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                Dictionary<int, ModelDef> modelDefByID;
                if (!TryGenerateModelDefs(tableModelDef, overlayByID, aniJobByID,
                    overlayIDByModelDefID, aniJobIDByModelDefID, nextIDByAniJobID,
                    out modelDefByID))
                {
                    MakeLogError("Failed to produce ModelDef-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }
            }
        }

        /// <summary>
        /// Create ScriptAni-objects from database table and output them as Dictionary aniByID.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="aniByID"></param>
        /// <returns></returns>
        public bool TryGenerateScriptAnis (List<List<object>> dataTable, 
            out Dictionary<int, ScriptAni> aniByID)
        {
            aniByID = new Dictionary<int, ScriptAni>();

            List<ColumnGetTypeInfo> info;
            TryGetColGetTypeInfo("ScriptAni", out info);

            try
            {
                foreach (List<object> row in dataTable)
                {
                    var ani = new ScriptAni();
                    var aniID = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptAniID":
                                aniID = (int) row[c];
                                break;
                            case "FPS":
                                if (row[c] is int) { ani.FPS = (int)row[c]; }
                                break;
                            case "StartFrame":
                                ani.StartFrame = (int) row[c];
                                break;
                            case "EndFrame":
                                ani.EndFrame = (int) row[c];
                                break;
                            case "SpecialFrames":
                                var rawSF = (string)row[c];
                                if (rawSF.Length == 0) { break; }

                                // must split something like "Combo=1;..." and apply key and val
                                var temp = rawSF.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var t in temp)
                                {
                                    if (t.Length == 0)
                                    {
                                        MakeLogWarning("Invalid SpecialFrame format detected for ScriptAni with id " 
                                            + aniID + " => " + rawSF);
                                        break;
                                    }
                                    var keyAndVal = t.Split('=');
                                    ani.SetSpecialFrame((SpecialFrame) Enum.Parse(typeof(SpecialFrame), keyAndVal[0]),
                                        float.Parse(keyAndVal[1]));
                                }
                                break;
                        }
                    }

                    if (aniID < 0) { throw new Exception("Did't find ScriptAniID in db-data!"); }
                    aniByID.Add(aniID, ani);
                }
            }
            catch (Exception ex)
            {
                info = null;
                MakeLogError(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create ScriptOverlay-objects from database table and output them as Dictionary overlayByID.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="overlayByID"></param>
        /// <returns></returns>
        public bool TryGenerateScriptOverlays (List<List<object>> dataTable,
            out Dictionary<int, ScriptOverlay> overlayByID)
        {
            overlayByID = new Dictionary<int, ScriptOverlay>();

            List<ColumnGetTypeInfo> info;
            TryGetColGetTypeInfo("ScriptOverlay", out info);

            try
            {
                foreach (List<object> row in dataTable)
                {
                    var overlay = new ScriptOverlay();
                    var id = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptOverlayID":
                                id = (int) row[c];
                                break;
                            case "CodeName":
                                overlay.CodeName = (string) row[c];
                                break;
                            case "ScriptOverlayName":
                                overlay.Name = (string) row[c];
                                break;
                        }
                    }
                    if (id < 0) { throw new Exception("No ScriptOverlayID found in db-data."); }
                    overlayByID.Add(id, overlay);
                }
            }
            catch (Exception ex)
            {
                info = null;
                MakeLogError(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create ScriptAniJob-objects from diverse input and output them as Dictionary aniJobByID.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="aniByID"></param>
        /// <param name="overlayByID"></param>
        /// <param name="overlayAniJobRelations"></param>
        /// <param name="aniJobByID"></param>
        /// <returns></returns>
        public bool TryGenerateScriptAniJobs(List<List<object>> dataTable,
            Dictionary<int, ScriptAni> aniByID, Dictionary<int, ScriptOverlay> overlayByID,
            List<ScriptOverlayAniJobRelation> overlayAniJobRelations,
            out Dictionary<int, int> nextIDByAniJobID,
            out Dictionary<int, ScriptAniJob> aniJobByID)
        {
            nextIDByAniJobID = new Dictionary<int, int>();
            aniJobByID = new Dictionary<int, ScriptAniJob>(dataTable.Count);

            List<ColumnGetTypeInfo> info;
            TryGetColGetTypeInfo("ScriptAniJob", out info);

            try
            {
                foreach (List<object> row in dataTable)
                {
                    var aniJob = new ScriptAniJob();
                    var currID = -1;
                    var nextID = -1;
                    var defaultAniID = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptAniJobID":
                                currID = (int) row[c];
                                break;
                            case "DefaultAniID":
                                if (row[c] is null) { break; }
                                defaultAniID = (int) row[c];
                                break;
                            case "AniName":
                                aniJob.AniName = (string) row[c];
                                break;
                            case "CodeName":
                                aniJob.CodeName = (string) row[c];
                                break;
                            case "NextScriptAniJobID":
                                if (row[c] is null) { break; }
                                nextID = (int) row[c];
                                break;
                            case "Layer":
                                if (row[c] is null) { break; }
                                aniJob.Layer = (int) row[c];
                                break;
                        }
                    }

                    Print(string.Format("{0} ({1}) -> {2}", currID, defaultAniID, nextID));

                    if (currID <= -1) { throw new Exception("Didn't find ScriptAniJobID in db-data!"); }
                    if (defaultAniID > -1)
                    {
                        aniJob.SetDefaultAni(aniByID[defaultAniID]);
                    }
                    //Print(defaultAniID + ": " + aniJob.DefaultAni);

                    // prepare for later completion of the initialization after all ScriptAniJobs-objects are present
                    aniJobByID.Add(currID, aniJob);
                    
                    if (nextID > -1) { nextIDByAniJobID.Add(currID, nextID); }
                }

                foreach (var kv in nextIDByAniJobID)
                {
                    aniJobByID[kv.Key].NextAni = aniJobByID[kv.Value];
                }

                foreach (var r in overlayAniJobRelations)
                {
                    var ani = aniByID[r.ScriptAniID];
                    var aniJob = aniJobByID[r.ScriptAniJobID];
                    var overlay = overlayByID[r.ScriptOverlayID];
                    aniJob.AddOverlayAni(ani, overlay);
                }
            }
            catch (Exception ex)
            {
                info = null;
                MakeLogError(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create ModelDef-objects from diverse prepared input and output them as Dictionary modelDefByID.
        /// Finalize initialization of all input objects.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="overlayByID"></param>
        /// <param name="aniJobByID"></param>
        /// <param name="overlayIDByModelDefID"></param>
        /// <param name="aniJobIDByModelDefID"></param>
        /// <param name="modelDefByID"></param>
        /// <returns></returns>
        public bool TryGenerateModelDefs (List<List<object>> dataTable, 
            Dictionary<int, ScriptOverlay> overlayByID, Dictionary<int, ScriptAniJob> aniJobByID,
            Dictionary<int, List<int>> overlayIDByModelDefID, Dictionary<int, List<int>> aniJobIDByModelDefID,
            Dictionary<int, int> nextIDByAniJobID, out Dictionary<int, ModelDef> modelDefByID)
        {
            modelDefByID = new Dictionary<int, ModelDef>(dataTable.Count);

            List<ColumnGetTypeInfo> info;
            TryGetColGetTypeInfo("ModelDef", out info);

            try
            {
                foreach (List<object> row in dataTable)
                {
                    var modelDef = new ModelDef();
                    var id = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ModelDefID":
                                id = (int) row[c];
                                break;
                            case "ModelDefName":
                                modelDef.CodeName = (string) row[c];
                                break;
                            case "Visual":
                                modelDef.Visual = (string) row[c];
                                break;
                            case "AniCatalog":
                                var aniCatalog = (string) row[c];
                                switch (aniCatalog)
                                {
                                    case "NPCCatalog":
                                        modelDef.SetAniCatalog(new AniCatalogs.NPCCatalog());
                                        break;
                                }
                                break;
                            case "Radius":
                                modelDef.Radius = (float) row[c];
                                break;
                            case "Height":
                                modelDef.Height = (float) row[c];
                                break;
                            case "FistRange":
                                modelDef.FistRange = (float) row[c];
                                break;
                        }
                    }

                    if (id < 0) { throw new Exception("No ModelDefID found in db-data!"); }

                    modelDefByID.Add(id, modelDef);
                }

                // finalize initialization by adding ScriptOverlays and ScriptAniJobs
                foreach (var kv in overlayIDByModelDefID)
                {
                    var modelDefID = kv.Key;
                    var overlayIDs = kv.Value;
                    foreach (var overlayID in overlayIDs)
                    {
                        modelDefByID[modelDefID].AddOverlay(overlayByID[overlayID]);
                    }
                }

                foreach (var kv in aniJobIDByModelDefID)
                {
                    var modelDefID = kv.Key;
                    var remains = new List<int>(kv.Value);
                    var added = new List<int>(remains.Count);
                    int next;
                   
                    while (remains.Count > 0)
                    {
                        var newlyAdded = new List<int>();
                        foreach (var r in remains)
                        {
                            // if ScriptAniJob has a followup animation
                            // and it is still missing from the current ModelDef
                            // then wait up until it possibly is
                            if (nextIDByAniJobID.TryGetValue(r, out next))
                            {
                                if (!added.Contains(next)) { continue; }
                            }
                            // otherwise proceed with initialization
                            modelDefByID[modelDefID].AddAniJob(aniJobByID[r]);
                            newlyAdded.Add(r);
                        }
                        added.AddRange(newlyAdded);
                        remains = remains.Except(newlyAdded).ToList();
                        if (newlyAdded.Count < 1)
                        {
                            MakeLogError("Cannor resolve NextAni-dependencies"
                                + " while adding ScriptAniJobs to ModelDef with database-id: " + modelDefID);
                        }
                    }
                }

                foreach (var kv in modelDefByID)
                {
                    kv.Value.Create();
                    Print("Created ModelDef => " + kv.Key + ": " + kv.Value.CodeName);
                }
            }
            catch (Exception ex)
            {
                info = null;
                MakeLogError(ex);
                return false;
            }
            finally { }

            return true;
        }

        /// <summary>
        /// Uses data from database table OverlayAniJobRelation to map ScriptOverlay to their respective ScriptAniJob.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public List<ScriptOverlayAniJobRelation> MapOverlayAniJobRelations (List<List<object>> dataTable)
        {
            var relations = new List<ScriptOverlayAniJobRelation>(dataTable.Count);

            List<ColumnGetTypeInfo> info;
            TryGetColGetTypeInfo("OverlayAniJobRelation", out info);

            try
            {
                foreach (List<object> row in dataTable)
                {
                    var overlayID = -1;
                    var aniJobID = -1;
                    var aniID = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptOverlayID":
                                overlayID = (int) row[c];
                                break;
                            case "ScriptAniJobID":
                                aniJobID = (int) row[c];
                                break;
                            case "ScriptAniID":
                                aniID = (int)row[c];
                                break;
                        }
                    }

                    // map the references
                    if (overlayID < 0) { throw new Exception("No ScriptOverlayID found in db-data!"); }
                    if (aniJobID < 0) { throw new Exception("No ScriptAniJobID found in db-data!"); }
                    if (aniID < 0) { throw new Exception("No ScriptAniID found in db-data!"); }
                    relations.Add(new ScriptOverlayAniJobRelation(overlayID, aniJobID, aniID));
                }
            }
            catch (Exception ex)
            {
                info = null;
                MakeLogError(ex);
            }

            return relations;
        }

        /// <summary>
        /// Uses data from database table ScriptOverlayModelDef to map ScriptOverlay to their respective ModelDef.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public Dictionary<int, List<int>> MapOverlayByModelDef (List<List<object>> dataTable)
        {
            var overlayIDByModelDefID = new Dictionary<int, List<int>>();
            List<int> overlayIDs = null;

            List<ColumnGetTypeInfo> info;
            TryGetColGetTypeInfo("ScriptOverlayModelDef", out info);

            try
            {
                foreach (List<object> row in dataTable)
                {
                    var overlayID = -1;
                    var modelDefID = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptOverlayID":
                                overlayID = (int) row[c];
                                break;
                            case "ModelDefID":
                                modelDefID = (int) row[c];
                                break;
                        }
                    }

                    // map the references
                    if (overlayID < 0) { throw new Exception("No ScriptOverlayID found in db-data!"); }
                    if (modelDefID < 0) { throw new Exception("No ModelDefID found in db-data!"); }

                    if (overlayIDByModelDefID.TryGetValue(modelDefID, out overlayIDs))
                    {
                        overlayIDs.Add(overlayID);
                    }
                    else
                    {
                        overlayIDByModelDefID.Add(modelDefID, new List<int> { overlayID });
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
                MakeLogError(ex);
            }

            return overlayIDByModelDefID;
        }

        /// <summary>
        /// Uses data from database table ScriptAniJobModelDef to map ScriptAniJob to their respective ModelDef.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public Dictionary<int, List<int>> MapAniJobByModelDef (List<List<object>> dataTable)
        {
            var aniJobIDByModelDefID = new Dictionary<int, List<int>>();
            List<int> aniJobIDs = null;

            List<ColumnGetTypeInfo> info;
            TryGetColGetTypeInfo("ScriptAniJobModelDef", out info);

            try
            {
                foreach (var row in dataTable)
                {
                    var aniJobID = -1;
                    var modelDefID = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptAniJobID":
                                aniJobID = (int) row[c];
                                break;
                            case "ModelDefID":
                                modelDefID = (int) row[c];
                                break;
                        }
                    }

                    // map the references
                    if (aniJobID < 0) { throw new Exception("No ScriptAniJobID found in db-data!"); }
                    if (modelDefID < 0) { throw new Exception("No ModelDef found in db-data!"); }

                    if (aniJobIDByModelDefID.TryGetValue(modelDefID, out aniJobIDs))
                    {
                        aniJobIDs.Add(aniJobID);
                    }
                    else
                    {
                        aniJobIDByModelDefID.Add(modelDefID, new List<int> { aniJobID });
                    }
                }

                foreach (var bla in aniJobIDByModelDefID)
                {
                    foreach (var blubb in bla.Value)
                    {
                        Print("### " + bla.Key + ": " + blubb);
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
                MakeLogError(ex);
            }

            return aniJobIDByModelDefID;
        }

    }

}
