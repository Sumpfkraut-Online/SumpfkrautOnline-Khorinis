using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class WorldDefEffectHandler : BaseEffectHandler
    {

        new public WorldDef Host { get { return (WorldDef) host; } }



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
