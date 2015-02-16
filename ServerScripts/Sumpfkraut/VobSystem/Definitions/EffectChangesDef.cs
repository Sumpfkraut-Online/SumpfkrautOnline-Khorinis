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

        HPChange,
        HPMaxChange,
        MPChange,
        MPMaxChange,
    }

    /**
     *   Class from which all additional effects (not only visual) 
     *   are instatiated (which are handled by the serverscript).
     */
    class EffectChangesDef
    {

        //public static readonly Dictionary<EffectChangesEnum, >
        public static void ApplyEffectChanges (ItemDef def, EffectChangesEnum changeType, string param)
        {
            try
            {
                switch (changeType)
                {
                    //case (EffectChangesEnum.IsGold):
                    //    def.IsGold = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsGold):
                        def.setIsGold(Convert.ToBoolean(param));
                        return;
                    //case (EffectChangesEnum.IsKeyInstance):
                    //    def.IsKeyInstance = Convert.ToBoolean(param);
                    //    return;
                    //case (EffectChangesEnum.IsTorch):
                    //    def.IsTorch = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsTorch):
                        def.setIsTorch(Convert.ToBoolean(param));
                        return;
                    //case (EffectChangesEnum.IsTorchBurning):
                    //    def.IsTorchBurning = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsTorchBurning):
                        def.setIsTorchBurning(Convert.ToBoolean(param));
                        return;
                    //case (EffectChangesEnum.IsTorchBurned):
                    //    def.IsTorchBurned = Convert.ToBoolean(param);
                    //    return;
                    case (EffectChangesEnum.IsTorchBurned):
                        def.setIsTorchBurned(Convert.ToBoolean(param));
                        return;

                    //case (EffectChangesEnum.Effect):
                    //    def.Effect = param;
                    //    return;
                    case (EffectChangesEnum.Effect):
                        def.setEffect(param);
                        return;
                    case (EffectChangesEnum.Spell):
                        // !!! TO DO: requires SpellDef --> resulting SpellInst !!!
                        return;

                    //case (EffectChangesEnum.Wear):
                    //    def.Wear = (Enumeration.ArmorFlags) Convert.ToInt32(param);
                    //    return;
                    case (EffectChangesEnum.Wear):
                        def.setWear((Enumeration.ArmorFlags) Convert.ToInt32(param));
                        return;
                    //case (EffectChangesEnum.DamageType):
                    //    def.DamageType = (Enumeration.DamageType) Convert.ToInt32(param);
                    //    return;
                    case (EffectChangesEnum.DamageType):
                        def.setDamageType((Enumeration.DamageType) Convert.ToInt32(param));
                        return;
                    //case (EffectChangesEnum.Range):
                    //    def.Range = Convert.ToInt32(param);
                    //    return;
                    case (EffectChangesEnum.Range):
                        def.setRange(Convert.ToInt32(param));
                        return;
                    //case (EffectChangesEnum.TotalDamage):
                    //    def.TotalDamage = Convert.ToInt32(param);
                    //    return;
                    case (EffectChangesEnum.TotalDamage):
                        def.setTotalDamage(Convert.ToInt32(param));
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
                            Enumeration.DamageTypeIndex dti = 0;
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
                                    dti = (Enumeration.DamageTypeIndex) Convert.ToInt32(data[i]);
                                    val = Convert.ToInt32(data[i + 1]);
                                    def.setProtection(dti, val);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Couldn't convert part of param-string to int or enum-entry DamageTypeIndex: " + ex);
                                }
                                
                                i += 2;
                            }
                        }
                        return;

                    case (EffectChangesEnum.HPChange):
                        def.setHPChange(Convert.ToInt32(param));
                        return;
                    case (EffectChangesEnum.HPMaxChange):
                        def.setHPMaxChange(Convert.ToInt32(param));
                        return;
                    case (EffectChangesEnum.MPChange):
                        def.setMPChange(Convert.ToInt32(param));
                        return;
                    case (EffectChangesEnum.MPMaxChange):
                        def.setMPMaxChange(Convert.ToInt32(param));
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
