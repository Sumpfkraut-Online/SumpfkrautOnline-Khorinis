using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.TotalEffectCalculation
{

    public class TEC_Effect_Name : TEC_Base
    {

        public override void Init ()
        {
            // map ChangeType(s) to their respective ChangeDestination(s)
            if (BaseEffectHandler.changeTypeToDestinations.ContainsKey(Enumeration.ChangeType.Effect_Name_Set)) { return; }
            BaseEffectHandler.changeTypeToDestinations.Add(Enumeration.ChangeType.Effect_Name_Set, 
                new List<Enumeration.ChangeDestination>() { Enumeration.ChangeDestination.Effect_Name });

            // map ChangeDestination to their respective method for TotalChange-calculation
            if (BaseEffectHandler.destinationToTotalDelegate.ContainsKey(Enumeration.ChangeDestination.Effect_Name)) { return; }
            BaseEffectHandler.destinationToTotalDelegate.Add(Enumeration.ChangeDestination.Effect_Name, TotaChange_Effect_Name);
        }

        protected static void TotaChange_Effect_Name (BaseEffectHandler effectHandler)
        {
            try
            {
                string finalName = "";
                TotalChange totalChange;
                Dictionary<Enumeration.ChangeDestination, TotalChange> destinationToToal = effectHandler.DestinationToTotal;

                if (!destinationToToal.TryGetValue(
                    Enumeration.ChangeDestination.Effect_Name, out totalChange))
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
                MakeLogErrorStatic(typeof(TEC_Effect_Name), ex);
            }
        }

    }

}
