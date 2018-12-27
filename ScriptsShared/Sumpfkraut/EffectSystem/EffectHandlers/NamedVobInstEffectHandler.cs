using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class NamedVobInstEffectHandler : VobInstEffectHandler
    {

        new public NamedVobInst Host { get { return (NamedVobInst) host; } }



        static NamedVobInstEffectHandler ()
        {
            PrintStatic(typeof(NamedVobInstEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            RegisterDestination(Enumeration.ChangeDestination.NamedVob_Name);

            PrintStatic(typeof(NamedVobInstEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NamedVobInstEffectHandler (List<Effect> effects, NamedVobInst host)
            : this("NamedVobInstEffectHandler", effects, host)
        { }

        public NamedVobInstEffectHandler (string objName, List<Effect> effects, NamedVobInst host) 
            : base(objName, effects, host)
        { }

    }

}
