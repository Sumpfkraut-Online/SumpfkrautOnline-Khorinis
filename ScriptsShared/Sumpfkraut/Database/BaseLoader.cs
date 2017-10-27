using GUC.Scripts.Sumpfkraut.EffectSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GUC.Scripts.Sumpfkraut.Database.DBTables;

namespace GUC.Scripts.Sumpfkraut.Database
{

    public delegate void FinishedLoadingHandler (object sender, FinishedLoadingArgs e);
    public partial class FinishedLoadingArgs : EventArgs
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public List<List<List<object>>> SqlResults;
    }

    public abstract partial class BaseLoader : GUC.Utilities.ExtendedObject
    {

        public static char[] defaultParamsSeperator = new char[] { ';' };



        protected object loadLock;

        // (must hide it via new-keyword in inherting classes)
        protected Dictionary<string, List<DBTables.ColumnGetTypeInfo>> dbStructure;

        // fixed load order when accessing the database tables
        protected List<string> dbTableLoadOrder;
        public List<string> GetDBTableLoadOrder () { return dbTableLoadOrder; }
        public void SetDBTableLoadOrder (List<string> value)
        {
            lock (loadLock)
            {
                dbTableLoadOrder = value;
            }
        }

        // uses DBTableLoadOrder and arranges the GetTypeInfos for result-data-conversion for later reusability
        protected List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo = null;
        public List<List<DBTables.ColumnGetTypeInfo>> GetColGetTypeInfo () { return colGetTypeInfo; }
        public bool TryGetColGetTypeInfo (string tableName, out List<ColumnGetTypeInfo> info)
        {
            info = null;
            int index = GetDBTableLoadOrder().IndexOf(tableName);
            if (index < 0) { return false; }
            else
            {
                info = GetColGetTypeInfo()[index];
                return true;
            }
        }

        protected string dbFilePath = null;
        public string GetDBFilePath () { return dbFilePath; }
        public void SetDBFilePath (string value)
        {
            lock (loadLock) { dbFilePath = value; }
        }

        protected List<List<List<object>>> sqlResults;
        public List<List<List<object>>> GetLastSQLResults ()
        {
            lock (loadLock) { return sqlResults; }
        }



        protected BaseLoader (string objName, string dbFilePath, 
            Dictionary<string, List<ColumnGetTypeInfo>> dbStructure, List<string> dbTableLoadOrder)
        {
            SetObjName(objName);
            this.loadLock = new object();
            this.dbFilePath = dbFilePath;
            this.dbStructure = dbStructure;
            this.dbTableLoadOrder = dbTableLoadOrder;
        }



        // null previous loading results
        // (override in inherited classes which produce more results than just sqlResults-object)
        public void DropResults ()
        {
            lock (loadLock) { sqlResults = null;}
        }

        // combines PrepareColGetTypeInfo- and DropResults-methods
        public void InitLoad ()
        {
            PrepareColGetTypeInfo();
            DropResults();
        }

        // prepare data conversion parameters if it's still not done yet (done only once per server run)
        public void PrepareColGetTypeInfo ()
        {
            if (colGetTypeInfo != null) { return; }
            colGetTypeInfo = new List<List<ColumnGetTypeInfo>>();
            for (int i = 0; i < dbTableLoadOrder.Count; i++)
            {
                colGetTypeInfo.Add(dbStructure[ dbTableLoadOrder[i] ]);
            }
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
            if (additionalSQLSort != null) { commandSB.Append(additionalSQLSort); }
            commandSB.Append(";");

            return commandSB.ToString();
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



        public abstract void Load (bool useAsyncMode);

        public abstract void Save (bool useAsyncMode);

    }

}
