using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Database.DBQuerying
{
    public class DBQuery : AbstractDBQuery
    {

        new public static readonly String _staticName = "DBQuery (static)";
        new protected String _objName = "DBQuery (default)";

        public Action<List<List<List<object>>>> callback;



        public DBQuery (string sqlCommand, 
            Action<List<List<List<object>>>> callback)
            : base(sqlCommand)
        {
            this.callback = callback;
        }

        public DBQuery (string sqlCommand, DBReaderMode dbReaderMode,
            Action<List<List<List<object>>>> callback)
            : base(sqlCommand, dbReaderMode)
        {
            this.callback = callback;
        }



        public override void ReturnResults (List<List<List<object>>> results)
        {
            if (this.callback != null)
            {
                this.callback(results);
            }
        }
    }
}
