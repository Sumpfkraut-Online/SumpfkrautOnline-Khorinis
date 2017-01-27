using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class Change_World_Time : BaseChange
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



        protected Change_World_Time (Effect effect, ChangeType changeType, object[] parameters) 
            : base(effect, changeType, parameters)
        {
            SetObjName("Change_World_Time (default)");
        }



        // use this method to create the objects
        new public static Change_Effect_Name Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            if (!CreateCheckBasics(effect, changeType, parameters, parameterTypes)) { return null; }
            return new Change_Effect_Name(effect, changeType, parameters);
        }

    }

}
