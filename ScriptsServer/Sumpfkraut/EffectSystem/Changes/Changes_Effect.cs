using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class Changes_Effect : BaseChange
    {

        new public static readonly string _staticName = "Change_Effect_Name (static)";

        new public static readonly ChangeType[] supportedChangeTypes = new ChangeType[]
        {
            ChangeType.Effect_Name_Set
        };

        new public static readonly Type[] parameterTypes = new Type[] 
        {
            typeof(string)
        };

        new public static List<ChangeDestination> influencedDestinations = new List<ChangeDestination>();



        protected Changes_Effect (Effect effect, ChangeType changeType, object[] parameters) 
            : base(effect, changeType, parameters)
        {
            SetObjName("Changes_Effect (default)");
        }



        // use this method to create the objects
        new public static Changes_Effect Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            if (!CheckCreateBasics(effect, changeType, parameters, parameterTypes)) { return null; }
            return new Changes_Effect(effect, changeType, parameters);
        }

    }

}
