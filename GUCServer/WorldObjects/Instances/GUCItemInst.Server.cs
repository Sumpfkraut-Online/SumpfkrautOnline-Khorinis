using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class GUCItemInst : GUCVobInst
    {
        #region Network Messages

        new internal static class Messages
        {
            public static void WriteChangeItemAmount(GameClient client, GUCItemInst item)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.PlayerItemAmountChangedMessage);
                stream.Write((byte)item.ID);
                stream.Write((ushort)item.amount);
                client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, 'I');
            }
        }

        #endregion

        partial void pSetAmount(int amount)
        {
            if (this.Container != null && this.Container is GUCNPCInst)
            {
                GUCNPCInst owner = (GUCNPCInst)this.Container;
                if (owner.IsPlayer)
                {
                    Messages.WriteChangeItemAmount(owner.client, this);
                }
            }
        }
    }
}
