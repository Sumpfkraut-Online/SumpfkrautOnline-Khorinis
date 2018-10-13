using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.Mobs.Definitions;
namespace GUC.Scripts.Sumpfkraut.Mobs.Instances
{
    public class MobInst : VobInst, Mob.IScriptMob
    {
        public MobInst(MobDef mobDef)
            : base(mobDef)
        {
        }

        public override void Spawn(WorldInst world, Vec3f pos, Angles ang)
        {
            base.Spawn(world, pos, ang);
        }

        public override void Despawn()
        {
            base.Despawn();
        }

        public void Throw(Vec3f velocity)
        {
            base.Throw(velocity);
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
