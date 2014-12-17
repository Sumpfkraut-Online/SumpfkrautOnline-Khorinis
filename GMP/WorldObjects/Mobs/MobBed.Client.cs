using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.WorldObjects.Character;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobBed
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

            oCMobBed gVob = oCMobBed.Create(process);
            gVob.VobType = zCVob.VobTypes.MobBed;
            
            this.Address = gVob.Address;
            sWorld.SpawnedVobDict.Add(this.Address, this);

            setVobData(process, gVob);
            setMobInterData(process, gVob);

            oCGame.Game(process).World.AddVob(gVob);

            triggerMobInter(process, gVob);


        }
    }
}
