using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class ItemDefEffectHandler : NamedVobDefEffectHandler
    {

        new public ItemDef Host { get { return (ItemDef) host; } }



        static ItemDefEffectHandler ()
        {
            PrintStatic(typeof(ItemDefEffectHandler), "Start subscribing listeners to events...");

            PrintStatic(typeof(ItemDefEffectHandler), "Finished subscribing listeners to events...");
        }



        public ItemDefEffectHandler (List<Effect> effects, ItemDef host)
            : this("ItemDefEffectHandler", effects, host)
        { }

        public ItemDefEffectHandler (string objName, List<Effect> effects, ItemDef host) 
            : base(objName, effects, host)
        { }
        
    }

}
