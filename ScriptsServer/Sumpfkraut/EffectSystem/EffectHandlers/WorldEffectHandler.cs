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


        new public WorldInst Host { get { return (WorldInst)base.Host; } }

        public WorldEffectHandler (List<Effect> effects, WorldInst host)
            : this("WorldEffectHandler (default)", effects, host)
        { }
        

        public WorldEffectHandler (string objName, List<Effect> effects, WorldInst host) 
            : base(objName, effects, host)
        { }



        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    Print("Apply what? Naaaa!");
        //}

    }

}
