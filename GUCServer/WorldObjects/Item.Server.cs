using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class Item : Vob
    {
        #region Network Messages

        new internal static class Messages
        {
            public static void WriteChangeItemAmount(GameClient client, Item item)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.PlayerItemAmountChangedMessage);
                stream.Write((byte)item.ID);
                stream.Write((ushort)item.amount);
                client.Send(stream, PktPriority.Low, PktReliability.ReliableOrdered, 'I');
            }
        }

        #endregion

        partial void pSetAmount(int amount)
        {
            if (this.Container != null && this.Container is NPC)
            {
                NPC owner = (NPC)this.Container;
                if (owner.IsPlayer)
                {
                    Messages.WriteChangeItemAmount(owner.client, this);
                }
            }
        }
    }
}
