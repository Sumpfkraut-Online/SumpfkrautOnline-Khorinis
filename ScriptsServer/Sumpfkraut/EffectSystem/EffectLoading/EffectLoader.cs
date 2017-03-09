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

        public struct IntChangePair
        {
            public int Number;
            public Change Change;

            public IntChangePair (int number, Change change)
            {
                Number = number;
                Change = change;
            }
        }

        public struct IntPair
        {
            public int Number1;
            public int Number2;

            public IntPair (int number1, int number2)
            {
                Number1 = number1;
                Number2 = number2;
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
                    
                    //int i_DefEffect = DBTableLoadOrder.IndexOf("DefEffect");
                    //List<ColumnGetTypeInfo> cgt_DefEffect = colGetTypeInfo[i_DefEffect];
                    var tableEffect = sqlResults[ DBTableLoadOrder.IndexOf("Effect") ];
                    var tableChange = sqlResults[ DBTableLoadOrder.IndexOf("Change") ];

                    List<IntChangePair> effectIDAndChange = null;
                    if (!TryGenerateChanges(tableChange, out effectIDAndChange))
                    {
                        MakeLogError("Aborting effect generation due to"
                            + " failed generation of Changes from raw database-data!");
                        return;
                    }

                    // create a dummy EffectHandler to register possible global Effects
                    var globalsEH = new BaseEffectHandler("TempGlobalsEffectHandler", null, null);

                    // create the Effects without assigning them an EffectHandler (must be done in external routine)

                    //if (!TryGenerateEmptyEffects(tableEffect, out effectsByID))
                    //{
                    //    MakeLogError("Aborting Effect generation due to"
                    //        + " failed generation of empty Effects from raw database-data!");
                    //    return;
                    //}

                    // TODO: fill in changes into the effects 
                    // + continue somewhere else when not all prerequisites are met (i.e. effects from which to inherit)
                    // + jump to necessary effects and create them first

                    //List<int> failedEffectIDs = null;
                    //if (!TryFillInEffects(effectsByID, effectIDAndChange, out failedEffectIDs))
                    //{
                    //    // error message
                    //}


                    //if (effectsByID.TryGetValue(effectID, out effect))
                    //{
                    //    effect.AddChange(change);
                    //}
                    //else
                    //{
                    //    effect = new Effect(null, new List<Change>() { change });
                    //    effectsByID.Add(effectID, effect);
                    //}
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Error while converting sqlResults to Effects: " + ex);
            }
            // no return value necessary because final results, effectsByID, is already saved as property in the loader
        }

        protected bool TryGenerateEffects (List<IntChangePair> effectIDAndChange, 
            out Dictionary<int, Effect> effectsByID)
        {
            effectsByID = new Dictionary<int, Effect>();
            int newlyCreated = 0;
            List<IntPair> targetRanges = new List<IntPair>() { new IntPair(0, effectIDAndChange.Count - 1) };
            List<IntPair> failedRanges = null;

            // as long as it succeeds to generate new Effects, proceed in doing so
            // until hopefully all Effects are generated
            do
            {
                newlyCreated = GenerateEffects(effectIDAndChange, effectsByID, out failedRanges, targetRanges);
                targetRanges = failedRanges;
            }
            while (newlyCreated > 0);

            return true;
        }

        protected int GenerateEffects (List<IntChangePair> effectIDAndChange, 
            Dictionary<int, Effect> effectsByID, out List<IntPair> failedRanges,
            List<IntPair> targetRanges = null)
        {
            int created = 0;
            failedRanges = new List<IntPair>();
            // return if there is nothing to process
            if (effectIDAndChange.Count < 1) { return created; }
            // if targetRanges is not specified, use the whole of effectIDAndChange
            targetRanges = targetRanges ?? new List<IntPair>() { new IntPair(0, effectIDAndChange.Count) };

            int currEffectID = -1;
            int effectStart, effectEnd = -1;
            Effect currEffect = null;
            Change currChange = null;
            ChangeType currChangeType = ChangeType.Undefined;

            for (int r = 0; r < targetRanges.Count; r++)
            {
                for (int i = targetRanges[r].Number1; i <= targetRanges[r].Number2; i++)
                {
                    if (effectIDAndChange[i].Number != currEffectID)
                    {
                        effectEnd = i - 1;

                        // save previously accumulated Effect and go on with the next
                        if (currEffect != null) { effectsByID.Add(currEffectID, currEffect); }
                        currEffectID = effectIDAndChange[i].Number;
                        currEffect = new Effect();

                        effectStart = i;
                    }

                    // detect if effect has to be postponed
                    currChange = effectIDAndChange[i].Change;
                    currChangeType = currChange.GetChangeType();
                    switch (currChangeType)
                    {
                        case ChangeType.
                    }

                    // add the change
                    if (currEffect.AddChange(effectIDAndChange[i].Change) == -1)
                    {
                        // wasn't added
                    }
                }
            }

            return created;
        }

        protected bool TryGenerateChanges (List<List<object>> tableChange, 
            out List<IntChangePair> effectIDAndChange)
        {
            effectIDAndChange = new List<IntChangePair>();
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
                        effectIDAndChange.Add(new IntChangePair(effectID, change));
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
