using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class VobDefEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "VobDefEffectHandler (static)";



        static VobDefEffectHandler ()
        {
            PrintStatic(typeof(SomeEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(SomeEffectHandler), "Finished subscribing listeners to events...");
        }

        public VobDefEffectHandler (List<Effect> effects)
            : this("VobDefEffectHandler (default)", effects)
        { }

        public VobDefEffectHandler (string objName, List<Effect> effects) 
            : base(objName, effects)
        { }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
        }

    }

}
