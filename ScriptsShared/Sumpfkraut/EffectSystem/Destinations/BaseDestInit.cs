using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System.Collections.Generic;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// Manages the ChangeDestination registration process.
    /// All subclasses prepare and register their DestInitInfo-objects,
    /// so they can be kept here in a centralized manner.
    /// </summary>
    public class BaseDestInit : ExtendedObject
    {

        /// <summary>
        /// Hold all registered ChangeDestinations with their related ChangeTypes
        /// as well as functions to possibly calculate and apply final values
        /// to host objects.
        /// </summary>
        protected static readonly Dictionary<ChangeDestination, DestInitInfo> changeDestinationToInfo;
        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        public static readonly BaseDestInit representative;



        /// <summary>
        /// Ensures initialization of important collections.
        /// </summary>
        static BaseDestInit ()
        {
            // init changeDestinationToInfo which is used by all children
            changeDestinationToInfo = new Dictionary<ChangeDestination, DestInitInfo>();
            // always create own representative
            representative = new BaseDestInit();
        }

        /// <summary>
        /// Dummy.
        /// </summary>
        protected BaseDestInit ()
        { }



        /// <summary>
        /// Register a new ChangeDestination with related initialization 
        /// information or override the current one.
        /// </summary>
        /// <param name="inputInfo"></param>
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

            //// trigger static initialization of the necessary BaseChangeInit-classes
            //int i;
            //for (i = 0; i < inputInfo.SupportedChangeTypes.Count; i++)
            //{
                
            //}
        }

        /// <summary>
        /// Retrieves DestInitInfo-object for a registered ChangeDestination.
        /// Indicates success through the return value.
        /// </summary>
        /// <param name="changeDest"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool TryGetDestInitInfo (ChangeDestination changeDest, out DestInitInfo info)
        {
            return changeDestinationToInfo.TryGetValue(changeDest, out info);
        }

        /// <summary>
        /// Retrieves DestInitInfo-object for a registered ChangeDestination.
        /// Indicates success through the return value.
        /// </summary>
        /// <param name="effectHandler"></param>
        /// <param name="changeDest"></param>
        /// <param name="totalChange"></param>
        /// <returns></returns>
        public static bool TryGetTotalChange (BaseEffectHandler effectHandler, ChangeDestination changeDest,
            out TotalChange totalChange)
        {
            totalChange = null;
            if (effectHandler.TryGetTotalChange(changeDest, out totalChange)) { return true; }
            return false;
        }




        /// <summary>
        /// Make sure that ChangeType was properly registered or show an error message if not
        /// while retrieving the ChangeInitInfo for the parameter types.
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="info"></param>
        /// <returns></returns>
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
