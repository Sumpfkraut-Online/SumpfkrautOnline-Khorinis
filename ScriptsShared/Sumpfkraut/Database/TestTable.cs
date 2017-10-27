using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.Database.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Database
{

    public class TestTable : BaseTable
    {

        public static readonly new BaseTable Instance = new TestTable();



        public override string GetTableName ()
        {
            return "testtable";
        }

        public override DBTables.ColumnGetTypeInfo[] GetColumGetTypeInfos ()
        {
            return new DBTables.ColumnGetTypeInfo[] { /* ... */ };
        }

        public override string GetSortByColumn ()
        {
            return "id";
        }

        public override SortOrder GetSortOrder ()
        {
            return SortOrder.ASC;
        }
        
    }

}
