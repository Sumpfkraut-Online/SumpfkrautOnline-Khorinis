namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    
    public enum DefChangeType
    {
        undefined = 0,
    }

    public enum EffectChangesEnum
    {
        // 1000 - 1999 reserved for ItemDef effect changes
        Description                 = 1000,
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

    public enum MobInterType
    {
        None            = 0,
        MobInter        = None + 1,
        MobBed          = MobInter + 1,
        MobSwitch       = MobBed + 1,
        MobDoor         = MobSwitch + 1,
        MobContainer    = MobDoor + 1,
    }

    public enum VobDefType
    {
        VobDef  = 0,
        MobDef  = VobDef + 1,
        ItemDef = MobDef + 1,
        NpcDef  = ItemDef + 1,
    }

}