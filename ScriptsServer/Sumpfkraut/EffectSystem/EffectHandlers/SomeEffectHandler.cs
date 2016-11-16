using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class SomeEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "SomeEffectHandler (static)";
        new protected static bool isInitialized = false;



        public SomeEffectHandler (List<Effect> effects)
            : this("SomeEffectHandler (default)", effects)
        { }

        public SomeEffectHandler (string objName, List<Effect> effects) 
            : base(objName, effects)
        { }



        override public void Init ()
        {
            PrintStatic(typeof(SomeEffectHandler), "Start subscribing listeners to events...");

            // to do

            PrintStatic(typeof(SomeEffectHandler), "Finished subscribing listeners to events...");
        }

    }

}
