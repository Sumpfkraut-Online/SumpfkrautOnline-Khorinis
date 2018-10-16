using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{
    public partial class MobDefEffectHandler : NamedVobDefEffectHandler
    {
        new public MobDef Host { get { return (MobDef)host; } }

        public MobDefEffectHandler(List<Effect> effects, MobDef host)
            : this("MobDefEffectHandler", effects, host)
        { }

        public MobDefEffectHandler(string objName, List<Effect> effects, MobDef host) 
            : base(objName, effects, host)
        { }

    }
}
