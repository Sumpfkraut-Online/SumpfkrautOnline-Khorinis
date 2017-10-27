using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.Database.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Database
{

    public abstract class BaseTable : ITable
    {

        public static readonly BaseTable Instance;
        


        protected BaseTable ()
        { }



        public abstract string GetTableName ();

        public abstract DBTables.ColumnGetTypeInfo[] GetColumGetTypeInfos ();

        public abstract string GetSortByColumn ();

        public abstract SortOrder GetSortOrder ();
        
    }

}
