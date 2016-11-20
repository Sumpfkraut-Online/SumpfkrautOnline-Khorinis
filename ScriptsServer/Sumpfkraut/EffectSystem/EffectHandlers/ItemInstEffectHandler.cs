using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class ItemInstEffectHandler : ItemDefEffectHandler
    {

        new public static readonly string _staticName = "ItemInstEffectHandler (static)";



        static ItemInstEffectHandler ()
        {
            PrintStatic(typeof(SomeEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(SomeEffectHandler), "Finished subscribing listeners to events...");
        }

        public ItemInstEffectHandler (List<Effect> effects)
            : this("ItemInstEffectHandler (default)", effects)
        { }

        public ItemInstEffectHandler (string objName, List<Effect> effects) 
            : base(objName, effects)
        { }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
        }

    }

}
