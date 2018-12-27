using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class WorldInstEffectHandler : BaseEffectHandler
    {

        new public WorldInst Host { get { return (WorldInst) host; } }



        static WorldInstEffectHandler ()
        {
            PrintStatic(typeof(WorldInstEffectHandler), "Start subscribing listeners to events...");
            
            RegisterDestination(Enumeration.ChangeDestination.World_Clock_IsRunning);
            RegisterDestination(Enumeration.ChangeDestination.World_Clock_Rate);
            RegisterDestination(Enumeration.ChangeDestination.World_Clock_Time);

            PrintStatic(typeof(WorldInstEffectHandler), "Finished subscribing listeners to events...");
        }



        public WorldInstEffectHandler (List<Effect> effects, WorldInst host)
            : this("WorldEffectHandler (default)", effects, host)
        { }

        public WorldInstEffectHandler (string objName, List<Effect> effects, WorldInst host) 
            : base(objName, effects, host)
        { }

    }

}
