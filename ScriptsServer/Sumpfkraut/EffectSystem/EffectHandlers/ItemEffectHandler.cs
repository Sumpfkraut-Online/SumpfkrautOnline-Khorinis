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



        public ItemEffectHandler (List<Effect> effects, ItemDef host)
            : this("ItemEffectHandler (default)", effects, host)
        { }

        public ItemEffectHandler (List<Effect> effects, ItemInst host)
            : this("ItemEffectHandler (default)", effects, host)
        { }

        public ItemEffectHandler (string objName, List<Effect> effects, ItemDef host) 
            : base(objName, effects, host)
        { }

        public ItemEffectHandler (string objName, List<Effect> effects, ItemInst host) 
            : base(objName, effects, host)
        { }



        public static void OnSetAmount (ItemInst itemInst, int amount)
        {
            throw new NotImplementedException();
        }



        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    Print("Apply what? Naaaa!");
            
        //    Type lot = HostType;

        //    if      (HostType == typeof(ItemDef))
        //    {
        //        // ...
        //    }
        //    else if (HostType == typeof(ItemInst))
        //    {
        //        ItemInst lo = GetHost<ItemInst>();
        //        // ...
        //    }
        //    else
        //    {
        //        Print("Screwed :D");
        //    }
        //}
        

    }

}
