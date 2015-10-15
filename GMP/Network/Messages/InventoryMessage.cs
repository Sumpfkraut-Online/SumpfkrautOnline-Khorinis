using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Client.WorldObjects;
using GUC.Enumeration;

namespace GUC.Client.Network.Messages
{
    static class InventoryMessage
    {
        public static void ReadAddItem(BitStream stream)
        {
            uint ID = stream.mReadUInt();
            ushort instID = stream.mReadUShort();

            Item item = new Item(ID, instID);
            item.amount = stream.mReadUShort();
            item.condition = stream.mReadUShort();

            Player.Inventory.Add(ID, item);
        }

        public static void ReadAmountUpdate(BitStream stream)
        {
            uint id = stream.mReadUInt();
            ushort amount = stream.mReadUShort();

            Player.Inventory[id].amount = amount;
        }

        public static void WriteDropItem(object item, int amount)
        {
            if (item == null)
                return;

            BitStream stream = Program.client.SetupSendStream(NetworkID.InventoryDropItemMessage);
            stream.mWrite(((Item)item).ID);
            stream.mWrite((ushort)amount);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void WriteUseItem(Item item)
        {
            if (item == null)
                return;

            BitStream stream = Program.client.SetupSendStream(NetworkID.InventoryUseItemMessage);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }
    }
}
