using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class ItemEffectHandler : VobEffectHandler
    {

        new public static readonly string _staticName = "ItemEffectHandler (static)";



        static ItemEffectHandler ()
        {
            PrintStatic(typeof(ItemEffectHandler), "Start subscribing listeners to events...");
            // to do
            PrintStatic(typeof(ItemEffectHandler), "Finished subscribing listeners to events...");
        }



        public ItemEffectHandler (List<Effect> effects, ItemDef linkedObject)
            : this("ItemEffectHandler (default)", effects, linkedObject)
        { }

        public ItemEffectHandler (List<Effect> effects, ItemInst linkedObject)
            : this("ItemEffectHandler (default)", effects, linkedObject)
        { }

        public ItemEffectHandler (string objName, List<Effect> effects, ItemDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }

        public ItemEffectHandler (string objName, List<Effect> effects, ItemInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
        }

    }

}
