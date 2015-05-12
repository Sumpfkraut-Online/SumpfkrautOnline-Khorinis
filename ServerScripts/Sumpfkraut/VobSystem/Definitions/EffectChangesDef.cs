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

        private static Object dictLock = new Object();

        private static Dictionary<int, List<object>> EffectChangesDefDict = new Dictionary<int, List<object>>();

        delegate bool Del_ApplyToDummy(ref DummyItemDef def, string param);

        private static Dictionary<EffectChangesEnum, Del_ApplyToDummy> ApplyToDummyDict_Item = 
            new Dictionary<EffectChangesEnum, Del_ApplyToDummy>()
        {
            {
                EffectChangesEnum.Spell, delegate(ref DummyItemDef def, string param)
                {
                    // !!! TO DO !!!
                    return false;
                }
            },
            {
                EffectChangesEnum.Description, delegate(ref DummyItemDef def, string param)
                {
                    def.setDescription(param);
                    return true;
                }
            },
            {
                EffectChangesEnum.Text0, delegate(ref DummyItemDef def, string param)
                {
                    def.setText0(param);
                    return true;
                }
            },
            {
                EffectChangesEnum.Text1, delegate(ref DummyItemDef def, string param)
                {
                    def.setText1(param);
                    return true;
                }
            },
            {
                EffectChangesEnum.Text2, delegate(ref DummyItemDef def, string param)
                {
                    def.setText2(param);
                    return true;
                }
            },
            {
                EffectChangesEnum.Text3, delegate(ref DummyItemDef def, string param)
                {
                    def.setText3(param);
                    return true;
                }
            },
            {
                EffectChangesEnum.Text4, delegate(ref DummyItemDef def, string param)
                {
                    def.setText4(param);
                    return true;
                }
            },
            {
                EffectChangesEnum.Text5, delegate(ref DummyItemDef def, string param)
                {
                    def.setText5(param);
                    return true;
                }
            },
            {
                EffectChangesEnum.Count0, delegate(ref DummyItemDef def, string param)
                {
                    int Count0 = -1;
                    if (int.TryParse(param, out Count0))
                    {
                        def.setCount0(Count0);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.Count1, delegate(ref DummyItemDef def, string param)
                {
                    int Cound1 = -1;
                    if (int.TryParse(param, out Cound1))
                    {
                        def.setCount1(Cound1);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.Count2, delegate(ref DummyItemDef def, string param)
                {
                    int Count2 = -1;
                    if (int.TryParse(param, out Count2))
                    {
                        def.setCount2(Count2);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.Count3, delegate(ref DummyItemDef def, string param)
                {
                    int Count3 = -1;
                    if (int.TryParse(param, out Count3))
                    {
                        def.setCount3(Count3);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.Count4, delegate(ref DummyItemDef def, string param)
                {
                    int Count4 = -1;
                    if (int.TryParse(param, out Count4))
                    {
                        def.setCount4(Count4);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.Count5, delegate(ref DummyItemDef def, string param)
                {
                    int Count5 = -1;
                    if (int.TryParse(param, out Count5))
                    {
                        def.setCount5(Count5);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            
            // OnUse

            {
                EffectChangesEnum.OnUse_HPChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUse_addHPChange = 0;
                    if (Int32.TryParse(param, out OnUse_addHPChange))
                    {
                        def.setOnUse_HPChange(def.getOnUse_HPChange() + OnUse_addHPChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnUse_HPMaxChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUse_addHPMaxChange = 0;
                    if (Int32.TryParse(param, out OnUse_addHPMaxChange))
                    {
                        def.setOnUse_HPMaxChange(def.getOnUse_HPMaxChange() + OnUse_addHPMaxChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnUse_MPChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUse_addMPChange = 0;
                    if (Int32.TryParse(param, out OnUse_addMPChange))
                    {
                        def.setOnUse_MPChange(def.getOnUse_MPChange() + OnUse_addMPChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnUse_MPMaxChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUse_addMPMaxChange = 0;
                    if (Int32.TryParse(param, out OnUse_addMPMaxChange))
                    {
                        def.setOnUse_MPMaxChange(def.getOnUse_MPMaxChange() + OnUse_addMPMaxChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            
            // OnEquip

            {
                EffectChangesEnum.OnEquip_HPChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnEquip_addHPChange = 0;
                    if (Int32.TryParse(param, out OnEquip_addHPChange))
                    {
                        def.setOnEquip_HPChange(def.getOnEquip_HPChange() + OnEquip_addHPChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnEquip_HPMaxChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnEquip_addHPMaxChange = 0;
                    if (Int32.TryParse(param, out OnEquip_addHPMaxChange))
                    {
                        def.setOnEquip_HPMaxChange(def.getOnEquip_HPMaxChange() + OnEquip_addHPMaxChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnEquip_MPChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnEquip_addMPChange = 0;
                    if (Int32.TryParse(param, out OnEquip_addMPChange))
                    {
                        def.setOnEquip_MPChange(def.getOnEquip_MPChange() + OnEquip_addMPChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnEquip_MPMaxChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnEquip_addMPMaxChange = 0;
                    if (Int32.TryParse(param, out OnEquip_addMPMaxChange))
                    {
                        def.setOnEquip_MPMaxChange(def.getOnEquip_MPMaxChange() + OnEquip_addMPMaxChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },

            // OnUnEquip

            {
                EffectChangesEnum.OnUnEquip_HPChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUnEquip_addHPChange = 0;
                    if (Int32.TryParse(param, out OnUnEquip_addHPChange))
                    {
                        def.setOnUnEquip_HPChange(def.getOnUnEquip_HPChange() + OnUnEquip_addHPChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnUnEquip_HPMaxChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUnEquip_addHPMaxChange = 0;
                    if (Int32.TryParse(param, out OnUnEquip_addHPMaxChange))
                    {
                        def.setOnUnEquip_HPMaxChange(def.getOnUnEquip_HPMaxChange() + OnUnEquip_addHPMaxChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnUnEquip_MPChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUnEquip_addMPChange = 0;
                    if (Int32.TryParse(param, out OnUnEquip_addMPChange))
                    {
                        def.setOnUnEquip_MPChange(def.getOnUnEquip_MPChange() + OnUnEquip_addMPChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
            {
                EffectChangesEnum.OnUnEquip_MPMaxChange, delegate(ref DummyItemDef def, string param)
                {
                    int OnUnEquip_addMPMaxChange = 0;
                    if (Int32.TryParse(param, out OnUnEquip_addMPMaxChange))
                    {
                        def.setOnUnEquip_MPMaxChange(def.getOnUnEquip_MPMaxChange() + OnUnEquip_addMPMaxChange);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            },
        };

        

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
                Del_ApplyToDummy applyFunc;
                if (ApplyToDummyDict_Item.TryGetValue(changeType, out applyFunc))
                {
                    applyFunc(ref def, param);
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
