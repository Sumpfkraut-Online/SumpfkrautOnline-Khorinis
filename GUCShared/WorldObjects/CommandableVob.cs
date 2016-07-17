using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects
{
    public abstract partial class CommandableVob : BaseVob
    {
        public abstract class Command
        {
            protected abstract void WriteStream(PacketWriter stream);
            protected abstract void ReadStream(PacketReader stream);
        }

        public abstract class TargetCommand : Command
        {
            BaseVob target;
            public BaseVob Target { get { return this.target; } }

            public TargetCommand(BaseVob target)
            {
                this.target = target;
            }
        }

        internal GameClient Commander;

        partial void pSpawn(World world, Vec3f position, Vec3f direction);
        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);
            pSpawn(world, position, direction);
        }

        partial void pDespawn();
        public override void Despawn()
        {
            base.Despawn();
            pDespawn();
        }
    }
}
