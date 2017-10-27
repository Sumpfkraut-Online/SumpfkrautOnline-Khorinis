using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GUC.Scripts.Sumpfkraut.Database.DBTables;

namespace GUC.Scripts.Sumpfkraut.Database
{

    interface ITable
    {

        string GetTableName ();
        ColumnGetTypeInfo[] GetColumGetTypeInfos ();
        string GetSortByColumn ();
        Enumeration.SortOrder GetSortOrder ();

    }

}
