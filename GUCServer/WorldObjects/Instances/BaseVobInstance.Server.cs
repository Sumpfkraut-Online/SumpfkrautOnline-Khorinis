using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Enumeration;
using RakNet;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseVobInstance
    {
        public override void Update()
        {
            throw new NotImplementedException();
        }

        partial void pCreate()
        {
            if (!this.IsStatic)
            {
                var stream = GameServer.SetupStream(NetworkIDs.InstanceCreateMessage);

                stream.Write((byte)this.VobType);
                this.WriteStream(stream);

                GameClient.ForEach(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0'));
            }
        }

        partial void pDelete()
        {
            if (!this.IsStatic)
            {
                var stream = GameServer.SetupStream(NetworkIDs.InstanceDeleteMessage);

                stream.Write((ushort)this.ID);

                GameClient.ForEach(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0'));
            }
        }
    }
}
