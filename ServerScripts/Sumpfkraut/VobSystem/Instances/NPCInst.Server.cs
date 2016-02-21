using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        public NPCInst(NPCDef def, int id = -1) : base(def, true)
        {
            SetBaseInst(new WorldObjects.NPC(this, def.baseDef, id));
        }
    }
}
