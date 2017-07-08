using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class WorldDefEffectHandler : BaseEffectHandler
    {

        new protected WorldDef host;
        new public WorldDef GetHost () { return host; }



        static WorldDefEffectHandler ()
        {
            PrintStatic(typeof(WorldDefEffectHandler), "Start subscribing listeners to events...");

            PrintStatic(typeof(WorldDefEffectHandler), "Finished subscribing listeners to events...");
        }



        public WorldDefEffectHandler (List<Effect> effects, WorldDef host)
            : this("WorldEffectHandler (default)", effects, host)
        { }

        public WorldDefEffectHandler (string objName, List<Effect> effects, WorldDef host) 
            : base(objName, effects, host)
        { }

    }

}
