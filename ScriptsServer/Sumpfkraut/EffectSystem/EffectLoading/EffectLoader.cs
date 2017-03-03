using GUC.Scripts.Sumpfkraut.Database;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using GUC.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static GUC.Scripts.Sumpfkraut.Database.DBAgent;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public class EffectLoader : ExtendedObject
    {

        new public static readonly string _staticName = "EffectLoader (static)";

        public static char[] defaultParamsSeperator = new char[] { ';' };

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
                    new DBTables.ColumnGetTypeInfo("ChangeType", SQLiteGetType.GetString),
                    new DBTables.ColumnGetTypeInfo("Params", SQLiteGetType.GetString),
                    new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetType.GetDateTime),
                    new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetType.GetDateTime),
                }
            },
        };

        // fixed load order when accessing the database tables
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "DefEffect", "DefChange"
        };

        // uses DBTableLoadOrder and arranges the GetTypeInfos for result-data-conversion for later reusability
        protected static List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo = null;
        public static List<List<DBTables.ColumnGetTypeInfo>> ColGetTypeInfo { get { return colGetTypeInfo; } }

        public delegate void FinishedLoadingHandler (FinishedLoadingHandlerArgs e);
        public class FinishedLoadingHandlerArgs
        {
            public DateTime startTime;
            public DateTime endTime;

            public List<List<List<object>>> sqlResults;
            public Dictionary<int, Effect> effectsByID;
        }



        protected object loadLock;

        protected string dbFilePath = null;
        public string GetDBFilePath () { return this.dbFilePath; }
        public void SetDBFilePath (string value)
        {
            lock (loadLock) { dbFilePath = value; }
        }

        protected Dictionary<int, Effect> effectsByID;
        public Dictionary<int, Effect> GetLastEffectsByID ()
        {
            lock (loadLock) { return effectsByID; }
        }

        protected List<List<List<object>>> sqlResults;
        public List<List<List<object>>> GetLastSQLResults ()
        {
            lock (loadLock) { return sqlResults; }
        }



        public EffectLoader (string dbFilePath)
        {
            SetObjName("EffectLoader");
            SetDBFilePath(dbFilePath);
        }



        // simply null loaded sqlResults and effectsByID
        public void DropResults ()
        {
            lock (loadLock)
            {
                sqlResults = null;
                effectsByID = null;
            }
        }

        // fill in effectsByID-property and optionally invoke a handler to 
        // directly use the results after finished loading
        public void Load (string dbFilePath, bool useAsyncMode = false, FinishedLoadingHandler handler = null)
        {
            FinishedLoadingHandlerArgs e = new FinishedLoadingHandlerArgs();
            e.startTime = DateTime.Now;
   
            lock (loadLock)
            {
                // drop all previous results in one fell swoop
                DropResults();

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

                // send out DBAgent which might work asynchronously
                // ...use AutoResetEvent for that case (forces thread to wait until continue is signalled)
                DBAgent dbAgent = new DBAgent(dbFilePath, commandQueue, false, useAsyncMode);
                dbAgent.SetObjName("DBAgent (EffectLoader)");
                dbAgent.FinishedQueue += EffectsFromSQLResults;
                dbAgent.waitHandle.WaitOne();
                dbAgent.Start();

                // hand all loadResults to a possibly defined handler-function
                e.sqlResults = sqlResults;
                e.effectsByID = effectsByID;
                e.endTime = DateTime.Now;
                handler?.Invoke(e);
            }
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
                    DBTables.ConvertSQLResults(sqlResults, colGetTypeInfo);

                    int i_DefEffect = DBTableLoadOrder.IndexOf("DefEffect");
                    List<DBTables.ColumnGetTypeInfo> cgt_DefEffect = ColGetTypeInfo[i_DefEffect];
                    List<List<object>> tableEffect = sqlResults[ DBTableLoadOrder.IndexOf("DefEffect") ];

                    int i_DefChange = DBTableLoadOrder.IndexOf("DefChange");
                    List<DBTables.ColumnGetTypeInfo> cgt_DefChange = ColGetTypeInfo[i_DefChange];
                    List<List<object>> tableChange = sqlResults[ DBTableLoadOrder.IndexOf("DefChange") ];

                    // create the Effects without assigning them an EffectHandler (must be done in external routine)
                    effectsByID = new Dictionary<int, Effect>();
                    Effect effect = null;
                    Change change = null;
                    int effectID, changeID;
                    ChangeType changeType;
                    string paramsString = null;
                    List<object> parameters = null;
                    ChangeInitInfo changeInitInfo = null;
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

                        // create change and add to it's respective effect in effectsByID
                        change = Change.Create(changeType, parameters);
                        if (effectsByID.TryGetValue(effectID, out effect))
                        {
                            effect.AddChange(change);
                        }
                        else
                        {
                            effect = new Effect(null, new List<Change>() { change });
                            effectsByID.Add(effectID, effect);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Error while converting sqlResults to Effects: " + ex);
            }
            // no return value necessary because final results, effectsByID, is already saved as property in the loader
        }

        public bool TryParseParameters (string parameterString, List<Type> types, 
            out List<object> parameters)
        {
            return TryParseParameters(parameterString, types, out parameters, defaultParamsSeperator);
        }

        public bool TryParseParameters (string parameterString, List<Type> types, 
            out List<object> parameters, string seperator)
        {
            return TryParseParameters(parameterString, types, out parameters, seperator);
        }

        // try parse parameter-string into a List of usable paramters of their respective types
        public bool TryParseParameters (string parameterString, List<Type> types, 
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
                if (!DBTables.TrySqlStringToData(splitted[i], t, out p))
                {
                    MakeLogError(string.Format("Aborting TryParseParameters because Params[{0}]" 
                        + "couldn't be converted according to applied SQLiteGetType {1}",
                        splitted[i], types[i]));
                }
                parameters[i] = p;
            }

            return true;
        }

    }

}
