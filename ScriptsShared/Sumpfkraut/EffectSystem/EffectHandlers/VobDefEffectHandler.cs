using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class VobDefEffectHandler : BaseEffectHandler
    {

        new protected VobDef linkedObject;
        new public VobDef GetLinkedObject () { return linkedObject; }



        static VobDefEffectHandler ()
        {
            PrintStatic(typeof(VobDefEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");

            PrintStatic(typeof(VobDefEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }



        public VobDefEffectHandler (List<Effect> effects, VobDef linkedObject)
            : this("VobDefEffectHandler", effects, linkedObject)
        { }

        public VobDefEffectHandler (string objName, List<Effect> effects, VobDef linkedObject) 
            : base(objName, effects, linkedObject)
        { }



        //protected override void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    throw new NotImplementedException();
        //}

    }

}
