using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages
{
    static class InventoryMessage
    {
        //Add an item to the client's inventory
        public static void WriteAddItem(GameClient client, Item item)
        {
            PacketWriter stream = Network.GameServer.SetupStream(NetworkIDs.InventoryAddMessage);
            //item.WriteInventory(stream);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void WriteAmountUpdate(GameClient client, Item item)
        {
            WriteAmountUpdate(client, item, item.Amount);
        }

        public static void WriteAmountUpdate(GameClient client, Item item, int amount)
        {
            PacketWriter stream = Network.GameServer.SetupStream(NetworkIDs.InventoryAmountMessage);
            stream.Write(item.ID);
            stream.Write((ushort)amount);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }
    }
}
