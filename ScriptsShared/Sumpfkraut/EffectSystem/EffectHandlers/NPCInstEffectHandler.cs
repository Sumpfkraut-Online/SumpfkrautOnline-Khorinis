using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class NPCInstEffectHandler : VobInstEffectHandler
    {

        new public NPCInst Host { get { return (NPCInst) host; } }



        static NPCInstEffectHandler ()
        {
            PrintStatic(typeof(NPCInstEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            //NPCInst.sOnHit += OnHit;

            PrintStatic(typeof(NPCInstEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NPCInstEffectHandler (List<Effect> effects, NPCInst host)
            : this("NPCInstEffectHandler", effects, host)
        { }

        public NPCInstEffectHandler (string objName, List<Effect> effects, NPCInst host) 
            : base(objName, effects, host)
        { }
    }

}
