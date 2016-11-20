using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class WorldDefEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "WorldDefEffectHandler (static)";



        static WorldDefEffectHandler ()
        {
            PrintStatic(typeof(SomeEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(SomeEffectHandler), "Finished subscribing listeners to events...");
        }

        public WorldDefEffectHandler (List<Effect> effects)
            : this("WorldDefEffectHandler (default)", effects)
        { }

        public WorldDefEffectHandler (string objName, List<Effect> effects) 
            : base(objName, effects)
        { }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
        }

    }

}
