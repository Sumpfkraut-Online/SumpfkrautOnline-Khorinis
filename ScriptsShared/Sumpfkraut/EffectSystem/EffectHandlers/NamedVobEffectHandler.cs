using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class NamedVobEffectHandler : VobEffectHandler
    {

        new public static readonly string _staticName = "NamedVobEffectHandler (s)";



        static NamedVobEffectHandler ()
        {
            PrintStatic(typeof(NamedVobEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            RegisterDestination(Enumeration.ChangeDestination.NamedVob_Name);

            PrintStatic(typeof(NamedVobEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NamedVobEffectHandler (List<Effect> effects, NamedVobDef linkedObject)
            : this("NamedVobEffectHandler", effects, linkedObject)
        { }

        public NamedVobEffectHandler (List<Effect> effects, NamedVobInst linkedObject)
            : this("NamedVobEffectHandler", effects, linkedObject)
        { }

        public NamedVobEffectHandler (string objName, List<Effect> effects, NamedVobDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }

        public NamedVobEffectHandler (string objName, List<Effect> effects, NamedVobInst linkedObject) 
            : base(objName, effects, linkedObject)
        { }

    }

}
