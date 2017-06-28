using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class BaseVobInst
    {
        protected EffectSystem.EffectHandlers.BaseEffectHandler effectHandler;
        public EffectSystem.EffectHandlers.BaseEffectHandler EffectHandler { get { return this.effectHandler; } }
        protected abstract EffectSystem.EffectHandlers.BaseEffectHandler CreateHandler();

        partial void pConstruct()
        {
            this.effectHandler = CreateHandler();
        }

        protected BaseVobInst(BaseVobDef def) : this()
        {
            this.Definition = def;
        }
    }
}
