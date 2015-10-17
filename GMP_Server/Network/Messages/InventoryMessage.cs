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

            BitStream stream = Program.server.SetupStream(NetworkID.InventoryAddMessage);
            stream.mWrite(item.ID);
            stream.mWrite(item.Instance.ID);
            stream.mWrite(item.Amount);
            stream.mWrite(item.Condition);
            stream.mWrite(item.SpecialLine);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', client.guid, false);
        }

        public static void WriteAmountUpdate(Client client, Item item)
        {
            WriteAmountUpdate(client, item, item.amount);
        }

        public static void WriteAmountUpdate(Client client, Item item, ushort amount)
        {
            if (client == null || item == null)
                return;

            BitStream stream = Program.server.SetupStream(NetworkID.InventoryAmountMessage);
            stream.mWrite(item.ID);
            stream.mWrite(amount);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', client.guid, false);
        }

        public static void ReadDropItem(BitStream stream, Client client)
        {
            Item item;
            if (sWorld.ItemDict.TryGetValue(stream.mReadUInt(), out item))
            {
                ushort amount = stream.mReadUShort();
                if (client.character.HasItem(item))
                {
                    if (item.amount > amount) //split it
                    {
                        item.amount -= amount;
                        InventoryMessage.WriteAmountUpdate(client, item);
                        Item newItem = Item.Copy(item);
                        newItem.amount = amount;
                        newItem.Drop(client.character);
                    } 
                    else //just throw the item out
                    {
                        WriteAmountUpdate(client, item, 0);
                        item.Drop(client.character);
                    }
                }
            }
        }

        public static void ReadUseItem(BitStream stream, Client client)
        {
            /*ushort id = stream.mReadUShort();
            if (id < ItemInstance.InstanceList.Count)
            {
                ItemInstance inst = ItemInstance.InstanceList[id];
                if (client.character.HasItem(inst))
                {
                    inst.Use(client.character);
                }
            }*/
        }
    }
}
