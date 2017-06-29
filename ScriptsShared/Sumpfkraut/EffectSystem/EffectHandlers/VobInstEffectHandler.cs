using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class VobInstEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "VobInstEffectHandler (s)";



        static VobInstEffectHandler ()
        {
            PrintStatic(typeof(VobInstEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            RegisterDestination(Enumeration.ChangeDestination.Vob_CodeName);
            RegisterDestination(Enumeration.ChangeDestination.Vob_VobType);

            PrintStatic(typeof(VobInstEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public VobInstEffectHandler (List<Effect> effects, VobInst linkedObject)
            : this("VobInstEffectHandler", effects, linkedObject)
        { }

        public VobInstEffectHandler (string objName, List<Effect> effects, VobInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    throw new NotImplementedException();
        //}

    }

}
