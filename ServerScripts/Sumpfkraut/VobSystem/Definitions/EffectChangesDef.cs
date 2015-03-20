using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{

    enum EffectChangesEnum
    {
        IsGold,
        IsKeyInstance,
        IsTorch,
        IsTorchBurning,
        IsTorchBurned,

        Effect,
        Spell,

        Wear,
        DamageType,
        Range,
        TotalDamage,
        Damages,
        Munition,

        Protection,

        OnUse_HPChange,
        OnUse_HPMaxChange,
        OnUse_MPChange,
        OnUse_MPMaxChange,

        OnEquip_HPChange,
        OnEquip_HPMaxChange,
        OnEquip_MPChange,
        OnEquip_MPMaxChange,

        OnUnEquip_HPChange,
        OnUnEquip_HPMaxChange,
        OnUnEquip_MPChange,
        OnUnEquip_MPMaxChange,
    }

    /**
     *   Class from which all additional effects (not only visual) 
     *   are instatiated (which are handled by the serverscript).
     */
    class EffectChangesDef
    {

        /**
         *   Dictionary which holds all EffectChanges for faster access.
         *   May be deprecated later if only using the database directly.
         */
        private static Dictionary<EffectChangesEnum, string> EffectChangesDict = new Dictionary<EffectChangesEnum, string>();

        private static Object dictLock = new Object();

        /**
         *   May be deprecated later if only using the databse directly.
         */
        public static void Add (EffectChangesEnum changeType, string param)
        {
            if ((changeType != null) && (param != null))
            {
                lock (dictLock)
                {
                    EffectChangesDict.Add(changeType, param);
                }
            }
        }

        /**
         *   May be deprecated later if only using the databse directly.
         */
        public static void Edit (EffectChangesEnum changeType, string param, bool createNew = true)
        {
            if ((changeType != null) && (param != null))
            {
                lock (dictLock)
                {
                    if (EffectChangesDict.ContainsKey(changeType))
                    {
                        EffectChangesDict[changeType] = param;
                    }
                    else
                    {
                        if (createNew)
                        {
                            EffectChangesDict.Add(changeType, param);
                        }
                    }
                }
            }
        }

        /**
         *   May be deprecated later if only using the databse directly.
         */
        public static void Remove (EffectChangesEnum changeType)
        {
            if (changeType != null)
            {
                lock (dictLock)
                {
                    if (EffectChangesDict.ContainsKey(changeType))
                    {
                        EffectChangesDict.Remove(changeType);
                    }
                }
            }
        }

        /**
         *   Apply effect changes to an item-defintiion, so derived items can use them.
         */
        public static void ApplyEffectChanges (ref ItemDef def, EffectChangesEnum changeType, string param)
        {
            try
            {
                // please don't kill me for this switch case... it could use a more OOP-like replacement
                switch (changeType)
                {
                    case (EffectChangesEnum.IsGold):
                        bool isGold = false;
                        if (bool.TryParse(param, out isGold))
                        {
                            def.setIsGold(isGold);
                        }
                        
                        return;
                    //case (EffectChangesEnum.IsKeyInstance):
                    //    def.IsKeyInstance = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsTorch):
                        bool isTorch = false;
                        if (bool.TryParse(param, out isTorch))
                        {
                            def.setIsTorch(isTorch);
                        }
                        return;
                    case (EffectChangesEnum.IsTorchBurning):
                        bool isTorchBurning = false;
                        if (bool.TryParse(param, out isTorchBurning))
                        {
                            def.setIsTorchBurning(isTorchBurning);
                        }
                        return;
                    case (EffectChangesEnum.IsTorchBurned):
                        bool isTorchBurned = false;
                        if (bool.TryParse(param, out isTorchBurned))
                        {
                            def.setIsTorchBurned(isTorchBurned);
                        }
                        return;

                    case (EffectChangesEnum.Effect):
                        if (param != null)
                        {
                            def.setEffect(param);
                        }
                        return;
                    case (EffectChangesEnum.Spell):
                        // !!! TO DO: requires SpellDef --> resulting SpellInst !!!
                        return;

                    case (EffectChangesEnum.Wear):
                        int wear = 0;
                        if (Int32.TryParse(param, out wear))
                        {
                            def.setWear((Enumeration.ArmorFlags) wear);
                        }
                        return;
                    case (EffectChangesEnum.DamageType):
                        int damageType = 0;
                        if (Int32.TryParse(param, out damageType))
                        {
                            def.setDamageType((Enumeration.DamageTypes) damageType);
                        }
                        return;
                    case (EffectChangesEnum.Range):
                        int addRange = 0;
                        if (Int32.TryParse(param, out addRange))
                        {
                            def.setRange(def.getRange() + addRange);
                        }
                        return;
                    case (EffectChangesEnum.TotalDamage):
                        int addTotalDamage = 0;
                        if (Int32.TryParse(param, out addTotalDamage))
                        {
                            def.setTotalDamage(def.getTotalDamage() + addTotalDamage);
                        }
                        return;
                    //case (EffectChangesEnum.Damages):
                    //    // !!! TO DO: multiple entries !!!
                    //    // ??? should setting them even possible ???
                    //    return;
                    //case (EffectChangesEnum.Munition):
                    //    // !!! TO DO: requires ItemDef --> resulting ItemInst !!!
                    //    return;

                    case (EffectChangesEnum.Protection):
                        // param string should be of pattern 0=0,1=0,2=25,...
                        string[] data = param.Split(new char[]{',', '='});
                        if ((data != null) && (data.Length > 0))
                        {
                            int dti = 0;
                            int val = 0;
                            int i = 0;
                            while (i < data.Length)
                            {
                                if ((i + 1) >= data.Length)
                                {
                                    break;
                                }

                                try
                                {
                                    if (Int32.TryParse(data[i], out dti) && Int32.TryParse(data[i + 1], out val))
                                    {
                                        def.setProtection((Enumeration.DamageTypeIndex) dti, 
                                            def.getProtection((Enumeration.DamageTypeIndex) dti) + val);
                                    }
                                    else
                                    {
                                        throw new Exception("Couldn't convert part of param-string to int or enum-entry DamageTypeIndex.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Couldn't cast converted part of param-string from int to enum-entry DamageTypeIndex: " + ex);
                                }
                                
                                i += 2;
                            }
                        }
                        return;

                    // OnUse
                    case (EffectChangesEnum.OnUse_HPChange):
                        int OnUse_addHPChange = 0;
                        if (Int32.TryParse(param, out OnUse_addHPChange))
                        {
                            def.setOnUse_HPChange(def.getOnUse_HPChange() + OnUse_addHPChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUse_HPChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnUse_HPMaxChange):
                        int OnUse_addHPMaxChange = 0;
                        if (Int32.TryParse(param, out OnUse_addHPMaxChange))
                        {
                            def.setOnUse_HPMaxChange(def.getOnUse_HPMaxChange() + OnUse_addHPMaxChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUse_HPMaxChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnUse_MPChange):
                        int OnUse_addMPChange = 0;
                        if (Int32.TryParse(param, out OnUse_addMPChange))
                        {
                            def.setOnUse_MPChange(def.getOnUse_MPChange() + OnUse_addMPChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUse_MPChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnUse_MPMaxChange):
                        int OnUse_addMPMaxChange = 0;
                        if (Int32.TryParse(param, out OnUse_addMPMaxChange))
                        {
                            def.setOnUse_MPMaxChange(def.getOnUse_MPMaxChange() + OnUse_addMPMaxChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUse_MPMaxChange.");
                        }
                        return;

                    // OnEquip
                    case (EffectChangesEnum.OnEquip_HPChange):
                        int OnEquip_addHPChange = 0;
                        if (Int32.TryParse(param, out OnEquip_addHPChange))
                        {
                            def.setOnEquip_HPChange(def.getOnEquip_HPChange() + OnEquip_addHPChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnEquip_HPChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnEquip_HPMaxChange):
                        int OnEquip_addHPMaxChange = 0;
                        if (Int32.TryParse(param, out OnEquip_addHPMaxChange))
                        {
                            def.setOnEquip_HPMaxChange(def.getOnEquip_HPMaxChange() + OnEquip_addHPMaxChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnEquip_HPMaxChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnEquip_MPChange):
                        int OnEquip_addMPChange = 0;
                        if (Int32.TryParse(param, out OnEquip_addMPChange))
                        {
                            def.setOnEquip_MPChange(def.getOnEquip_MPChange() + OnEquip_addMPChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnEquip_MPChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnEquip_MPMaxChange):
                        int OnEquip_addMPMaxChange = 0;
                        if (Int32.TryParse(param, out OnEquip_addMPMaxChange))
                        {
                            def.setOnEquip_MPMaxChange(def.getOnEquip_MPMaxChange() + OnEquip_addMPMaxChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnEquip_MPMaxChange.");
                        }
                        return;

                    // OnUnEquip
                    case (EffectChangesEnum.OnUnEquip_HPChange):
                        int OnUnEquip_addHPChange = 0;
                        if (Int32.TryParse(param, out OnUnEquip_addHPChange))
                        {
                            def.setOnUnEquip_HPChange(def.getOnUnEquip_HPChange() + OnUnEquip_addHPChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUnEquip_HPChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnUnEquip_HPMaxChange):
                        int OnUnEquip_addHPMaxChange = 0;
                        if (Int32.TryParse(param, out OnUnEquip_addHPMaxChange))
                        {
                            def.setOnUnEquip_HPMaxChange(def.getOnUnEquip_HPMaxChange() + OnUnEquip_addHPMaxChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUnEquip_HPMaxChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnUnEquip_MPChange):
                        int OnUnEquip_addMPChange = 0;
                        if (Int32.TryParse(param, out OnUnEquip_addMPChange))
                        {
                            def.setOnUnEquip_MPChange(def.getOnUnEquip_MPChange() + OnUnEquip_addMPChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUnEquip_MPChange.");
                        }
                        return;
                    case (EffectChangesEnum.OnUnEquip_MPMaxChange):
                        int OnUnEquip_addMPMaxChange = 0;
                        if (Int32.TryParse(param, out OnUnEquip_addMPMaxChange))
                        {
                            def.setOnUnEquip_MPMaxChange(def.getOnUnEquip_MPMaxChange() + OnUnEquip_addMPMaxChange);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying OnUnEquip_MPMaxChange.");
                        }
                        return;

                    default:
                        throw new Exception("There exists no effect-change for EffectChangesEnum changeType="
                            + changeType + ".");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't process effect changes for EffectChangesEnum changeType=" 
                    + changeType + " and with string param=" + param + ": " + ex);
            }
        }

    }

}
