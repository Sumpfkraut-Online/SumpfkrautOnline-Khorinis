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

        new public static readonly string _staticName = "NPCInstEffectHandler (s)";



        static NPCInstEffectHandler ()
        {
            PrintStatic(typeof(NPCInstEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            //NPCInst.sOnHit += OnHit;

            PrintStatic(typeof(NPCInstEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }


        public NPCInstEffectHandler (List<Effect> effects, NPCDef linkedObject)
            : this("NPCInstEffectHandler", effects, linkedObject)
        { }

        public NPCInstEffectHandler (List<Effect> effects, NPCInst linkedObject)
            : this("NPCInstEffectHandler", effects, linkedObject)
        { }

        public NPCInstEffectHandler (string objName, List<Effect> effects, NPCDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }

        public NPCInstEffectHandler (string objName, List<Effect> effects, NPCInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        private static void OnHit (NPCInst attacker, NPCInst target, int damage)
        {
            throw new NotImplementedException();
        }

    }

}
