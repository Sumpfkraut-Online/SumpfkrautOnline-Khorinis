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
                "ModelDef", new List<DBTables.ColumnGetTypeInfo>
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
                "ScriptOverlay", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptOverlayID",    SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ModelDefID",         SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptOverlayID",    SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("CodeName",           SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("ScriptOverlayName",  SQLiteGetType.GetString),
                }
            },
            {
                "ScriptAniJob", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniJobID",     SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("AniName",            SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("CodeName",           SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("AniJobType",         SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("NextScriptAniJobID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("Layer",              SQLiteGetType.GetInt32),
                }
            },
            {
                "ScriptAni", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniID",        SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptOverlayID",    SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptAniJobID",     SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("FPS",                SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("StartFrame",         SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("EndFrame",           SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("SpecialFrames",      SQLiteGetType.GetString),
                }
            },
        };

        // fixed load order when accessing the database tables
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "ModelDef", "ScriptOverlay", "ScriptAniJob", "ScriptAni"
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
                PrepareLoadCommand("ModelDef", colGetTypeInfo[0]),
                PrepareLoadCommand("ScriptOverlay", colGetTypeInfo[1]),
                PrepareLoadCommand("ScriptAniJob", colGetTypeInfo[2]),
                PrepareLoadCommand("ScriptAni", colGetTypeInfo[3]),
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

        public void VisualsFromSQLResults (AbstractRunnable sender, FinishedQueueEventHandlerArgs e)
        {
            lock (loadLock)
            {
                sqlResults = e.GetSQLResults();

                if (sqlResults == null) { return; }

                // convert the data-strings to their respective types
                ConvertSQLResults(sqlResults, colGetTypeInfo);

                var tableModelDef       = sqlResults[DBTableLoadOrder.IndexOf("ModelDef")];
                var tableScriptOverlay  = sqlResults[DBTableLoadOrder.IndexOf("ScriptOverlay")];
                var tableScriptAniJob   = sqlResults[DBTableLoadOrder.IndexOf("ScriptAniJob")];
                var tableScriptAni      = sqlResults[DBTableLoadOrder.IndexOf("ScriptAni")];

                Dictionary<int, ScriptAni> aniByAniJobID;
                Dictionary<int, ScriptAni> aniByOverlayID;
                if (!TryGenerateScriptAnis(tableScriptAni, 
                    out aniByAniJobID, out aniByOverlayID))
                {
                    MakeLogError("Failed to produce ScriptAni-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                Dictionary<int, ScriptOverlay> overlayByModelDefID;
                Dictionary<int, ScriptOverlay> overlayByAniID;
                if (!TryGenerateScriptOverlays(tableScriptOverlay, 
                    out overlayByModelDefID, out overlayByAniID))
                {
                    MakeLogError("Failed to produce ScriptOverlay-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                Dictionary<int, ScriptAniJob> aniJobByModelDefID;
                if (!TryGenerateScriptAniJobs(tableScriptAniJob, aniByAniJobID, overlayByAniID, 
                    out aniJobByModelDefID))
                {
                    MakeLogError("Failed to produce ScriptAniJob-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                Dictionary<int, ModelDef> modelDefByID;
                if (!TryGenerateModelDefs(tableModelDef, overlayByModelDefID, aniJobByModelDefID, 
                    out modelDefByID))
                {
                    MakeLogError("Failed to produce ModelDef-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }
            }
        }

        public bool TryGenerateModelDefs (List<List<object>> dataTable, 
            Dictionary<int, ScriptOverlay> overlayByModelDefID, Dictionary<int, ScriptAniJob> aniJobByModelDefID,
            out Dictionary<int, ModelDef> modelDefByID)
        {
            modelDefByID = new Dictionary<int, ModelDef>(dataTable.Count);

            List<ColumnGetTypeInfo> info;
            if (!TryGetColGetTypeInfo("ModelDef", out info)) { return false; }

            try
            {
                info = new List<ColumnGetTypeInfo>(dataTable.Count);
                foreach (List<object> row in dataTable)
                {
                    var modelDef = new ModelDef();
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ModelDefName":
                                modelDef.CodeName = (string) row[c];
                                break;
                            case "Visual":
                                modelDef.Visual = (string) row[c];
                                break;
                            case "AniCatalog":
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
                    modelDefs.Add(modelDef);
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

        public bool TryGenerateScriptOverlays (List<List<object>> dataTable,
            out Dictionary<int, ScriptOverlay> overlayByModelDefID, out Dictionary<int, ScriptOverlay> overlayByAniJobID)
        {
            overlayByModelDefID = new Dictionary<int, ScriptOverlay>();
            overlayByAniJobID = new Dictionary<int, ScriptOverlay>();

            List<ColumnGetTypeInfo> info;
            if (!TryGetColGetTypeInfo("ScriptOverlay", out info)) { return false; }

            try
            {
                info = new List<ColumnGetTypeInfo>(dataTable.Count);
                foreach (List<object> row in dataTable)
                {
                    var overlay = new ScriptOverlay();
                    var modelDefID = -1;
                    var aniJobID = -1;
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
                            case "CodeName":
                                overlay.CodeName = (string) row[c];
                                break;
                            case "ScriptOverlayName":
                                overlay.Name = (string) row[c];
                                break;
                        }
                    }
                    if (modelDefID > -1) { overlayByModelDefID.Add(modelDefID, overlay); }
                    if (aniJobID > -1) { overlayByAniJobID.Add(aniJobID, overlay); }
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

        public bool TryGenerateScriptAniJobs (List<List<object>> dataTable, 
            Dictionary<int, ScriptAni> aniByAniJobID, Dictionary<int, ScriptOverlay> overlayByAniJobID, 
            out Dictionary<int, ScriptAniJob> aniJobByModelDefID)
        {
            aniJobByModelDefID = new Dictionary<int, ScriptAniJob>();
            var nextIDByCurrID = new Dictionary<int, int>();
            var aniJobByID = new Dictionary<int, ScriptAniJob>(dataTable.Count);

            List<ColumnGetTypeInfo> info;
            if (!TryGetColGetTypeInfo("ScriptAniJob", out info)) { return false; }

            try
            {
                info = new List<ColumnGetTypeInfo>(dataTable.Count);
                foreach (List<object> row in dataTable)
                {
                    var aniJob = new ScriptAniJob();
                    var currID = -1;
                    var nextID = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptAniJobID":
                                currID = (int) row[c];
                                break;
                            case "AniName":
                                aniJob.AniName = (string) row[c];
                                break;
                            case "CodeName":
                                aniJob.CodeName = (string) row[c];
                                break;
                            //case "AniJobType":
                            //    // TO DO
                            //    break;
                            case "NextScriptAniID":
                                nextID = (int) row[c];
                                break;
                            case "Layer":
                                aniJob.Layer = (int) row[c];
                                break;
                        }
                    }
                    
                    if (currID > -1)
                    {
                        aniJobByID.Add(currID, aniJob);
                        if (nextID > -1) { nextIDByCurrID.Add(currID, nextID);  }
                    }
                }

                foreach (var keyVal in nextIDByCurrID)
                {
                    aniJobByID[keyVal.Key].NextAni = aniJobByID[keyVal.Value];
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

        public bool TryGenerateScriptAnis (List<List<object>> dataTable, 
            out Dictionary<int, ScriptAni> aniByAniJobID, out Dictionary<int, ScriptAni> aniByOverlayID)
        {
            aniByAniJobID = new Dictionary<int, ScriptAni>();
            aniByOverlayID = new Dictionary<int, ScriptAni>();

            List<ColumnGetTypeInfo> info;
            if (!TryGetColGetTypeInfo("ScriptAni", out info)) { return false; }

            try
            {
                info = new List<ColumnGetTypeInfo>(dataTable.Count);
                foreach (List<object> row in dataTable)
                {
                    var ani = new ScriptAni();
                    var overlayID = -1;
                    var aniJobID = -1;
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "ScriptAniJobID":
                                aniJobID = (int) row[c];
                                break;
                            case "ScriptOverlayID":
                                overlayID = (int) row[c];
                                break;
                            case "FPS":
                                ani.FPS = (int) row[c];
                                break;
                            case "StartFrame":
                                ani.StartFrame = (float) row[c];
                                break;
                            case "EndFrame":
                                ani.EndFrame = (float) row[c];
                                break;
                            case "SpecialFrames":
                                // must split something like "Combo=1;..." and apply key and val
                                var temp = ((string) row[c]).Split(new char[] { ';' });
                                foreach (var t in temp)
                                {
                                    var keyAndVal = t.Split('=');
                                    ani.SetSpecialFrame((SpecialFrame) Enum.Parse(typeof(SpecialFrame), keyAndVal[0]),
                                        float.Parse(keyAndVal[1]));
                                }
                                break;
                        }
                    }
                    if (aniJobID > -1) { aniByAniJobID.Add(aniJobID, ani); }
                    if (overlayID > -1) { aniByOverlayID.Add(overlayID, ani); }
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

    }

}
