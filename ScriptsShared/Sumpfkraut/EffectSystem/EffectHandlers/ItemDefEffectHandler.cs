using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class ItemDefEffectHandler : NamedVobDefEffectHandler
    {

        static ItemDefEffectHandler ()
        {
            PrintStatic(typeof(ItemDefEffectHandler), "Start subscribing listeners to events...");

            PrintStatic(typeof(ItemDefEffectHandler), "Finished subscribing listeners to events...");
        }



        new protected ItemDef linkedObject;
        new public ItemDef GetLinkedObject () { return linkedObject; }



        public ItemDefEffectHandler (List<Effect> effects, ItemDef linkedObject)
            : this("ItemDefEffectHandler", effects, linkedObject)
        { }

        public ItemDefEffectHandler (string objName, List<Effect> effects, ItemDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }
        
    }

}
