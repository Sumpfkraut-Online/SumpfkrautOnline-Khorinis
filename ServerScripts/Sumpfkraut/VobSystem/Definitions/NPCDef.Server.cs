using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class NPCDef
    {
        public NPCDef(string codeName, int id = -1) : base(codeName)
        {
            SetBaseDef(new NPCInstance(this, id));
        }
    }
}
