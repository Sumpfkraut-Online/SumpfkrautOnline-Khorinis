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

        new protected NPCInst linkedObject;
        new public NPCInst GetLinkedObject () { return linkedObject; }



        static NPCInstEffectHandler ()
        {
            PrintStatic(typeof(NPCInstEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            //NPCInst.sOnHit += OnHit;

            PrintStatic(typeof(NPCInstEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NPCInstEffectHandler (List<Effect> effects, NPCInst linkedObject)
            : this("NPCInstEffectHandler", effects, linkedObject)
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
