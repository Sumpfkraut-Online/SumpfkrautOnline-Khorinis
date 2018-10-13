using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects;
using GUC.WorldObjects.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Mobs.Instances
{
    public class MobInst : VobInst, Mob.IScriptMob
    {
        public MobInst()
        {
        }

        public new void Spawn(WorldSystem.WorldInst world)
        {
            base.Spawn(world);
        }

        public override void Despawn()
        {
            base.Despawn();
        }

        public override void OnReadProperties(PacketReader stream)
        {
            base.OnReadProperties(stream);
        }

        public override void OnWriteProperties(PacketWriter stream)
        {
            base.OnWriteProperties(stream);
        }
    }
}
