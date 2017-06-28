using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst
    {
        new public VobEffectHandler EffectHandler { get { return (VobEffectHandler)base.EffectHandler; } }
        protected override BaseEffectHandler CreateHandler()
        {
            return new VobEffectHandler(null, null, this);
        }

        public VobInst(VobDef def) : this()
        {
            this.Definition = def;
        }
    }
}
