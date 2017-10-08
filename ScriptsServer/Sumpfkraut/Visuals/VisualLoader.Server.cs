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
                    new ColumnGetTypeInfo("ModelDefID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ModelDefName", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("Visual", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("Radius", SQLiteGetType.GetFloat),
                    new ColumnGetTypeInfo("Height", SQLiteGetType.GetFloat),
                    new ColumnGetTypeInfo("FistRange", SQLiteGetType.GetFloat),
                }
            },
            {
                "ScriptOverlay", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptOverlayID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ModelDefID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("CodeName", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("ScriptOverlayName", SQLiteGetType.GetString),
                }
            },
            {
                "ScriptAniJob", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniJobID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptAniID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("AniName", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("CodeName", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("AniJobType", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("PrevCodeName", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("NextCodeName", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("Layer", SQLiteGetType.GetInt32),
                }
            },
            {
                "ScriptAni", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptOverlayID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptAniJobID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("FPS", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("StartFrame", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("EndFrame", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("SpecialFrames", SQLiteGetType.GetString),
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

                var tableModelDef = sqlResults[0];
                var tableScriptOverlay = sqlResults[1];
                var tableScriptAniJob = sqlResults[2];
                var tableScriptAni = sqlResults[3];

                List<ModelDef> modelDefs;
                if (!TryGenerateModelDefs(tableModelDef, out modelDefs))
                {
                    MakeLogError("Failed to produce ModelDef-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                List<ScriptOverlay> scriptOverlays;
                if (!TryGenerateScriptOverlays(tableScriptOverlay, out scriptOverlays))
                {
                    MakeLogError("Failed to produce ScriptOverlay-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                List<ScriptAni> scriptAnis;
                if (!TryGenerateScriptAnis(tableScriptAni, out scriptAnis))
                {
                    MakeLogError("Failed to produce ScriptAni-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                }

                List<ScriptAniJob> scriptAniJobs;
                if (!TryGenerateScriptAniJobs(tableScriptAniJob, out scriptAniJobs))
                {
                    MakeLogError("Failed to produce ScriptAniJob-objects from sql data. "
                        + "Aborting Generation of Visuals.");
                    return;
                } 
            }
        }

        public bool TryGenerateModelDefs (List<List<object>> dataTable, 
            out List<ModelDef> modelDefs)
        {
            modelDefs = null;

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

        public bool TryGenerateScriptOverlays (List<List<object>> dataTable, List<ModelDef> modelDefs, 
            out List<ScriptOverlay> scriptOverlays)
        {
            scriptOverlays = null;

            List<ColumnGetTypeInfo> info;
            if (!TryGetColGetTypeInfo("ScriptOverlay", out info)) { return false; }

            try
            {
                info = new List<ColumnGetTypeInfo>(dataTable.Count);
                foreach (List<object> row in dataTable)
                {
                    var scriptOverlay = new ScriptOverlay();
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            case "CodeName":
                                scriptOverlay.CodeName = (string) row[c];
                                break;
                            case "ScriptOverlayName":
                                scriptOverlay.Name = (string) row[c];
                                break;
                        }
                    }
                    scriptOverlays.Add(scriptOverlay);
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

        public bool TryGenerateScriptAniJobs (List<List<object>> dataTable, 
            out List<ScriptAniJob> scriptAniJobs)
        {
            scriptAniJobs = null;

            List<ColumnGetTypeInfo> info;
            if (!TryGetColGetTypeInfo("ScriptAniJob", out info)) { return false; }

            try
            {
                info = new List<ColumnGetTypeInfo>(dataTable.Count);
                foreach (List<object> row in dataTable)
                {
                    var scriptAniJob = new ScriptAniJob();
                    for (int c = 0; c < row.Count; c++)
                    {
                        switch (info[c].colName)
                        {
                            // ...
                        }
                    }
                    scriptAniJobs.Add(scriptAniJob);
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
            out List<ScriptAni> scriptAnis)
        {
            scriptAnis = null;

            List<ColumnGetTypeInfo> info;
            if (!TryGetColGetTypeInfo("ScriptAni", out info)) { return false; }

            try
            {
                info = new List<ColumnGetTypeInfo>(dataTable.Count);
                foreach (List<object> row in dataTable)
                {
                    // StartFrame, EndFrame, FPS
                    var scriptAni = new ScriptAni((int) row[1], (int) row[2]);
                    scriptAni.FPS = (float) row[0];
                    scriptAnis.Add(scriptAni);
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
