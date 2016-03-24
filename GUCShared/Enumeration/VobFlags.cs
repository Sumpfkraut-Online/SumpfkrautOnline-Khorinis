using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum MobType
    {
        Vob,
        Mob,
        MobInter,
        MobFire,
        MobLadder,
        MobSwitch,
        MobWheel,
        MobContainer,
        MobDoor
    }

    public enum VobType
    {
        ERROR = 0,
        Item = 8636420,
        Npc = 8640292,
        Player = 8640293,//Not a real VobType Only for network!
        Mob = 8639700,
        MobFire = 8638876,
        Mover = 8627324,
        MobInter = 8639884,
        MobLockable = 8637628,
        MobContainer = 8637284,
        MobDoor = 8638548,
        VobLight = 8624756,
        Trigger = 8625692,
        TriggerList = 8625812,
        Vob = 8624508,
        Freepoint = 8643636,
        Camera = 8624508,
        TriggerScript = 8643756, //really? dunno
        MobSwitch = 8636988,
        MobBed = 8636692,
        ZoneMusic = 8629644,
        zCCSCamera = 8587500,
        TouchDamage = 8642700,
        MessageFilter = 8627196,
        zCVobSound = 8629484,
        zCVobAnimate = 8626668
        //8624756? (MYLIGHT, LIGHT)
        //8626188 pfx

    }


    public enum MobInterNetwork
    {
        OnTrigger,
        OnUnTrigger,
        StartInteraction,
        StopInteraction,
        StartStateChange,

        PickLock
    }

    
}
