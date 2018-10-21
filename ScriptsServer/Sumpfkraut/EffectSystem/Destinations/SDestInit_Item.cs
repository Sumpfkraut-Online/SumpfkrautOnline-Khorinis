using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// ChangeDestination registration and functionality for item vobs.
    /// </summary>
    public class SDestInit_Item : BaseDestInit, DestInit_Item
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static readonly SDestInit_Item representative;

        /// <summary>
        /// Default item material.
        /// </summary>
        public readonly ItemMaterials Default_Material = ItemMaterials.Wood;

        /// <summary>
        /// Ensures coupling of ChangeDestinations to >= 1 ChangeTypes
        /// which are relevant for item vobs.
        /// </summary>
        static SDestInit_Item ()
        {
            representative = new SDestInit_Item();
        }

        /// <summary>
        /// Called on static constructor to initialize singleton object.
        /// </summary>
        protected SDestInit_Item ()
        {
            AddOrChange(new DestInitInfo(ChangeDestination.Item_Material,
                new List<ChangeType>() { ChangeType.Item_Material_Set },
                CalculateMaterial, ApplyMaterial));
        }

        public void CalculateMaterial (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Item_Material_Set, out info)) { return; }

                ItemMaterials material;
                if (tc.Components.Count < 1) { material = Default_Material; }
                else
                {
                    // last entry counts
                    material = (ItemMaterials) tc.Components[0].GetParameters()[0];
                }

                var finalChange = Change.Create(info, new List<object>() { material });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex)
            {
                MakeLogError("Error while caclulating TotalChange via pCalculateMaterial: " + ex);
            }
        }

        public void ApplyMaterial (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var host = eh.Host;
                if      (host is ItemDef)
                {
                    var vobDef = host as ItemDef;
                    vobDef.Material = (ItemMaterials) tc.GetTotal().GetParameters()[0];
                }
                else if (host is ItemInst)
                {
                    var vobInst = host as ItemInst;
                    // TO DO when there is the possiblity to change the name in VobSystem
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Error while applying TotalChange via pApplyMaterial: " + ex);
            }
        }

    }

}
