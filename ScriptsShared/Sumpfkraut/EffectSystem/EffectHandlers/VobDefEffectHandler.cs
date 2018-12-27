using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public class VobDefEffectHandler : BaseEffectHandler
    {

        new public VobDef Host { get { return (VobDef) host; } }



        static VobDefEffectHandler ()
        {
            PrintStatic(typeof(VobDefEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            PrintStatic(typeof(VobDefEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public VobDefEffectHandler (List<Effect> effects, VobDef host)
            : this("VobDefEffectHandler", effects, host)
        { }

        public VobDefEffectHandler (string objName, List<Effect> effects, VobDef host) 
            : base(objName, effects, host)
        { }

    }

}
