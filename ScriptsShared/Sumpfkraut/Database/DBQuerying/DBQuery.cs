using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Database.DBQuerying
{
    public partial class DBQuery : AbstractDBQuery
    {

        new public static readonly string _staticName = "DBQuery (static)";

        public Action<List<List<List<object>>>> callback;



        public DBQuery (string dataSource, string sqlCommand, 
            Action<List<List<List<object>>>> callback)
            : base(dataSource, sqlCommand)
        {
            SetObjName("DBQuery (default)");
            this.callback = callback;
        }

        public DBQuery (string dataSource, string sqlCommand, DBReaderMode dbReaderMode,
            Action<List<List<List<object>>>> callback)
            : base(dataSource, sqlCommand, dbReaderMode)
        {
            SetObjName("DBQuery (default)");
            this.callback = callback;
        }



        public override void ReturnResults (List<List<List<object>>> results)
        {
            if (callback != null)
            {
                callback(results);
            }
        }
    }



    public class DBQuery<T1> : AbstractDBQuery
    {

        new public static readonly string _staticName = "DBQuery<T1> (static)";

        public Action<List<List<List<object>>>, T1> callback;
        T1 arg1;


        public DBQuery (string dataSource, string sqlCommand, 
            Action<List<List<List<object>>>, T1> callback, 
            T1 arg1)
            : base(dataSource, sqlCommand)
        {
            this._objName = "DBQuery<T1> (default)";
            this.callback = callback;
            this.arg1 = arg1;
        }

        public DBQuery (string dataSource, string sqlCommand, DBReaderMode dbReaderMode,
            Action<List<List<List<object>>>, T1> callback, 
            T1 arg1)
            : base(dataSource, sqlCommand, dbReaderMode)
        {
            this._objName = "DBQuery<T1> (default)";
            this.callback = callback;
            this.arg1 = arg1;
        }



        public override void ReturnResults (List<List<List<object>>> results)
        {
            if (callback != null)
            {
                callback(results, arg1);
            }
        }
    }



    public class DBQuery<T1, T2> : AbstractDBQuery
    {

        new public static readonly string _staticName = "DBQuery<T1, T2> (static)";

        public Action<List<List<List<object>>>, T1, T2> callback;
        T1 arg1;
        T2 arg2;


        public DBQuery (string dataSource, string sqlCommand, 
            Action<List<List<List<object>>>, T1, T2> callback, 
            T1 arg1, T2 arg2)
            : base(dataSource, sqlCommand)
        {
            this._objName = "DBQuery<T1, T2> (default)";
            this.callback = callback;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        public DBQuery (string dataSource, string sqlCommand, DBReaderMode dbReaderMode,
            Action<List<List<List<object>>>, T1, T2> callback, 
            T1 arg1, T2 arg2)
            : base(dataSource, sqlCommand, dbReaderMode)
        {
            this._objName = "DBQuery<T1, T2> (default)";
            this.callback = callback;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }



        public override void ReturnResults (List<List<List<object>>> results)
        {
            if (callback != null)
            {
                callback(results, arg1, arg2);
            }
        }
    }

}
