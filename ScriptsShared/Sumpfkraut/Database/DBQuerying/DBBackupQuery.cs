using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Database.DBQuerying
{
    public partial class DBBackupQuery : GUC.Utilities.ExtendedObject, IDBQuery
    {

        new public static readonly String _staticName = "DBBackupQuery (s)";



        public DBBackupQuery ()
        {
            SetObjName("DBBackupQuery");
        }



        public void HandleQuery ()
        {
            // do backup
        }

    }
}
