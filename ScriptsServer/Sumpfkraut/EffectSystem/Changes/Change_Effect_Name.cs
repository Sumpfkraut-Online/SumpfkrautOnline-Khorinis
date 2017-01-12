using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class Change_Effect_Name : BaseChange
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



        protected Change_Effect_Name (Effect effect, ChangeType changeType, object[] parameters) 
            : base(effect, changeType, parameters)
        {
            SetObjName("Change_Effect_Name (default)");
        }



        // use this method to create the objects
        new public static Change_Effect_Name Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            if (!CreateCheckBasics(effect, changeType, parameters, parameterTypes)) { return null; }
            return new Change_Effect_Name(effect, changeType, parameters);
        }



        // became useless because there is not only 1 effect but more and they will be assigned names seperately
        public override void CalculateTotalChange (BaseEffectHandler effectHandler)
        {
            // grab the last effect name and use it
            try
            {
                string finalName = "";
                TotalChange totalChange;

                if (!effectHandler.DestToTotalChange.TryGetValue(
                    ChangeDestination.Effect_Name, out totalChange))
                {
                    return;
                }
                if (totalChange == null) { return; }

                BaseChange lastChange = totalChange.Components.Last();
                if (lastChange.Parameters.Length > 0)
                {
                    finalName = totalChange.Components.Last().Parameters[0].ToString();
                }

                totalChange.Total.SetParameters(new string[] { finalName });
            }
            catch (Exception ex)
            {
                MakeLogErrorStatic(typeof(Change_Effect_Name), ex);
            }
        }

        

    }

}
