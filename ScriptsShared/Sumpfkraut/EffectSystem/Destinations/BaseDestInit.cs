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

    /// <summary>
    /// Most basic class destination initialization objects. It is not marked abstract
    /// but can be seen as such, thus, allowing to create the first static 
    /// representative-member.
    /// </summary>
    public partial class BaseDestInit : ExtendedObject
    {

        /// <summary>
        /// Hold all registered ChangeDestinations with their related ChangeTypes
        /// as well as functions to possibly calculate and apply final values
        /// to host objects.
        /// </summary>
        protected static Dictionary<ChangeDestination, DestInitInfo> changeDestinationToInfo;
        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        public static BaseDestInit representative;



        static BaseDestInit ()
        {
            // init changeDestinationToInfo which is used by all children
            changeDestinationToInfo = new Dictionary<ChangeDestination, DestInitInfo>();
            // always create own representative
            representative = new BaseDestInit();
        }

        /// <summary>
        /// This constructure exists solely to allow the instantiation of a representative
        /// of this base class. Without the representative, this whole class could be abstract.
        /// </summary>
        protected BaseDestInit ()
        { }



        /// <summary>
        /// Add or replace DestInitInfo-objects, thus changing how other objects
        /// are influenced by the EffectSystem.
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
        /// Try to retrieve currently active DestInitInfo for a ChangeDestination.
        /// </summary>
        /// <param name="changeDest"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool TryGetDestInitInfo (ChangeDestination changeDest, out DestInitInfo info)
        {
            return changeDestinationToInfo.TryGetValue(changeDest, out info);
        }

        /// <summary>
        /// Static helper method to get the calculated total change applied by an EffectHandler
        /// on its host object for a ChangeDestination.
        /// </summary>
        /// <param name="effectHandler"></param>
        /// <param name="changeDest"></param>
        /// <param name="totalChange"></param>
        /// <returns></returns>
        public static bool TryGetTotalChange (BaseEffectHandler effectHandler, ChangeDestination changeDest,
            out TotalChange totalChange)
        {
            return effectHandler.TryGetTotalChange(changeDest, out totalChange);
        }



        /// <summary>
        /// Make sure, that ChangeType was properly registered or show an error message if not
        /// while retrieving the ChangeInitInfo for the parameter types
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
