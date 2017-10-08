using GUC.Scripts.Sumpfkraut.Database;
using GUC.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GUC.Scripts.Sumpfkraut.Database.DBAgent;
using static GUC.Scripts.Sumpfkraut.Database.DBTables;

namespace GUC.Scripts.Sumpfkraut.Visuals
{

    public partial class VisualLoader : BaseLoader
    {
        public VisualLoader (string objName, string dbFilePath, 
            Dictionary<string, List<ColumnGetTypeInfo>> dbStructure, List<string> dbTableLoadOrder) 
            : base(objName, dbFilePath, dbStructure, dbTableLoadOrder)
        { }

        partial void pLoad (bool useAsyncMode);
        public override void Load (bool useAsyncMode)
        {
            throw new NotImplementedException();
        }

        partial void pSave (bool useAsyncMode);
        public override void Save (bool useAsyncMode)
        {
            throw new NotImplementedException();
        }
    }

}
