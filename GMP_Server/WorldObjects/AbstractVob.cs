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
    public abstract class AbstractVob : AbstractObject
    {
        static uint idCount = 1; // Start with 1 cause a "null-vob" (id = 0) is needed for networking

        public uint ID { get; internal set; }

        internal WorldCell cell;

        public World World { get; internal set; }
        public bool Spawned { get { return World != null; } }

        #region Position
        internal Vec3f pos = new Vec3f(0, 0, 0);
        internal Vec3f dir = new Vec3f(0, 0, 1);

        public virtual Vec3f Position
        {
            get { return pos; }
            set
            {
                pos = value;
                if (Spawned)
                {
                    World.UpdatePosition(this, null);
                }
            }
        }

        public virtual Vec3f Direction
        {
            get { return dir; }
            set
            {
                if (value.IsNull())
                {
                    dir = new Vec3f(0, 0, 1);
                }
                else
                {
                    dir = value;
                }
                if (Spawned)
                {
                    Network.Messages.VobMessage.WritePosDir(cell.SurroundingClients(), null);
                }
            }
        }
        #endregion

        protected AbstractVob(object scriptObject) : base(scriptObject)
        {
            ID = idCount++;
            Network.Server.sAllVobsDict.Add(ID, this);
        }

        #region Spawn
        public void Spawn(World world)
        {
            Spawn(world, Position, Direction);
        }

        public void Spawn(World world, Vec3f position)
        {
            Spawn(world, position, this.Direction);
        }

        public virtual void Spawn(World world, Vec3f position, Vec3f direction)
        {
            pos = position;
            dir = direction;
            world.SpawnVob(this);
        }

        public virtual void Despawn()
        {
            if (Spawned)
            {
                World.DespawnVob(this);
            }
        }
        #endregion

        /// <summary> Despawns and removes the vob from the server. </summary>
        public virtual void Delete()
        {
            Despawn();
            Network.Server.sAllVobsDict.Remove(ID);
        }

        internal abstract void WriteSpawn(IEnumerable<Client> list);

        internal void WriteDespawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldVobDeleteMessage);
            stream.mWrite(ID);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }
    }
}
