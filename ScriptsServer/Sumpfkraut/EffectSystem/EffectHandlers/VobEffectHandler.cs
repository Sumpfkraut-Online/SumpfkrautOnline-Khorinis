using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class VobEffectHandler : BaseEffectHandler
    {

        new public static readonly string _staticName = "VobEffectHandler (static)";
        
        static VobEffectHandler ()
        {
            PrintStatic(typeof(BaseEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            RegisterDestination(Enumeration.ChangeDestination.Vob_CodeName);
            RegisterDestination(Enumeration.ChangeDestination.Vob_Name);
            RegisterDestination(Enumeration.ChangeDestination.Vob_VobInstType);

            PrintStatic(typeof(BaseEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }
        
        new public VobInst Host { get { return (VobInst)base.Host; } }

        public VobEffectHandler (List<Effect> effects, VobInst host)
            : this("VobEffectHandler (default)", effects, host)
        { }
        
        public VobEffectHandler (string objName, List<Effect> effects, VobInst host) 
            : base(objName, effects, host)
        { }

        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    throw new NotImplementedException();
        //}

    }

}
