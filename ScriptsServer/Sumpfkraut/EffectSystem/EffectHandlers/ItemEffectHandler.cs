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

            ItemInst.OnSetAmount += OnSetAmount;

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



        public static void OnSetAmount (ItemInst itemInst, int amount)
        {
            throw new NotImplementedException();
        }



        protected override void ApplyEffectInner (Effect effect, bool reverse = false)
        {
            Print("Apply what? Naaaa!");
            
            Type lot = LinkedObjectType;

            if      (LinkedObjectType == typeof(ItemDef))
            {
                // ...
            }
            else if (LinkedObjectType == typeof(ItemInst))
            {
                ItemInst lo = GetLinkedObject<ItemInst>();
                // ...
            }
            else
            {
                Print("Screwed :D");
            }
        }
        

    }

}
