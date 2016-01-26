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
using GUC.WorldObjects.Collections;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class Vob : WorldObject, IVobObj<uint>
    {
        static uint idCount = 1; // Start with 1 cause a "null-vob" (id = 0) is needed for networking

        internal WorldCell cell;

        #region Position

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

        partial void pCreate()
        {
            this.ID = Vob.idCount++;
        }

        #region Network Messages

        internal virtual void WriteSpawnMessage(IEnumerable<Client> list)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldVobSpawnMessage);
            this.WriteSpawnProperties(stream);

            foreach (Client client in list)
            {
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            }
        }

        internal void WriteDespawnMessage(IEnumerable<Client> list)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldVobDeleteMessage);
            stream.Write(this.ID);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        #endregion
    }
}
