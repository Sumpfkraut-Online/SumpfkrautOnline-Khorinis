using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class NamedVobDefEffectHandler : VobDefEffectHandler
    {

        new public static readonly string _staticName = "NamedVobDefEffectHandler (s)";



        new protected NamedVobDef linkedObject;
        new public NamedVobDef GetLinkedObject () { return linkedObject; }



        static NamedVobDefEffectHandler ()
        {
            PrintStatic(typeof(NamedVobDefEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            PrintStatic(typeof(NamedVobDefEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public NamedVobDefEffectHandler (List<Effect> effects, NamedVobDef linkedObject)
            : this("NamedVobDefEffectHandler", effects, linkedObject)
        { }

        public NamedVobDefEffectHandler (string objName, List<Effect> effects, NamedVobDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }


    }

}
