using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using RakNet;

namespace GUC.Models
{
    public partial class Model : GameObject
    {
        partial void pCreate()
        {
            if (!this.IsStatic)
            {
                var stream = GameServer.SetupStream(NetworkIDs.ModelCreateMessage);
                
                this.WriteStream(stream);

                GameClient.ForEach(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0'));
            }
        }

        partial void pDelete()
        {
            if (!this.IsStatic)
            {
                var stream = GameServer.SetupStream(NetworkIDs.ModelDeleteMessage);
                stream.Write((ushort)this.ID);
                GameClient.ForEach(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0'));
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
