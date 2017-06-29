using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class NamedVobInstEffectHandler : VobInstEffectHandler
    {

        new public static readonly string _staticName = "NamedVobInstEffectHandler (s)";



        new protected NamedVobInst linkedObject;
        new public NamedVobInst GetLinkedObject () { return linkedObject; }



        static NamedVobInstEffectHandler ()
        {
            PrintStatic(typeof(NamedVobInstEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            RegisterDestination(Enumeration.ChangeDestination.NamedVob_Name);

            PrintStatic(typeof(NamedVobInstEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NamedVobInstEffectHandler (List<Effect> effects, NamedVobInst linkedObject)
            : this("NamedVobInstEffectHandler", effects, linkedObject)
        { }

        public NamedVobInstEffectHandler (string objName, List<Effect> effects, NamedVobInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }

    }

}
