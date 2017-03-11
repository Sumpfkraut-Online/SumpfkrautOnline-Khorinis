using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class BaseChangeInit : ExtendedObject
    {

        new public static readonly string _staticName = "BaseChangeInit (static)";
        protected static Dictionary<ChangeType, ChangeInitInfo> changeTypeToInfo;
        // static representative of the class (do not change it in any way after instantiation!)
        public static BaseChangeInit representative;



        static BaseChangeInit ()
        {
            // init changeDestinationToInfo which is used by all children
            changeTypeToInfo = new Dictionary<ChangeType, ChangeInitInfo>();

            // always create own representative in inheriting classes
            representative = new BaseChangeInit();
            representative.SetObjName("BaseChangeInit");
        }

        protected BaseChangeInit ()
        { }



        public static bool TryGetChangeInitInfo (ChangeType changeType, out ChangeInitInfo info)
        {
            return changeTypeToInfo.TryGetValue(changeType, out info);
        }
        
        // add or change existing included type of change and its respective parametertypes
        protected void AddOrChange (ChangeInitInfo inputInfo)
        {
            //MakeLog("Initializing ChangeType " + inputInfo.ChangeType);

            ChangeInitInfo info;
            if (changeTypeToInfo.TryGetValue(inputInfo.ChangeType, out info))
            {
                MakeLogWarning("Overwriting entry for ChangeType " + inputInfo.ChangeType);
                info.ParameterTypes = inputInfo.ParameterTypes;
            }
            else
            {
                changeTypeToInfo.Add(inputInfo.ChangeType, inputInfo);
            }
        }

    }

}
