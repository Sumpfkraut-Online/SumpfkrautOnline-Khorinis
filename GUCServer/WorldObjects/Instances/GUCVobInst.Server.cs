using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class GUCVobInst
    {
        #region Network Messages

        new internal static class Messages
        {
            public static void WriteThrow(GUCVobInst vob, Vec3f velocity)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.VobThrowMessage);
                stream.Write((ushort)vob.ID);
                stream.Write(velocity);
                vob.ForEachVisibleClient(client => client.Send(stream, NetPriority.Medium, NetReliability.ReliableOrdered, 'W'));
            }
        }

        #endregion

        partial void pThrow(Vec3f velocity)
        {
            if (this.IsSpawned)
                Messages.WriteThrow(this, velocity);
        }
    }
}
