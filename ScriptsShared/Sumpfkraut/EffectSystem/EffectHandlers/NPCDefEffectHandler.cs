using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class NPCDefEffectHandler : NamedVobDefEffectHandler
    {

        new public NPCDef Host { get { return (NPCDef) host; } }



        static NPCDefEffectHandler ()
        {
            PrintStatic(typeof(NPCDefEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            PrintStatic(typeof(NPCDefEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NPCDefEffectHandler (List<Effect> effects, NPCDef host)
            : this("NPCDefEffectHandler", effects, host)
        { }

        public NPCDefEffectHandler (string objName, List<Effect> effects, NPCDef host) 
            : base(objName, effects, host)
        { }

    }

}
