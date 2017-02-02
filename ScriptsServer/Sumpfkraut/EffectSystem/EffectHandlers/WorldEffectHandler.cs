using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class WorldEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "WorldEffectHandler (static)";



        static WorldEffectHandler ()
        {
            PrintStatic(typeof(WorldEffectHandler), "Start subscribing listeners to events...");
            
            // to do

            PrintStatic(typeof(WorldEffectHandler), "Finished subscribing listeners to events...");
        }



        public WorldEffectHandler (List<Effect> effects, WorldDef linkedObject)
            : this("WorldEffectHandler (default)", effects, linkedObject)
        { }

        public WorldEffectHandler (List<Effect> effects, WorldInst linkedObject)
            : this("WorldEffectHandler (default)", effects, linkedObject)
        { }

        public WorldEffectHandler (string objName, List<Effect> effects, WorldDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }

        public WorldEffectHandler (string objName, List<Effect> effects, WorldInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    Print("Apply what? Naaaa!");
        //}

    }

}
