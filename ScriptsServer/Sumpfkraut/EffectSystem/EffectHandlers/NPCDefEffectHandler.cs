using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class NPCDefEffectHandler : VobDefEffectHandler
    {

        new public static readonly string _staticName = "NPCDefEffectHandler (static)";



        static NPCDefEffectHandler ()
        {
            PrintStatic(typeof(SomeEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(SomeEffectHandler), "Finished subscribing listeners to events...");
        }

        public NPCDefEffectHandler (List<Effect> effects)
            : this("NPCDefEffectHandler (default)", effects)
        { }

        public NPCDefEffectHandler (string objName, List<Effect> effects) 
            : base(objName, effects)
        { }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
        }

    }

}
