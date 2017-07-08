﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class WorldInstEffectHandler : BaseEffectHandler
    {

        new protected WorldInst linkedObject;
        new public WorldInst GetLinkedObject () { return linkedObject; }



        static WorldInstEffectHandler ()
        {
            PrintStatic(typeof(WorldInstEffectHandler), "Start subscribing listeners to events...");
            
            RegisterDestination(Enumeration.ChangeDestination.World_Clock_IsRunning);
            RegisterDestination(Enumeration.ChangeDestination.World_Clock_Rate);
            RegisterDestination(Enumeration.ChangeDestination.World_Clock_Time);

            PrintStatic(typeof(WorldInstEffectHandler), "Finished subscribing listeners to events...");
        }



        public WorldInstEffectHandler (List<Effect> effects, WorldInst linkedObject)
            : this("WorldEffectHandler (default)", effects, linkedObject)
        { }

        public WorldInstEffectHandler (string objName, List<Effect> effects, WorldInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }

    }

}
