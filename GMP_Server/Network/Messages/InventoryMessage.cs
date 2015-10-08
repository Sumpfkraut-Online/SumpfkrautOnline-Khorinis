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
        public static void WriteAddItem(Client client, ItemInstance instance, int amount)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.InventoryAddMessage);
            stream.mWrite(instance.ID);
            stream.mWrite(amount);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', client.guid, false);
        }

        public static void WriteRemoveItem(Client client, ItemInstance instance, int amount)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.InventoryRemoveMessage);
            stream.mWrite(instance.ID);
            stream.mWrite(amount);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', client.guid, false);
        }

        public static void ReadDropItem(BitStream stream, Client client)
        {
            ushort id = stream.mReadUShort();
            int amount = stream.mReadInt();

            if (id < ItemInstance.InstanceList.Count)
            {
                ItemInstance inst = ItemInstance.InstanceList[id];
                if (client.character.HasItem(inst, amount))
                {
                    client.character.RemoveItem(inst, amount);
                    Item item = new Item(inst);
                    item.Amount = amount;

                    item.Drop(client.character);
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
