using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class ItemInstEffectHandler : NamedVobInstEffectHandler
    {

        new public ItemInst Host { get { return (ItemInst) host; } }



        static ItemInstEffectHandler ()
        {
            PrintStatic(typeof(ItemInstEffectHandler), "Start subscribing listeners to events...");

            PrintStatic(typeof(ItemInstEffectHandler), "Finished subscribing listeners to events...");
        }



        public ItemInstEffectHandler (List<Effect> effects, ItemInst host)
            : this("ItemInstEffectHandler", effects, host)
        { }

        public ItemInstEffectHandler (string objName, List<Effect> effects, ItemInst host) 
            : base(objName, effects, host)
        { }
        
        
    }

}
