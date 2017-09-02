using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.ItemContainers
{
    internal partial class NPCInventory
    {
        #region Network Messages

        internal static class Messages
        {
            public static void WritePlayerAddItem(GameClient client, Item item)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.PlayerInvAddItemMessage);
                stream.Write((byte)item.ID);
                item.WriteInventoryProperties(stream);
                client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, 'I');
            }

            public static void WritePlayerRemoveItem(GameClient client, Item item)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.PlayerInvRemoveItemMessage);
                stream.Write((byte)item.ID);
                client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, 'I');
            }
        }

        #endregion

        #region Add & Remove

        partial void pAdd(Item item)
        {
            if (this.Owner.IsPlayer && this.Owner.IsSpawned)
                Messages.WritePlayerAddItem(this.Owner.client, item);
        }
        
        partial void pRemoveBefore(Item item)
        {
            if (this.Owner.IsPlayer && this.Owner.IsSpawned)
                Messages.WritePlayerRemoveItem(this.Owner.client, item);
        }

        #endregion
    }
}
