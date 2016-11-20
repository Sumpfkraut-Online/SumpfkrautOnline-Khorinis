using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class VobEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "VobEffectHandler (static)";



        static VobEffectHandler ()
        {
            PrintStatic(typeof(SomeEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(SomeEffectHandler), "Finished subscribing listeners to events...");
        }

        public VobEffectHandler (List<Effect> effects)
            : this("VobEffectHandler (default)", effects)
        { }

        public VobEffectHandler (string objName, List<Effect> effects) 
            : base(objName, effects)
        { }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
        }

    }

}
