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



        new protected ItemDef host;
        new public ItemDef GetHost () { return host; }



        public ItemDefEffectHandler (List<Effect> effects, ItemDef host)
            : this("ItemDefEffectHandler", effects, host)
        { }

        public ItemDefEffectHandler (string objName, List<Effect> effects, ItemDef host) 
            : base(objName, effects, host)
        { }
        
    }

}
