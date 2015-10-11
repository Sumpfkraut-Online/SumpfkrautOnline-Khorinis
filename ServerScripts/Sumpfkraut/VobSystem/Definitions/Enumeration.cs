namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{


    public enum MobInterType
    {
        None            = 0,
        MobInter        = None + 1,
        MobBed          = MobInter + 1,
        MobSwitch       = MobBed + 1,
        MobDoor         = MobSwitch + 1,
        MobContainer    = MobDoor + 1,
    }

}