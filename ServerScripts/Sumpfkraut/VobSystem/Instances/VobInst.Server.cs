using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst
    {
        public VobInst(VobDef def) : this(def, new WorldObjects.Vob())
        {
        }
        
        protected VobInst(VobDef def, WorldObjects.Vob baseInst) : base(def, baseInst)
        {
        }
    }
}
