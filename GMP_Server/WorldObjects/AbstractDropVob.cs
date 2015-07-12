using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Server.Network;

namespace GUC.Server.WorldObjects
{
    public abstract class AbstractDropVob : AbstractCtrlVob
    {
        internal bool physicsEnabled = false;
        internal Vec3f lastPos;

        internal void Update(Vec3f newPos)
        {
            if (lastPos != null && lastPos.getDistance(newPos) == 0)
            {
                VobController.RemoveControlledVob(this);
                World.UpdatePosition(this, VobController); //set the position on all other clients
                physicsEnabled = false;
            }
            else
            {
                lastPos = pos;
            }
        }

        public void Drop(World world)
        {
            Drop(world, Position, Direction);
        }

        public void Drop(World world, Vec3f position)
        {
            Drop(world, position, Direction);
        }

        public void Drop(World world, Vec3f position, Vec3f direction)
        {
            physicsEnabled = true;
            Spawn(world, position, direction);
        }

        const int DropDistance = 60;
        const int Elevation = 20;
        public void Drop(NPC npc) //Drop in front of a NPC
        {
            if (npc.isPlayer)
            {
                npc.client.AddControlledVob(this);
            }

            Drop(npc.World, new Vec3f(npc.pos.X + npc.dir.X*DropDistance, npc.pos.Y + Elevation, npc.pos.Z + npc.dir.Z*DropDistance));
        }
    }
}
