using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{

    enum EffectChangesEnum
    {
        // 1000 - 1999 reserved for ItemDef effect changes
        IsGold                      = 1000,
        IsKeyInstance               = EffectChangesEnum.IsGold + 1,
        IsTorch                     = EffectChangesEnum.IsKeyInstance + 1,                   
        IsTorchBurning              = EffectChangesEnum.IsTorch + 1,
        IsTorchBurned               = EffectChangesEnum.IsTorchBurning + 1,

        Effect                      = EffectChangesEnum.IsTorchBurned + 1,
        Spell                       = EffectChangesEnum.Effect + 1,

        Wear                        = EffectChangesEnum.Spell + 1,
        DamageType                  = EffectChangesEnum.Wear + 1,
        Range                       = EffectChangesEnum.DamageType + 1,
        TotalDamage                 = EffectChangesEnum.Range + 1,
        Damages                     = EffectChangesEnum.TotalDamage + 1,
        Munition                    = EffectChangesEnum.Damages + 1,

        Protection                  = EffectChangesEnum.Munition + 1,

        Description                 = EffectChangesEnum.Protection + 1,
        Text0                       = EffectChangesEnum.Description + 1,
        Text1                       = EffectChangesEnum.Text0 + 1,
        Text2                       = EffectChangesEnum.Text1 + 1,
        Text3                       = EffectChangesEnum.Text2 + 1,
        Text4                       = EffectChangesEnum.Text3 + 1,
        Text5                       = EffectChangesEnum.Text4 + 1,
        Count0                      = EffectChangesEnum.Text5 + 1,
        Count1                      = EffectChangesEnum.Count0 + 1,
        Count2                      = EffectChangesEnum.Count1 + 1,
        Count3                      = EffectChangesEnum.Count2 + 1,
        Count4                      = EffectChangesEnum.Count3 + 1,
        Count5                      = EffectChangesEnum.Count4 + 1,

        OnUse_HPChange              = EffectChangesEnum.Count5 + 1,
        OnUse_HPMaxChange           = EffectChangesEnum.OnUse_HPChange + 1,
        OnUse_MPChange              = EffectChangesEnum.OnUse_HPMaxChange + 1,
        OnUse_MPMaxChange           = EffectChangesEnum.OnUse_MPChange + 1,

        OnEquip_HPChange            = EffectChangesEnum.OnUse_MPMaxChange + 1,
        OnEquip_HPMaxChange         = EffectChangesEnum.OnEquip_HPChange + 1,
        OnEquip_MPChange            = EffectChangesEnum.OnEquip_HPMaxChange + 1,
        OnEquip_MPMaxChange         = EffectChangesEnum.OnEquip_MPChange + 1,

        OnUnEquip_HPChange          = EffectChangesEnum.OnEquip_MPMaxChange + 1,
        OnUnEquip_HPMaxChange       = EffectChangesEnum.OnUnEquip_HPChange + 1,
        OnUnEquip_MPChange          = EffectChangesEnum.OnUnEquip_HPMaxChange + 1,
        OnUnEquip_MPMaxChange       = EffectChangesEnum.OnUnEquip_MPChange + 1,
    }

    /**
     *   Class from which all additional effects (not only visual) 
     *   are instatiated (which are handled by the serverscript).
     */
    class EffectChangesDef
    {

        private static Dictionary<int, List<object>> EffectChangesDefDict = new Dictionary<int, List<object>>();

        private static Object dictLock = new Object();

        public static void Add (int id, EffectChangesEnum changeType, string param, bool replace=false)
        {
            lock (dictLock)
            {
                if ((Enum.IsDefined(typeof(EffectChangesEnum), changeType)) && (param != null))
                {
                    List<object> entry = null;
                    if (EffectChangesDefDict.TryGetValue(id, out entry))
                    {
                        if (replace)
                        {
                            entry[0] = changeType;
                            entry[1] = param;
                        }
                        else
                        {
                            Log.Logger.logWarning("ID " + id + " in EffectChangesDefDict already occupied"
                                + " and replace-parameter is set to false.");
                        }
                    }
                    else
                    {
                        entry = new List<object>(){changeType, param};
                        EffectChangesDefDict.Add(id, entry);
                    }
                }
            }
        }

        public static bool TryGetValue (int id, out List<object> effectChange)
        {
            lock (dictLock)
            {
                if (EffectChangesDefDict.TryGetValue(id, out effectChange))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void Remove (int id)
        {
            lock (dictLock)
            {
                if (EffectChangesDefDict.ContainsKey(id))
                {
                    EffectChangesDefDict.Remove(id);
                }
            }
        }

        /**
         *   Apply effect changes to an item-defintiion, so derived items can use them.
         *   This method includes similar type-conversion as in Sumpfkraut.DataBase.DBTables.SqlStringToData. 
         *   It handles the special multiparameter-types of effect change definitions, however.
         *   @see Sumpfkraut.Database.DBTables.SqlStringToData
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


                    // descriptive parameters
                    case (EffectChangesEnum.Description):
                        def.setDescription(param);
                        return;
                    case (EffectChangesEnum.Text0):
                        def.setText0(param);
                        return;
                    case (EffectChangesEnum.Text1):
                        def.setText1(param);
                        return;
                    case (EffectChangesEnum.Text2):
                        def.setText2(param);
                        return;
                    case (EffectChangesEnum.Text3):
                        def.setText3(param);
                        return;
                    case (EffectChangesEnum.Text4):
                        def.setText4(param);
                        return;
                    case (EffectChangesEnum.Text5):
                        def.setText5(param);
                        return;
                    case (EffectChangesEnum.Count0):
                        int Count0 = -1;
                        if (int.TryParse(param, out Count0))
                        {
                            def.setCount0(Count0);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count0.");
                        }
                        return;
                    case (EffectChangesEnum.Count1):
                        int Count1 = -1;
                        if (int.TryParse(param, out Count1))
                        {
                            def.setCount1(Count1);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count1.");
                        }
                        return;
                    case (EffectChangesEnum.Count2):
                        int Count2 = -1;
                        if (int.TryParse(param, out Count2))
                        {
                            def.setCount2(Count2);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count2.");
                        }
                        return;
                    case (EffectChangesEnum.Count3):
                        int Count3 = -1;
                        if (int.TryParse(param, out Count3))
                        {
                            def.setCount3(Count3);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count3.");
                        }
                        return;
                    case (EffectChangesEnum.Count4):
                        int Count4 = -1;
                        if (int.TryParse(param, out Count4))
                        {
                            def.setCount4(Count4);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count4.");
                        }
                        return;
                    case (EffectChangesEnum.Count5):
                        int Count5 = -1;
                        if (int.TryParse(param, out Count5))
                        {
                            def.setCount5(Count5);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count5.");
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

        public static void ApplyToDummy (ref DummyItemDef def, List<object> effectChangesDef)
        {
            if (effectChangesDef.Count >= 2){
                try
                {
                    EffectChangesEnum changeType = (EffectChangesEnum) effectChangesDef[0];
                    string param = (string) effectChangesDef[1];
                    ApplyToDummy(ref def, changeType, param);
                }
                catch
                {
                    throw new Exception ("ApplyToDummy: No valid changeType could");
                }
            }
            else
            {
                throw new Exception ("ApplyToDummy: Parameter effectChangesDef is not of length >= 2!");
            }
            
        }

        public static void ApplyToDummy (ref DummyItemDef def, EffectChangesEnum changeType, string param)
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


                    // descriptive parameters
                    case (EffectChangesEnum.Description):
                        def.setDescription(param);
                        return;
                    case (EffectChangesEnum.Text0):
                        def.setText0(param);
                        return;
                    case (EffectChangesEnum.Text1):
                        def.setText1(param);
                        return;
                    case (EffectChangesEnum.Text2):
                        def.setText2(param);
                        return;
                    case (EffectChangesEnum.Text3):
                        def.setText3(param);
                        return;
                    case (EffectChangesEnum.Text4):
                        def.setText4(param);
                        return;
                    case (EffectChangesEnum.Text5):
                        def.setText5(param);
                        return;
                    case (EffectChangesEnum.Count0):
                        int Count0 = -1;
                        if (int.TryParse(param, out Count0))
                        {
                            def.setCount0(Count0);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count0.");
                        }
                        return;
                    case (EffectChangesEnum.Count1):
                        int Count1 = -1;
                        if (int.TryParse(param, out Count1))
                        {
                            def.setCount1(Count1);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count1.");
                        }
                        return;
                    case (EffectChangesEnum.Count2):
                        int Count2 = -1;
                        if (int.TryParse(param, out Count2))
                        {
                            def.setCount2(Count2);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count2.");
                        }
                        return;
                    case (EffectChangesEnum.Count3):
                        int Count3 = -1;
                        if (int.TryParse(param, out Count3))
                        {
                            def.setCount3(Count3);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count3.");
                        }
                        return;
                    case (EffectChangesEnum.Count4):
                        int Count4 = -1;
                        if (int.TryParse(param, out Count4))
                        {
                            def.setCount4(Count4);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count4.");
                        }
                        return;
                    case (EffectChangesEnum.Count5):
                        int Count5 = -1;
                        if (int.TryParse(param, out Count5))
                        {
                            def.setCount5(Count5);
                        }
                        else
                        {
                            throw new Exception("Couldn't convert part of param-string to int while applying Count5.");
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
