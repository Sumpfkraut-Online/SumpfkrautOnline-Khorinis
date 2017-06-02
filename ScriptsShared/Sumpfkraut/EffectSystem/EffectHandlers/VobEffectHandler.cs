﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class VobEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "VobEffectHandler (s)";



        static VobEffectHandler ()
        {
            PrintStatic(typeof(VobEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            RegisterDestination(Enumeration.ChangeDestination.Vob_CodeName);
            RegisterDestination(Enumeration.ChangeDestination.Vob_VobType);

            PrintStatic(typeof(VobEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public VobEffectHandler (List<Effect> effects, VobDef linkedObject)
            : this("VobEffectHandler (default)", effects, linkedObject)
        { }

        public VobEffectHandler (List<Effect> effects, VobInst linkedObject)
            : this("VobEffectHandler (default)", effects, linkedObject)
        { }

        public VobEffectHandler (string objName, List<Effect> effects, VobDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }

        public VobEffectHandler (string objName, List<Effect> effects, VobInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    throw new NotImplementedException();
        //}

    }

}