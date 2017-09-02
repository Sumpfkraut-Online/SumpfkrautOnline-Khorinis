using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects;
using GUC.Network;

namespace GUC.Models
{
    public partial class ModelInstance : IDObject
    {
        partial void pAfterCreate()
        {
            if (!this.IsStatic)
            {
                var stream = GameServer.SetupStream(ServerMessages.ModelInstanceCreateMessage);

                this.WriteStream(stream);

                GameClient.ForEach(c => c.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, '\0'));
            }
        }

        partial void pBeforeDelete()
        {
            if (!this.IsStatic)
            {
                var stream = GameServer.SetupStream(ServerMessages.ModelInstanceDeleteMessage);
                stream.Write((ushort)this.ID);
                GameClient.ForEach(c => c.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, '\0'));
            }
        }
    }
}
