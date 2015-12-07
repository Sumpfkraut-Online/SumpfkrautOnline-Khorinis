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
            if (client == null || item == null)
                return;

            PacketWriter stream = Program.server.SetupStream(NetworkID.InventoryAddMessage);
            stream.Write(item.ID);
            stream.Write(item.Instance.ID);
            stream.Write(item.Amount);
            stream.Write(item.Condition);
            stream.Write(item.SpecialLine);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void WriteAmountUpdate(Client client, Item item)
        {
            WriteAmountUpdate(client, item, item.amount);
        }

        public static void WriteAmountUpdate(Client client, Item item, ushort amount)
        {
            if (client == null || item == null)
                return;

            PacketWriter stream = Program.server.SetupStream(NetworkID.InventoryAmountMessage);
            stream.Write(item.ID);
            stream.Write(amount);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void ReadDropItem(BitStream stream, Client client)
        {
            Item item;
            if (Server.sItemDict.TryGetValue(stream.mReadUInt(), out item))
            {
                ushort amount = stream.mReadUShort();
                if (client.Character.HasItem(item))
                {
                    if (item.amount > amount) //split it
                    {
                        item.amount -= amount;
                        InventoryMessage.WriteAmountUpdate(client, item);
                        Item newItem = Item.Copy(item);
                        newItem.amount = amount;
                        newItem.Drop(client.Character);
                    } 
                    else //just throw the item out
                    {
                        WriteAmountUpdate(client, item, 0);
                        item.Drop(client.Character);
                    }
                }
            }
        }
    }
}
