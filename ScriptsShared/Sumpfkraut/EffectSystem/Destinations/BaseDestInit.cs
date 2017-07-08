using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers.BaseEffectHandler;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class BaseDestInit : ExtendedObject
    {

        protected static Dictionary<ChangeDestination, DestInitInfo> changeDestinationToInfo;
        // static representative of the class (do not change it in any way after instantiation!)
        public static BaseDestInit representative;



        static BaseDestInit ()
        {
            // init changeDestinationToInfo which is used by all children
            changeDestinationToInfo = new Dictionary<ChangeDestination, DestInitInfo>();
            // always create own representative
            representative = new BaseDestInit();
            representative.SetObjName("BaseDestInit");
        }

        protected BaseDestInit ()
        { }



        protected void AddOrChange (DestInitInfo inputInfo)
        {
            DestInitInfo info;
            if (changeDestinationToInfo.TryGetValue(inputInfo.ChangeDestination, out info))
            {
                MakeLogWarning("Overwriting entry for ChangeDestination " + inputInfo.ChangeDestination);
                info.SupportedChangeTypes = inputInfo.SupportedChangeTypes;
                info.CalculateTotalChange = inputInfo.CalculateTotalChange;
                info.ApplyTotalChange = inputInfo.ApplyTotalChange;
            }
            else
            {
                changeDestinationToInfo.Add(inputInfo.ChangeDestination, inputInfo);
            }

            // trigger static initialization of the necessary BaseChangeInit-classes
            int i;
            for (i = 0; i < inputInfo.SupportedChangeTypes.Count; i++)
            {
                
            }
        }

        public static bool TryGetDestInitInfo (ChangeDestination changeDest, out DestInitInfo info)
        {
            return changeDestinationToInfo.TryGetValue(changeDest, out info);
        }

        public static bool TryGetTotalChange (BaseEffectHandler effectHandler, ChangeDestination changeDest,
            out TotalChange totalChange)
        {
            totalChange = null;
            if (effectHandler.TryGetTotalChange(changeDest, out totalChange)) { return true; }
            return false;
        }




        // make sure, that ChangeType was properly registered or show an error message if not
        // while retrieving the ChangeInitInfo for the parameter types
        public bool ValidateChangeInit (ChangeType changeType, out ChangeInitInfo info)
        {
            if (BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_CodeName_Set, out info))
            {
                return true;
            }
            else
            {
                MakeLogError("Tried to calculate TotalChange with non-initialized ChangeType "
                    + changeType);
                return false;
            }   
        }

    }

}
