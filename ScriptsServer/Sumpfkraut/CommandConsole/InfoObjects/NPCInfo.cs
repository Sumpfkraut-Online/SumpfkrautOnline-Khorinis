using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.CommandConsole.InfoObjects
{
    public class NPCInfo : VobInfo
    {

        public NPCInfo ()
        {
            this._objName = "NPCInfo (default)";
        }



        public int HP;
        public int HPMax;
        public String NPCName;
        public String MapName;

    }
}
