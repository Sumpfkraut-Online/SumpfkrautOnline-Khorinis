using GUC.Scripts.Sumpfkraut.EffectSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GUC.Scripts.Sumpfkraut.Database.DBTables;

namespace GUC.Scripts.Sumpfkraut.Database
{

    public abstract partial class BaseObjectLoader : GUC.Utilities.ExtendedObject
    {

        new public static readonly string _staticName = "BaseObjectLoader (static)";



        public delegate void FinishedLoadingHandler (object sender, FinishedLoadingArgs e);
        public event FinishedLoadingHandler FinishedLoading;
        public class FinishedLoadingArgs : EventArgs
        {
            public DateTime startTime;
            public DateTime endTime;
            public List<List<List<object>>> sqlResults;
        }

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

        //protected string effectTableName = null;
        //public string GetEffectTableName () { return effectTableName; }
        //public void SetEffectTableName (string value)
        //{
        //    lock (loadLock) { effectTableName = value; }
        //}

        //protected string changeTableName = null;
        //public string GetChangeTableName () { return changeTableName; }
        //public void SetChangeTableName (string value)
        //{
        //    lock (loadLock) { changeTableName = value; }
        //}

        //protected Dictionary<int, Effect> effectsByID;
        //public Dictionary<int, Effect> GetLastEffectsByID ()
        //{
        //    lock (loadLock) { return effectsByID; }
        //}



        protected BaseObjectLoader (string objName, string dbFilePath, 
            Dictionary<string, List<ColumnGetTypeInfo>> dbStructure, List<string> dbTableLoadOrder)
        {
            SetObjName(objName);
            this.loadLock = new object();
            this.dbFilePath = dbFilePath;
            this.dbStructure = dbStructure;
            this.dbTableLoadOrder = dbTableLoadOrder;
        }



        // null prvious loading results
        // (override in inherited classes which produce more results than just sqlResults-object)
        public void DropResults ()
        {
            lock (loadLock) { sqlResults = null;}
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

        public abstract void Load (bool useAsyncMode = false);

    }

}
