using GUC.Scripts.Sumpfkraut.Database;
using GUC.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GUC.Scripts.Sumpfkraut.Database.DBAgent;
using static GUC.Scripts.Sumpfkraut.Database.DBTables;

namespace GUC.Scripts.Sumpfkraut.Visuals
{

    public class VisualLoader : BaseLoader
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
                }
            },
            {
                "ScriptAni", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("ScriptAniID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptOverlayID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ScriptAniJobID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("Layer", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("Duration", SQLiteGetType.GetInt32),
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



        public override void Load (bool useAsyncMode)
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
                dbAgent.SetObjName("DBAgent (EffectLoader)");
                dbAgent.FinishedQueue += VisualsFromSQLResults;
                //dbAgent.waitHandle.WaitOne();
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
            }
        }

    }

}
