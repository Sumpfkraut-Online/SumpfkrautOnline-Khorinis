using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class ItemInstEffectHandler : VobInstEffectHandler
    {

        new public static readonly string _staticName = "ItemInstEffectHandler (s)";



        static ItemInstEffectHandler ()
        {
            PrintStatic(typeof(ItemInstEffectHandler), "Start subscribing listeners to events...");

            ItemInst.OnSetAmount += OnSetAmount;

            PrintStatic(typeof(ItemInstEffectHandler), "Finished subscribing listeners to events...");
        }



        new protected ItemInst linkedObject;
        new public ItemInst GetLinkedObject () { return linkedObject; }



        public ItemInstEffectHandler (List<Effect> effects, ItemInst linkedObject)
            : this("ItemInstEffectHandler", effects, linkedObject)
        { }

        public ItemInstEffectHandler (string objName, List<Effect> effects, ItemInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        public static void OnSetAmount (ItemInst itemInst, int amount)
        {
            throw new NotImplementedException();
        }
        
    }

}
