using GUC.Scripts.Sumpfkraut.Database;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using GUC.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static GUC.Scripts.Sumpfkraut.Database.DBAgent;
using static GUC.Scripts.Sumpfkraut.Database.DBTables;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public partial class EffectLoader : BaseLoader
    {

        public static new char[] defaultParamsSeperator = new char[] { ';' };

        public static readonly Dictionary<string, List<ColumnGetTypeInfo>> DBStructure =
            new Dictionary<string, List<ColumnGetTypeInfo>>()
        {
            {
                "Effect", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("DefEffectID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ChangeDate", SQLiteGetType.GetDateTime),
                    new ColumnGetTypeInfo("CreationDate", SQLiteGetType.GetDateTime),
                }
            },
            {
                "Change", new List<DBTables.ColumnGetTypeInfo>
                {
                    new ColumnGetTypeInfo("DefChangeID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("DefEffectID", SQLiteGetType.GetInt32),
                    new ColumnGetTypeInfo("ChangeType", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("Params", SQLiteGetType.GetString),
                    new ColumnGetTypeInfo("ChangeDate", SQLiteGetType.GetDateTime),
                    new ColumnGetTypeInfo("CreationDate", SQLiteGetType.GetDateTime),
                }
            },
        };

        // fixed load order when accessing the database tables
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "Effect", "Change"
        };

        public delegate void FinishedLoadingEffectsHandler (object sender, FinishedLoadingEffectsArgs e);
        public partial class FinishedLoadingEffectsArgs : FinishedLoadingArgs
        {
            public Dictionary<int, Effect> EffectsByID;
        }

        public static List<ChangeType> DependencyChangeTypes = new List<ChangeType>()
        {
            ChangeType.Effect_Parent_Add,
        };

        public partial struct IDAndChanges
        {
            public int ID;
            public List<Change> Changes;

            public IDAndChanges (int id, List<Change> changes)
            {
                ID = id;
                Changes = changes ?? new List<Change>();
            }
        }

        

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

        protected Dictionary<int, Effect> effectByID;
        public Dictionary<int, Effect> GetLastEffectByID ()
        {
            lock (loadLock) { return effectByID; }
        }



        public EffectLoader (string dbFilePath, string effectTableName, string changeTableName)
            : base ("EffectLoader", dbFilePath, DBStructure, DBTableLoadOrder)
        {
            SetEffectTableName(effectTableName);
            SetChangeTableName(changeTableName);
        }



        // simply null loaded sqlResults and effectsByID
        new public void DropResults ()
        {
            lock (loadLock)
            {
                sqlResults = null;
                effectByID = null;
            }
        }



        public void Load (bool useAsyncMode, FinishedLoadingEffectsHandler handler)
        {
            lock (loadLock)
            {
                FinishedLoadingEffectsArgs e = new FinishedLoadingEffectsArgs();
                e.StartTime = DateTime.Now;

                Load(useAsyncMode);

                // hand all loadResults to a possibly defined handler-function
                e.SqlResults = sqlResults;
                e.EffectsByID = effectByID;
                e.EndTime = DateTime.Now;
                handler?.Invoke(this, e);
            }
        }

        public override void Load (bool useAsyncMode)
        {
            lock (loadLock)
            {
                // start prepared, fresh and clean as morning dew
                InitLoad();
                // fill the queue of commands / subsequent sql-database-requests
                List<string> commandQueue = PrepareLoadCommandQueue();
                //foreach (var cmd in commandQueue) { Log.Logger.Log(cmd); }
                
                // send out DBAgent which might work asynchronously
                // ...use AutoResetEvent for that case (forces thread to wait until continue is signalled)
                DBAgent dbAgent = new DBAgent(dbFilePath, commandQueue, false, useAsyncMode);
                dbAgent.SetObjName("DBAgent (EffectLoader)");
                dbAgent.FinishedQueue += EffectsFromSQLResults;
                //dbAgent.waitHandle.WaitOne();
                dbAgent.Start();
            }
        }

        public List<string> PrepareLoadCommandQueue ()
        {
            // assumes that colGetTypeInfo[0] and colGetTypeInfo[1] represent
            // the effect- and the change-tables respectively
            // returns the data well sorted by the database framework (according to sql-order by)
            return new List<string>()
            {
                PrepareLoadCommand(GetEffectTableName(), colGetTypeInfo[0]),
                PrepareLoadCommand(GetChangeTableName(), colGetTypeInfo[1], 
                    "," + GetEffectTableName() + "ID ASC")
            };
        }

        public void EffectsFromSQLResults (AbstractRunnable sender, FinishedQueueEventHandlerArgs e)
        {
            lock (loadLock)
            {
                sqlResults = e.GetSQLResults();

                // return if there is nothing to process
                if ((sqlResults == null) || (sqlResults.Count < 2)) { return; }

                // convert the data-strings to their respective types
                ConvertSQLResults(sqlResults, colGetTypeInfo);
                    
                var tableEffect = sqlResults[ DBTableLoadOrder.IndexOf("Effect") ];
                var tableChange = sqlResults[ DBTableLoadOrder.IndexOf("Change") ];

                List<IDAndChanges> idAndChangesList = null;
                if (!TryGenerateIDAndChanges(tableChange, out idAndChangesList))
                {
                    MakeLogError("Aborting effect generation due to"
                        + " failed generation of Changes from raw database-data!");
                    return;
                }

                List<int> failedIndices;
                if (!TryGenerateEffects(idAndChangesList, out effectByID, out failedIndices))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Failed to generate Effects with [temporary index | EffectID]: ");
                    for (int i = 0; i < failedIndices.Count; i++)
                    {
                        sb.Append("[");
                        sb.Append(i);
                        sb.Append("|");
                        if ((failedIndices[i] < 0) || (failedIndices[i] > (idAndChangesList.Count - 1)))
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
                }
            }
            // no return value necessary because final results, effectsByID, is already saved as property in the loader
        }

        protected bool TryGenerateEffects (List<IDAndChanges> idAndChangesList, 
            out Dictionary<int, Effect> effectsByID, out List<int> failedIndices)
        {
            // create a dummy EffectHandler to register possible global Effects
            var globalsEH = new BaseEffectHandler("TempGlobalsEffectHandler", null, null);
            effectsByID = new Dictionary<int, Effect>();
            failedIndices = null;

            int lastRemainingCount = int.MaxValue;
            List<int> remainingIndices = null;
            do
            {
                remainingIndices = GenerateEffects(effectsByID, idAndChangesList, 
                    globalsEH, remainingIndices);
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

        // create and add effects to effectsByID using idAndChangesList
        // returns List of unfinished / postponed / failed indices in idAndChangesList
        protected List<int> GenerateEffects (Dictionary<int, Effect> effectsByID, 
            List<IDAndChanges> idAndChangesList, BaseEffectHandler globalsEH,
            List<int> targetIndices = null)
        {
            var failedIndices = new List<int>();

            if (targetIndices != null)
            {
                // use specified indices to iterate over
                int index;
                for (int i = 0; i < targetIndices.Count; i++)
                {
                    index = targetIndices[i];
                    if ((index < 0) || (index > (idAndChangesList.Count - 1)))
                    {
                        MakeLogWarning("Out of bounds index " + index + " in GenerateEffects!");
                        failedIndices.Add(index);
                        continue;
                    }
                    if (!TryGenerateEffect(effectsByID, idAndChangesList[index], 
                        globalsEH)) { failedIndices.Add(index); }
                }
            }
            else
            {
                // whole List will be iterated without any target indices specified
                for (int i = 0; i < idAndChangesList.Count; i++)
                {
                    if (!TryGenerateEffect(effectsByID, idAndChangesList[i], 
                        globalsEH)) { failedIndices.Add(i); }
                }
            }

            return failedIndices;
        }

        protected bool TryGenerateEffect (Dictionary<int, Effect> effectsByID, IDAndChanges ec, 
            BaseEffectHandler globalsEH)
        {
            var effect = new Effect();
            globalsEH = globalsEH ?? new BaseEffectHandler("globalsEH", null, null);

            var dependencies = FindDependencies(ec);
            if (ContainsUnresolvedDependencies(dependencies)) { return false; }

            effect = new Effect();
            effect.AddChanges(ec.Changes);
            effectsByID.Add(ec.ID, effect);
            if (DetectIsGlobal(ec)) { globalsEH.AddEffect(effect); }

            return true;
        }

        protected List<Change> FindDependencies (IDAndChanges ec)
        {
            var dependencies = new List<Change>();
            Change change;
            ChangeType changeType;

            try
            {
                for (int i = 0; i < ec.Changes.Count; i++)
                {
                    change = ec.Changes[i];
                    changeType = change.GetChangeType();
                    if (DependencyChangeTypes.Contains(changeType))
                    {
                        dependencies.Add(change);
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
            }

            return dependencies;
        }

        protected bool ContainsUnresolvedDependencies (List<Change> dependencies)
        {
            if ((dependencies == null) || (dependencies.Count < 1)) { return false; }

            ChangeType changeType;
            try
            {
                for (int i = 0; i < dependencies.Count; i++)
                {
                    changeType = dependencies[i].GetChangeType();
                    switch (changeType)
                    {
                        case ChangeType.Effect_Parent_Add:
                            string parentGlobalID = (string) dependencies[i].GetParameters()[0];
                            if (!Effect.GlobalEffectExists(parentGlobalID)) { return true; }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
                return true;
            }

            return false;
        }

        protected bool DetectIsGlobal (IDAndChanges ec)
        {
            for (int i = 0; i < ec.Changes.Count; i++)
            {
                if (ec.Changes[i].GetChangeType() == ChangeType.Effect_GlobalID_Set)
                {
                    return true;
                }
            }

            return false;
        }
        
        protected bool TryGenerateIDAndChanges (List<List<object>> tableChange, 
            out List<IDAndChanges> idAndChangesList)
        {
            idAndChangesList = new List<IDAndChanges>();
            if ((tableChange == null) || (tableChange.Count < 1)) { return false; }

            Change change = null;
            int effectID, changeID;
            ChangeType changeType;
            string paramsString = null;
            List<object> parameters = null;
            ChangeInitInfo changeInitInfo = null;
            try
            {
                lock (loadLock)
                {
                    for (int i = 0; i < tableChange.Count; i++)
                    {
                        changeID = (int) tableChange[i][0];
                        effectID = (int) tableChange[i][1];

                        // translate the changeType 
                        if (!Enum.TryParse((string) tableChange[i][2], out changeType))
                        {
                            MakeLogError(string.Format("Received unsupported ChangeType {0} "
                                + "from database! Aborting initialization of Change with ID {1}.",
                                tableChange[i][2], changeID));
                            continue;
                        }

                        // translate the parameters
                        paramsString = (string) tableChange[i][3];
                        if (!BaseChangeInit.TryGetChangeInitInfo(changeType, out changeInitInfo))
                        {
                            MakeLogError("Aborting initialization of Change with ID " + changeID
                                + " because ChangeInitInfo for retrieving parameter types was not found!");
                            continue;
                        }
                        if (!TryParseParameters(paramsString, changeInitInfo.ParameterTypes, out parameters))
                        {
                            MakeLogError("Aborting initialization of Change because an error occured "
                                + "while parsing respective parameters!");
                            continue;
                        }

                        // finally create the change
                        change = Change.Create(changeType, parameters);
                        if ((idAndChangesList.Count > 0) && (effectID == idAndChangesList[idAndChangesList.Count - 1].ID))
                        {
                            idAndChangesList[i].Changes.Add(change);
                        }
                        else
                        {
                            idAndChangesList.Add(new IDAndChanges(effectID, new List<Change>() { change }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Error while parsing sqlResults to Changes and Effects: " + ex);
                return false;
            }

            return true;
        }

    }

}
