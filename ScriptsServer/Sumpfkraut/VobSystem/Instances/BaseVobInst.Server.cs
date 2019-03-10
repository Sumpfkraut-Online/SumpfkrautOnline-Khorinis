using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class BaseVobInst
    {
        protected BaseVobInst(BaseVobDef def) : this()
        {
            this.Definition = def;
        }

        public void SetPosAng(Vec3f position, Angles angles) { this.BaseInst.SetPosAng(position, angles); }
    }
}
