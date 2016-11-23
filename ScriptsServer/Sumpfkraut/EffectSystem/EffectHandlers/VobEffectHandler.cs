using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class VobEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "VobEffectHandler (static)";



        static VobEffectHandler ()
        {
            PrintStatic(typeof(VobEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(VobEffectHandler), "Finished subscribing listeners to events...");
        }



        public VobEffectHandler (List<Effect> effects, object linkedObject)
            : this("VobEffectHandler (default)", effects, linkedObject)
        { }

        public VobEffectHandler (string objName, List<Effect> effects, object linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
        }

    }

}
