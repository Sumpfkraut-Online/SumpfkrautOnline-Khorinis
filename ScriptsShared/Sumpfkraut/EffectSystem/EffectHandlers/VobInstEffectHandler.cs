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

        new protected VobInst host;
        new public VobInst GetHost () { return host; }



        static VobInstEffectHandler ()
        {
            PrintStatic(typeof(VobInstEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            RegisterDestination(Enumeration.ChangeDestination.Vob_CodeName);
            RegisterDestination(Enumeration.ChangeDestination.Vob_VobType);

            PrintStatic(typeof(VobInstEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public VobInstEffectHandler (List<Effect> effects, VobInst host)
            : this("VobInstEffectHandler", effects, host)
        { }

        public VobInstEffectHandler (string objName, List<Effect> effects, VobInst host) 
            : base(objName, effects, host)
        { }

    }

}
