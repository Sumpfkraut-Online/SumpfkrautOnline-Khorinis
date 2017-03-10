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

    public class EffectLoader : BaseObjectLoader
    {

        new public static readonly string _staticName = "EffectLoader (static)";

        public static char[] defaultParamsSeperator = new char[] { ';' };

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

        public delegate void FinishedLoadingEffectsHandler (FinishedLoadingEffectsArgs e);
        public class FinishedLoadingEffectsArgs : FinishedLoadingArgs
        {
            public Dictionary<int, Effect> effectsByID;
        }

        public static List<ChangeType> DependencyChangeTypes = new List<ChangeType>()
        {
            ChangeType.Effect_Parent_Add,
        };

        public struct EffectChanges
        {
            public int EffectID;
            public List<Change> Changes;

            public EffectChanges (int effectID, List<Change> changes)
            {
                EffectID = effectID;
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

        protected Dictionary<int, Effect> effectsByID;
        public Dictionary<int, Effect> GetLastEffectsByID ()
        {
            lock (loadLock) { return effectsByID; }
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
                effectsByID = null;
            }
        }



        public void Load (bool useAsyncMode, FinishedLoadingEffectsHandler handler)
        {
            lock (loadLock)
            {
                FinishedLoadingEffectsArgs e = new FinishedLoadingEffectsArgs();
                e.startTime = DateTime.Now;

                Load(useAsyncMode);

                // hand all loadResults to a possibly defined handler-function
                e.sqlResults = sqlResults;
                e.effectsByID = effectsByID;
                e.endTime = DateTime.Now;
                handler?.Invoke(e);
            }
        }

        public override void Load (bool useAsyncMode)
        {
            lock (loadLock)
            {
                // start prepared, fresh and clean as morning dew
                PrepareColGetTypeInfo();
                DropResults();
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

        public string PrepareLoadCommand (string tableName, List<ColumnGetTypeInfo> getTypeInfos, 
            string additionalSQLSort = null)
        {
            StringBuilder commandSB = new StringBuilder();

            // select columns in order (by their names) --> SELECT col1, col2, ... coln
            commandSB.Append("SELECT ");
            int lastColumnIndex = getTypeInfos.Count - 1;
            for (int c = 0; c < getTypeInfos.Count; c++)
            {
                if (c != lastColumnIndex)
                {
                    commandSB.Append(getTypeInfos[c].colName + ",");
                }
                else
                {
                    commandSB.Append(getTypeInfos[c].colName);
                }
            }

            commandSB.AppendFormat(" FROM {0} WHERE 1 ORDER BY {0}ID ASC", tableName);
            if (additionalSQLSort != null)
            {
                commandSB.Append(additionalSQLSort);
            }
            commandSB.Append(";");

            return commandSB.ToString();
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
                    ConvertSQLResults(sqlResults, colGetTypeInfo);
                    
                    var tableEffect = sqlResults[ DBTableLoadOrder.IndexOf("Effect") ];
                    var tableChange = sqlResults[ DBTableLoadOrder.IndexOf("Change") ];

                    List<EffectChanges> effectChanges = null;
                    if (!TryGenerateChanges(tableChange, out effectChanges))
                    {
                        MakeLogError("Aborting effect generation due to"
                            + " failed generation of Changes from raw database-data!");
                        return;
                    }

                    List<int> failedIndices;
                    if (!TryGenerateEffects(effectChanges, out effectsByID, out failedIndices))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Failed to generate Effects with [temporary index | EffectID]: ");
                        for (int i = 0; i < failedIndices.Count; i++)
                        {
                            sb.Append("[");
                            sb.Append(i);
                            sb.Append("|");
                            if ((failedIndices[i] < 0) || (failedIndices[i] > (effectChanges.Count - 1)))
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
            }
            catch (Exception ex)
            {
                MakeLogError("Error while converting sqlResults to Effects: " + ex);
            }
            // no return value necessary because final results, effectsByID, is already saved as property in the loader
        }

        protected bool TryGenerateEffects (List<EffectChanges> effectChanges, 
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
                remainingIndices = GenerateEffects(effectsByID, effectChanges, 
                    globalsEH, remainingIndices);
            }
            while ((remainingIndices.Count > 0) && (remainingIndices.Count < lastRemainingCount));

            // the still remaining indices failed to be resolved
            if (remainingIndices.Count > 0) { failedIndices = remainingIndices; }
            return true;
        }

        // create and add effects to effectsByID using effectChanges
        // returns List of unfinished / postponed / failed indices in effectChanges
        protected List<int> GenerateEffects (Dictionary<int, Effect> effectsByID, 
            List<EffectChanges> effectChanges, BaseEffectHandler globalsEH,
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
                    if ((index < 0) || (index > (effectChanges.Count - 1)))
                    {
                        MakeLogWarning("Out of bounds index " + index + " in GenerateEffects!");
                        failedIndices.Add(index);
                        continue;
                    }
                    if (!TryGenerateEffect(effectsByID, effectChanges[index], 
                        globalsEH)) { failedIndices.Add(index); }
                }
            }
            else
            {
                // whole List will be iterated without any target indices specified
                for (int i = 0; i < effectChanges.Count; i++)
                {
                    if (!TryGenerateEffect(effectsByID, effectChanges[i], 
                        globalsEH)) { failedIndices.Add(i); }
                }
            }

            return failedIndices;
        }

        protected bool TryGenerateEffect (Dictionary<int, Effect> effectsByID, EffectChanges ec, 
            BaseEffectHandler globalsEH)
        {
            var effect = new Effect();
            globalsEH = globalsEH ?? new BaseEffectHandler("globalsEH", null, null);

            var dependencies = FindDependencies(ec);
            if (ContainsUnresolvedDependencies(dependencies)) { return false; }

            effect = new Effect();
            effect.AddChanges(ec.Changes);
            effectsByID.Add(ec.EffectID, effect);
            if (DetectIsGlobal(ec)) { globalsEH.AddEffect(effect); }

            return true;
        }

        protected List<Change> FindDependencies (EffectChanges ec)
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
            bool check = false;

            return check;
        }

        protected bool DetectIsGlobal (EffectChanges ec)
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
        
        protected bool TryGenerateChanges (List<List<object>> tableChange, 
            out List<EffectChanges> effectChanges)
        {
            effectChanges = new List<EffectChanges>();
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
                        if ((effectChanges.Count > 0) && (effectID == effectChanges[effectChanges.Count - 1].EffectID))
                        {
                            effectChanges[i].Changes.Add(change);
                        }
                        else
                        {
                            effectChanges.Add(new EffectChanges(effectID, new List<Change>() { change }));
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

        protected bool TryParseParameters (string parameterString, List<Type> types, 
            out List<object> parameters)
        {
            return TryParseParameters(parameterString, types, out parameters, defaultParamsSeperator);
        }

        protected bool TryParseParameters (string parameterString, List<Type> types, 
            out List<object> parameters, string seperator)
        {
            return TryParseParameters(parameterString, types, out parameters, seperator);
        }

        // try parse parameter-string into a List of usable paramters of their respective types
        protected bool TryParseParameters (string parameterString, List<Type> types, 
            out List<object> parameters, char[] seperator)
        {
            parameters = null;
            string[] splitted = parameterString.Split(seperator);
            if (types == null)
            {
                MakeLogError("Aborting TryParseParameters because parameter parameterTypes is null!");
                return false;
            }
            if (types.Count < splitted.Length)
            {
                MakeLogError("Aborting TryParseParameters because the amount of parameterTypes is insufficient: " 
                    + types.Count + " instead of the required " + splitted.Length + ". ");
            }

            parameters = new List<object>(splitted.Length);
            object p;
            Type t;
            for (int i = 0; i < splitted.Length; i++)
            {
                t = types[i];
                if (!TrySqlStringToData(splitted[i], t, out p))
                {
                    MakeLogError(string.Format("Aborting TryParseParameters because Params[{0}]" 
                        + "couldn't be converted according to applied SQLiteGetType {1}",
                        splitted[i], types[i]));
                }
                
                parameters.Add(p);
            }

            return true;
        }

    }

}
