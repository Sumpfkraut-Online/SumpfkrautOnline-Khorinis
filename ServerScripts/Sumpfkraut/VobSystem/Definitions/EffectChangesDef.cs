using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{

    enum EffectChangesEnum
    {
        IsGold                      = 100,
        IsKeyInstance               = 101,
        IsTorch                     = 102,
        IsTorchBurning              = 103,
        IsTorchBurned               = 104,

        Effect                      = 105,
        Spell                       = 106,

        Wear                        = 107,
        DamageType                  = 108,
        Range                       = 109,
        TotalDamage                 = 110,
        Damages                     = 111,
        Munition                    = 112,

        Protection                  = 113,

        HPChange                    = 114,
        HPMaxChange                 = 115,
        MPChange                    = 116,
        MPMaxChange                 = 117,
    }

    /**
     *   Class from which all additional effects (not only visual) 
     *   are instatiated (which are handled by the serverscript).
     */
    class EffectChangesDef
    {

        /**
         *   Dictionary which holds all EffectChanges for faster access.
         *   May be deprecated later if only using the databse directly.
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
                switch (changeType)
                {
                    //case (EffectChangesEnum.IsGold):
                    //    def.IsGold = Convert.ToBoolean(param);
                    //    return;
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
                    //case (EffectChangesEnum.IsTorch):
                    //    def.IsTorch = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsTorch):
                        bool isTorch = false;
                        if (bool.TryParse(param, out isTorch))
                        {
                            def.setIsTorch(isTorch);
                        }
                        return;
                    //case (EffectChangesEnum.IsTorchBurning):
                    //    def.IsTorchBurning = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsTorchBurning):
                        bool isTorchBurning = false;
                        if (bool.TryParse(param, out isTorchBurning))
                        {
                            def.setIsTorchBurning(isTorchBurning);
                        }
                        return;
                    //case (EffectChangesEnum.IsTorchBurned):
                    //    def.IsTorchBurned = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsTorchBurned):
                        bool isTorchBurned = false;
                        if (bool.TryParse(param, out isTorchBurned))
                        {
                            def.setIsTorchBurned(isTorchBurned);
                        }
                        return;

                    //case (EffectChangesEnum.Effect):
                    //    def.Effect = param;
                    //    return;
                    case (EffectChangesEnum.Effect):
                        if (param != null)
                        {
                            def.setEffect(param);
                        }
                        return;
                    case (EffectChangesEnum.Spell):
                        // !!! TO DO: requires SpellDef --> resulting SpellInst !!!
                        return;

                    //case (EffectChangesEnum.Wear):
                    //    def.Wear = (Enumeration.ArmorFlags) Convert.ToInt32(param);
                    //    return;
                    case (EffectChangesEnum.Wear):
                        int wear = 0;
                        if (Int32.TryParse(param, out wear))
                        {
                            def.setWear((Enumeration.ArmorFlags) wear);
                        }
                        return;
                    //case (EffectChangesEnum.DamageType):
                    //    def.DamageType = (Enumeration.DamageType) Convert.ToInt32(param);
                    //    return;
                    case (EffectChangesEnum.DamageType):
                        int damageType = 0;
                        if (Int32.TryParse(param, out damageType))
                        {
                            def.setDamageType((Enumeration.DamageTypes) damageType);
                        }
                        return;
                    //case (EffectChangesEnum.Range):
                    //    def.Range = Convert.ToInt32(param);
                    //    return;
                    case (EffectChangesEnum.Range):
                        int addRange = 0;
                        if (Int32.TryParse(param, out addRange))
                        {
                            def.setRange(def.getRange() + addRange);
                        }
                        return;
                    //case (EffectChangesEnum.TotalDamage):
                    //    def.TotalDamage = Convert.ToInt32(param);
                    //    return;
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

                    //case (EffectChangesEnum.HPChange):
                    //    int addHPChange = 0;
                    //    if (Int32.TryParse(param, out addHPChange))
                    //    {
                    //        def.setHPChange(def.getHPChange() + addHPChange);
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("Couldn't convert part of param-string to int while applying HPChange.");
                    //    }
                    //    return;
                    //case (EffectChangesEnum.HPMaxChange):
                    //    int addHPMaxChange = 0;
                    //    if (Int32.TryParse(param, out addHPMaxChange))
                    //    {
                    //        def.setHPMaxChange(def.getHPMaxChange() + addHPMaxChange);
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("Couldn't convert part of param-string to int while applying HPMaxChange.");
                    //    }
                    //    return;
                    //case (EffectChangesEnum.MPChange):
                    //    int addMPChange = 0;
                    //    if (Int32.TryParse(param, out addMPChange))
                    //    {
                    //        def.setMPChange(def.getMPChange() + addMPChange);
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("Couldn't convert part of param-string to int while applying MPChange.");
                    //    }
                    //    return;
                    //case (EffectChangesEnum.MPMaxChange):
                    //    def.setMPMaxChange(Convert.ToInt32(param));
                    //    int addMPMaxChange = 0;
                    //    if (Int32.TryParse(param, out addMPMaxChange))
                    //    {
                    //        def.setMPMaxChange(def.getMPMaxChange() + addMPMaxChange);
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("Couldn't convert part of param-string to int while applying MPMaxChange.");
                    //    }
                    //    return;

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
