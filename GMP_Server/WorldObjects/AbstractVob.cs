using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Types;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Server.Network.Messages;

namespace GUC.Server.WorldObjects
{
    public class Vob : ServerObject
    {
        static uint idCount = 1; // Start with 1 cause a "null-vob" (id = 0) is needed for networking

        public uint ID { get; protected set; }
        public VobType Type { get; protected set; }
        public VobInstance Instance { get; protected set; }

        public string Visual { get { return Instance.Visual; } }
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }
    
        public World World { get; internal set; }
        public bool IsSpawned { get { return World != null; } }

        internal WorldCell cell;

        #region Position
        internal Vec3f pos = new Vec3f(0, 0, 0);
        internal Vec3f dir = new Vec3f(0, 0, 1);

        public virtual Vec3f Position
        {
            get { return this.pos; }
            set
            {
                this.pos = value;
                if (this.IsSpawned)
                {
                    this.World.UpdatePosition(this, null);
                }
            }
        }

        public virtual Vec3f Direction
        {
            get { return this.dir; }
            set
            {
                if (value.IsNull())
                {
                    this.dir = new Vec3f(0, 0, 1);
                }
                else
                {
                    this.dir = value;
                }
                if (this.IsSpawned)
                {
                    VobMessage.WritePosDir(this.cell.SurroundingClients(), null);
                }
            }
        }
        #endregion

        public Vob(VobInstance instance, object scriptObject) : base(scriptObject)
        {
            this.ID = Vob.idCount++;
            this.Type = VobType.Vob;
            this.Instance = instance;
            Network.Server.sAllVobsDict.Add(this.ID, this);
        }

        /// <summary> Despawns and removes the vob from the server. </summary>
        public override void Delete()
        {
            this.Despawn();
            Network.Server.sAllVobsDict.Remove(ID);
        }

        #region Spawn
        public void Spawn(World world)
        {
            Spawn(world, this.pos, this.dir);
        }

        public void Spawn(World world, Vec3f position)
        {
            Spawn(world, position, this.dir);
        }

        public virtual void Spawn(World world, Vec3f position, Vec3f direction)
        {
            this.pos = position;
            this.dir = direction;
            world.SpawnVob(this);
        }

        public virtual void Despawn()
        {
            if (this.IsSpawned)
            {
                this.World.DespawnVob(this);
            }
        }
        #endregion

        internal virtual void WriteSpawn(IEnumerable<Client> list)
        {

        }

        internal void WriteDespawn(IEnumerable<Client> list)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.WorldVobDeleteMessage);
            stream.Write(this.ID);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }
    }
}
