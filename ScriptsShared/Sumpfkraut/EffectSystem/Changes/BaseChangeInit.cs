using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    /// <summary>
    /// Quasi-static base singleton to initialize/register ChangeTypes and their
    /// typed parameter list. Use static constructor to initialize the representative
    /// singleton instance.
    /// </summary>
    public class BaseChangeInit : ExtendedObject
    {

        /// <summary>
        /// Mapping from relevant ChangeTypes to ChangeInitInfo-objects which make the
        /// connection between Changes and ChangeDestinations.
        /// </summary>
        protected static Dictionary<ChangeType, ChangeInitInfo> changeTypeToInfo;
        /// <summary>
        /// A static representative of the class (do not change it in any way after instantiation!).
        /// </summary>
        public static BaseChangeInit representative;



        static BaseChangeInit ()
        {
            // init changeDestinationToInfo which is used by all children
            changeTypeToInfo = new Dictionary<ChangeType, ChangeInitInfo>();

            // always create own representative in inheriting classes
            representative = new BaseChangeInit();
        }

        protected BaseChangeInit ()
        { }



        public static bool TryGetChangeInitInfo (ChangeType changeType, out ChangeInitInfo info)
        {
            return changeTypeToInfo.TryGetValue(changeType, out info);
        }

        /// <summary>
        /// Add or change existing included type of Change and its respective parameter types.
        /// </summary>
        /// <param name="inputInfo"></param>
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
