using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst
    {
        public VobInst(VobDef def, int id = -1) : base(def)
        {
            SetBaseInst(new WorldObjects.Vob(this, def.baseDef, id));
        }

        protected VobInst(VobDef def, bool b) : base(def)
        {
        }
    }
}
