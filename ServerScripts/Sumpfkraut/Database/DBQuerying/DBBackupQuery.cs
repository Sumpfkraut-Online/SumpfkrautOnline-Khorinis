using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Database.DBQuerying
{
    public class DBBackupQuery : ScriptObject, IDBQuery
    {

        new public static readonly String _staticName = "DBBackupQuery (static)";



        public DBBackupQuery ()
        {
            SetObjName("DBBackupQuery (default)");
        }



        public void HandleQuery ()
        {
            // do backup
        }

    }
}
