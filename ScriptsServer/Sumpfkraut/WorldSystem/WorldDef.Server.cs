using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{

    public partial class WorldDef
    {

        public static readonly string DBPatternFilePath = "DBPattern_SqLite3_World.sql";


        private List<List<List<object>>> sqlResult = null;
        private bool sqlResultInUse = false;
        public void DropSQLResult () { if (!sqlResultInUse) { sqlResult = null; } }



        public WorldDef ()
            : this("WorldDef (default)")
        { }

        public WorldDef (string objName)
        {
            SetObjName(objName);
        }



        

    }

}
