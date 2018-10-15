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
using GUC.Models;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.Mobs.Instances
{
    public class MobInst : VobInst, Mob.IScriptMob
    {

        GUC.Models.ModelInstance s;

        public MobInst()
        {

        }

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

        protected override BaseVob CreateVob()
        {
            return new Mob(new ModelInst(this),this);
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
            stream.Write("cool");
        }
    }
}
