using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Types;
using RakNet;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.Server.WorldObjects
{
    public abstract class AbstractVob
    {
        static uint idCount = 1; // Start with 1 cause a "null-vob" (id = 0) is needed for networking

        public uint ID { get; internal set; }

        internal WorldCell cell;
        internal Client ClientOrNull { get { return (this is NPC && ((NPC)this).isPlayer) ? ((NPC)this).client : null; } }
        public World World { get; internal set; }
        public bool Spawned { get; private set; }

        #region Position
        internal Vec3f pos = new Vec3f(0, 0, 0);
        internal Vec3f dir = new Vec3f(0, 0, 1);

        public float[] Posf { get { return pos.Data; } set { Position = (Vec3f)value; } }
        public float[] Dirf { get { return dir.Data; } set { Direction = (Vec3f)value; } }

        public Vec3f Position
        {
            get { return pos; }
            set
            {
                if (value != null)
                {
                    pos = value;
                }
                else
                {
                    pos = new Vec3f();
                }
                if (Spawned)
                    World.UpdatePosition(this, null);
            }
        }

        public Vec3f Direction
        {
            get { return dir; }
            set
            {
                if (value != null && !value.isNull())
                {
                    dir = value;
                }
                else
                {
                    dir = new Vec3f(0, 0, 1);
                }
                if (Spawned && cell != null)
                    Network.Messages.VobMessage.WritePosDir(cell.SurroundingClients(), null);
            }
        }
        #endregion

        #region Collision
        protected bool cddyn = true;
        protected bool cdstatic = true;

        public bool CDDyn
        {
            get { return cddyn; }
            set
            {
                cddyn = value;
                //network update
            }
        }
        public bool CDStatic
        {
            get { return cdstatic; }
            set
            {
                cdstatic = value;
                //network update
            }
        }
        #endregion

        protected AbstractVob()
        {
            ID = idCount++;
            Spawned = false;
            sWorld.AddVob(this);
        }

        #region Spawn
        public void Spawn(World world)
        {
            Spawn(world, Position, Direction);
        }

        public void Spawn(World world, Vec3f position)
        {
            Spawn(world, position, Direction);
        }

        public virtual void Spawn(World world, Vec3f position, Vec3f direction)
        {
            pos = position;
            dir = direction;
            world.SpawnVob(this);
            Spawned = true;
        }

        public virtual void Despawn()
        {
            if (Spawned)
            {
                World.DespawnVob(this);
                Spawned = false;
            }
        }
        #endregion

        public virtual void RemoveFromServer()
        {
            Despawn();
            sWorld.RemoveVob(this);
        }

        internal abstract void WriteSpawn(IEnumerable<Client> list);

        internal void WriteDespawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldVobDeleteMessage);
            stream.mWrite(ID);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #region Cells
        internal void ChangeCells()
        {

        }


        #endregion
    }
}
