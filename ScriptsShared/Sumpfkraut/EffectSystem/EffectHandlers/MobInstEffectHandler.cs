using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{
    public partial class MobInstEffectHandler : NamedVobInstEffectHandler
    {
        new public MobInst Host { get { return (MobInst)this.host; } }

        public MobInstEffectHandler(List<Effect> effects, NamedVobInst host)
            : this("MobInstEffectHandler", effects, host)
        { }

        public MobInstEffectHandler(string objName, List<Effect> effects, NamedVobInst host) 
            : base(objName, effects, host)
        { }

    }
}
