using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class Change_World_Clock : BaseChange
    {

        new public static readonly string _staticName = "Change_Effect_Name (static)";

        new public static readonly ChangeType[] includedChangeTypes = new ChangeType[]
        {
            ChangeType.World_Clock_Time_Set,
            ChangeType.World_Clock_Rate_Set,
            ChangeType.World_Clock_IsRunning_Set
        };

        new public static readonly Type[] parameterTypes = new Type[] 
        {
            typeof(string)
        };

        new public static List<ChangeDestination> influencedDestinations = new List<ChangeDestination>();



        protected Change_World_Clock (Effect effect, ChangeType changeType, object[] parameters) 
            : base(effect, changeType, parameters)
        {
            SetObjName("Change_World_Clock (default)");
        }



        // use this method to create the objects
        new public static Change_World_Clock Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            if (!CheckCreateBasics(effect, changeType, parameters, parameterTypes)) { return null; }
            return new Change_World_Clock(effect, changeType, parameters);
        }

    }

}
