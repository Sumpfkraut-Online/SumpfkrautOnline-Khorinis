using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Database.DBQuerying
{
    public abstract partial class AbstractDBQuery : GUC.Utilities.ExtendedObject, IDBQuery
    {

        protected string dataSource = null;
        public string DataSource { get { return dataSource; } }

        protected String sqlCommand;
        public String GetSqlCommand () { return this.sqlCommand; }

        protected DBReaderMode dbReaderMode;
        public DBReaderMode GetDBReaderMode () { return this.dbReaderMode; }



        public AbstractDBQuery (string dataSource, string sqlCommand)
            : this(dataSource, sqlCommand, DBReaderMode.loadData)
        { }

        public AbstractDBQuery (string dataSource, string sqlCommand, DBReaderMode dbReaderMode)
        {
            this.dataSource = dataSource;
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
                    DBReader.LoadFromDB(ref results, sqlCommand, DataSource);
                    break;
                case DBReaderMode.saveData:
                    DBReader.SaveToDB(DataSource, sqlCommand);
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
