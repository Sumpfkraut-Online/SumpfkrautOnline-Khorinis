using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class AnotherEffectHandler : SomeEffectHandler
    {

        new public static readonly string _staticName = "AnotherEffectHandler (static)";



        static AnotherEffectHandler ()
        {
            PrintStatic(typeof(AnotherEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(AnotherEffectHandler), "Finished subscribing listeners to events...");
        }

        public AnotherEffectHandler (List<Effect> effects)
            : this("SomeEffectHandler (default)", effects)
        { }

        public AnotherEffectHandler (string objName, List<Effect> effects) 
            : base(objName, effects)
        { }

    }

}
