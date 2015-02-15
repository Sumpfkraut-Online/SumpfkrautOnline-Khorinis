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

        HP,
        HPMax,
        MP,
        MPMax,
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
                    case (EffectChangesEnum.IsGold):
                        def.IsGold = Convert.ToBoolean(param);
                        return;
                    case (EffectChangesEnum.IsKeyInstance):
                        def.IsKeyInstance = Convert.ToBoolean(param);
                        return;
                    case (EffectChangesEnum.IsTorch):
                        def.IsTorch = Convert.ToBoolean(param);
                        return;
                    case (EffectChangesEnum.IsTorchBurning):
                        def.IsTorchBurning = Convert.ToBoolean(param);
                        return;
                    case (EffectChangesEnum.IsTorchBurned):
                        def.IsTorchBurned = Convert.ToBoolean(param);
                        return;

                    case (EffectChangesEnum.Effect):
                        def.Effect = param;
                        return;
                    case (EffectChangesEnum.Spell):
                        // !!! TO DO: requires SpellDef --> resulting SpellInst !!!
                        return;

                    case (EffectChangesEnum.Wear):
                        def.Wear = (Enumeration.ArmorFlags) Convert.ToInt32(param);
                        return;
                    case (EffectChangesEnum.DamageType):
                        def.DamageType = (Enumeration.DamageType) Convert.ToInt32(param);
                        return;
                    case (EffectChangesEnum.Range):
                        def.Range = Convert.ToInt32(param);
                        return;
                    case (EffectChangesEnum.TotalDamage):
                        def.TotalDamage = Convert.ToInt32(param);
                        return;
                    case (EffectChangesEnum.Damages):
                        // !!! TO DO: multiple entries !!!
                        // ??? should setting them even possible ???
                        return;
                    case (EffectChangesEnum.Munition):
                        // !!! TO DO: requires ItemDef --> resulting ItemInst !!!
                        return;

                    case (EffectChangesEnum.Protection):
                        // !!! TO DO: multiple entries !!!
                        // !!! use the setProtection method instead of lacking set-accessor !!!
                        // !!! requires int-index as well as value !!!
                        return;

                    case (EffectChangesEnum.HP):
                        def.HPChange = Convert.ToInt32(param);
                        return;
                    case (EffectChangesEnum.HPMax):
                        def.HPMaxChange = Convert.ToInt32(param);
                        return;
                    case (EffectChangesEnum.MP):
                        def.MPChange = Convert.ToInt32(param);
                        return;
                    case (EffectChangesEnum.MPMax):
                        def.MPMaxChange = Convert.ToInt32(param);
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
