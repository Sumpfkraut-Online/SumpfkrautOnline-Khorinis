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

        new public static readonly Type[] parameterTypes = new Type[] 
        {
            typeof(string)
        };



        public Change_Effect_Name (Effect effect, ChangeType changeType, object[] parameters) 
            : base(effect, changeType, parameters)
        {
            SetObjName("Change_Effect_Name (default)");
        }



        new public static Change_Effect_Name Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            if (!CreateCheckBasics(effect, changeType, parameters, parameterTypes)) { return null; }
            return new Change_Effect_Name(effect, changeType, parameters);
        }



        public override void CalculateTotalChange (BaseEffectHandler effectHandler)
        {
            try
            {
                string finalName = "";
                TotalChange totalChange;
                Dictionary<ChangeDestination, TotalChange> destinationToToal = effectHandler.DestinationToTotal;

                if (!destinationToToal.TryGetValue(
                    ChangeDestination.Effect_Name, out totalChange))
                {
                    return;
                }

                Change lastChange = totalChange.Components.Last();
                if (lastChange.Parameters.Length > 0)
                {
                    finalName = totalChange.Components.Last().Parameters[0].ToString();
                }

                totalChange.Total.SetParametersComplete(new string[] { finalName }, new Type[] { typeof(string) });
            }
            catch (Exception ex)
            {
                MakeLogErrorStatic(typeof(Change_Effect_Name), ex);
            }
        }

        

    }

}
