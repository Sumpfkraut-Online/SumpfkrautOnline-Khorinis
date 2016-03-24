using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst
    {
        public VobInst(VobDef vobDef) : this(vobDef, new WorldObjects.Vob())
        { }
        
        protected VobInst(VobDef vobDef, WorldObjects.Vob baseInst) : base(vobDef, baseInst)
        { }
    }
}
