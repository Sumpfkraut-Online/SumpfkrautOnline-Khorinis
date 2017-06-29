using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class NPCDefEffectHandler : NamedVobDefEffectHandler
    {

        new public static readonly string _staticName = "NPCDefEffectHandler (s)";



        static NPCDefEffectHandler ()
        {
            PrintStatic(typeof(NPCDefEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            PrintStatic(typeof(NPCDefEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NPCDefEffectHandler (List<Effect> effects, NPCDef linkedObject)
            : this("NPCDefEffectHandler", effects, linkedObject)
        { }

        public NPCDefEffectHandler (string objName, List<Effect> effects, NPCDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }

    }

}
