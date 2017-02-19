using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class Change_Effect_Name : BaseChangeInit
    {

        new public static readonly string _staticName = "Change_Effect_Name (static)";

        new public static readonly ChangeType[] includedChangeTypes = new ChangeType[]
        {
            ChangeType.Effect_Name_Set
        };

        new public static readonly Type[] parameterTypes = new Type[] 
        {
            typeof(string)
        };

        new public static List<ChangeDestination> influencedDestinations = new List<ChangeDestination>();



        protected Change_Effect_Name (Effect effect, ChangeType changeType, object[] parameters) 
            : base(effect, changeType, parameters)
        {
            SetObjName("Change_Effect_Name (default)");
        }



        // use this method to create the objects
        new public static Change_Effect_Name Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            if (!CheckCreateBasics(effect, changeType, parameters, parameterTypes)) { return null; }
            return new Change_Effect_Name(effect, changeType, parameters);
        }

    }

}
