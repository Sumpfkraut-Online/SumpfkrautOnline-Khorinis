using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class BaseVobInst
    {
        protected BaseVobInst(BaseVobDef def, WorldObjects.BaseVob baseInst) : this(baseInst)
        {
            if (def == null)
                throw new ArgumentNullException("VobDef is null!");

            this.definition = def;
            this.baseInst.Instance = this.definition.BaseDef;
        }
    }
}
