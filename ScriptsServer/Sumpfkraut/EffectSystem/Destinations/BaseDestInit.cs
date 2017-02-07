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

    public class BaseDestInit : ExtendedObject
    {

        new public static readonly string _staticName = "BaseDestInit (static)";
        protected static Dictionary<ChangeDestination, DestinationInfo> changeDestinationToInfo;
        public static BaseDestInit representative;



        static BaseDestInit ()
        {
            // init changeDestinationToInfo which is used by all children
            changeDestinationToInfo = new Dictionary<ChangeDestination, DestinationInfo>();
            // always create own representative
            representative = new BaseDestInit();
            representative.SetObjName("BaseDestInit");
        }

        protected BaseDestInit ()
        { }



        public static bool TryGetDestinationInfo (ChangeDestination changeDestination, out DestinationInfo info)
        {
            return changeDestinationToInfo.TryGetValue(changeDestination, out info);
        }

        protected void AddOrChange (ChangeDestination changeDestination, List<ChangeType> supportedChangeTypes,
            CalculateTotalChange calculateTotalChange, ApplyTotalChange applyTotalChange)
        {
            DestinationInfo info;

            MakeLog("Initializing changeDestination " + changeDestination);

            if (changeDestinationToInfo.TryGetValue(changeDestination, out info))
            {
                MakeLogWarning("Overwriting changeDestination: " + changeDestination);
                info.supportedChangeTypes = supportedChangeTypes;
                info.calculateTotalChange = calculateTotalChange;
                info.applyTotalChange = applyTotalChange;
            }
            else
            {
                info = new DestinationInfo(changeDestination, supportedChangeTypes,
                    calculateTotalChange, applyTotalChange);
                changeDestinationToInfo.Add(changeDestination, info);
            }
        }

    }

}
