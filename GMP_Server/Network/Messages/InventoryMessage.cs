using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages
{
    static class InventoryMessage
    {
        //Add an item to the client's inventory
        public static void WriteAddItem(Client client, Item item)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkIDs.InventoryAddMessage);
            item.WriteInventory(stream);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void WriteAmountUpdate(Client client, Item item)
        {
            WriteAmountUpdate(client, item, item.Amount);
        }

        public static void WriteAmountUpdate(Client client, Item item, ushort amount)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkIDs.InventoryAmountMessage);
            stream.Write(item.ID);
            stream.Write(amount);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }
    }
}
