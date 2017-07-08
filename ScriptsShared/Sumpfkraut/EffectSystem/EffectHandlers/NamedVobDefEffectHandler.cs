using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class NamedVobDefEffectHandler : VobDefEffectHandler
    {

        new protected NamedVobDef host;
        new public NamedVobDef GetHost () { return host; }



        static NamedVobDefEffectHandler ()
        {
            PrintStatic(typeof(NamedVobDefEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            PrintStatic(typeof(NamedVobDefEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NamedVobDefEffectHandler (List<Effect> effects, NamedVobDef host)
            : this("NamedVobDefEffectHandler", effects, host)
        { }

        public NamedVobDefEffectHandler (string objName, List<Effect> effects, NamedVobDef host) 
            : base(objName, effects, host)
        { }


    }

}
