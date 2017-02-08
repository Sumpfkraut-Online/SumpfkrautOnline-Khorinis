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



        public static bool TryGetDestInitInfo (ChangeDestination changeDestination, out DestInitInfo info)
        {
            return changeDestinationToInfo.TryGetValue(changeDestination, out info);
        }

        protected void AddOrChange (DestInitInfo inputInfo)
        {
            MakeLog("Initializing changeDestination " + inputInfo.ChangeDestination);

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
        }

    }

}
