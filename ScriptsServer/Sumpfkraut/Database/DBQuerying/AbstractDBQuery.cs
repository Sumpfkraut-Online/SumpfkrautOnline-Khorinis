using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Database.DBQuerying
{
    public abstract class AbstractDBQuery : GUC.Utilities.ExtendedObject, IDBQuery
    {

        new public static readonly String _staticName = "AbstractDBQuery (static)";

        protected String sqlCommand;
        public String GetSqlCommand () { return this.sqlCommand; }

        protected DBReaderMode dbReaderMode;
        public DBReaderMode GetDBReaderMode () { return this.dbReaderMode; }



        public AbstractDBQuery (String sqlCommand)
            : this(sqlCommand, DBReaderMode.loadData)
        { }

        public AbstractDBQuery (String sqlCommand, DBReaderMode dbReaderMode)
        {
            SetObjName("AbstractDBQuery (default)");
            this.sqlCommand = sqlCommand;
            this.dbReaderMode = dbReaderMode;
        }



        public void HandleQuery ()
        {
            // execute command-string
            List<List<List<object>>> results = new List<List<List<object>>>();
            switch (dbReaderMode)
            {
                case DBReaderMode.loadData:
                    DBReader.LoadFromDB(ref results, sqlCommand);
                    break;
                case DBReaderMode.saveData:
                    DBReader.SaveToDB(sqlCommand);
                    break;
                default:
                    MakeLogWarning("Invalid dbReaderMode=" + dbReaderMode 
                        + "detected in HandleQuery!");
                    break;
            }
            

            ReturnResults(results);
        }

        public abstract void ReturnResults(List<List<List<object>>> results);
    }
}
