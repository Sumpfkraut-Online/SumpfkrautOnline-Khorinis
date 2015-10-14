using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobDoor
    {
        public override void Spawn(String map, Vec3f position, Vec3f direction)
        {
            this.Map = map;
            this.Position = position;
            this.Direction = direction;

            spawned = true;

            if (this.Address != 0)
                return;

            if (this.Map != Player.Hero.Map)
                return;

            Process process = Process.ThisProcess();

            oCMobDoor gVob = oCMobDoor.Create(process);
            gVob.VobType = zCVob.VobTypes.MobDoor;


            this.Address = gVob.Address;
            sWorld.SpawnedVobDict.Add(this.Address, this);

            setVobData(process, gVob);
            setMobInterData(process, gVob);
            setMobLockableData(process, gVob);

            oCGame.Game(process).World.AddVob(gVob);

            triggerMobInter(process, gVob);


        }
    }
}
