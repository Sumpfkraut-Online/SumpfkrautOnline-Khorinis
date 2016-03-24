using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class BaseVobInst
    {
        protected BaseVobInst (BaseVobDef vobDef, WorldObjects.BaseVob baseInst) : this(baseInst)
        {
            if (vobDef == null)
            {
                throw new ArgumentNullException("Invalid null-value probided for vobDef in constructor!");
            }

            this.vobDef = vobDef;
            this.baseInst.Instance = this.vobDef.BaseDef;
        }
    }
}
