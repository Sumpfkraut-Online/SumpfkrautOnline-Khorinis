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

        new public static readonly string _staticName = "WorldEffectHandler (s)";



        new protected WorldDef linkedObject;
        new public WorldDef GetLinkedObject () { return linkedObject; }



        static WorldDefEffectHandler ()
        {
            PrintStatic(typeof(WorldDefEffectHandler), "Start subscribing listeners to events...");

            PrintStatic(typeof(WorldDefEffectHandler), "Finished subscribing listeners to events...");
        }



        public WorldDefEffectHandler (List<Effect> effects, WorldDef linkedObject)
            : this("WorldEffectHandler (default)", effects, linkedObject)
        { }

        public WorldDefEffectHandler (string objName, List<Effect> effects, WorldDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }

    }

}
