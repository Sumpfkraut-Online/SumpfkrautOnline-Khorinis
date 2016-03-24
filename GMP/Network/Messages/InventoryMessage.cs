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
            item.Amount = stream.mReadUShort();
            item.Condition = stream.mReadUShort();
            item.specialLine = stream.mReadString();

            Player.Inventory.Add(ID, item);
            Menus.GUCMenus.Inventory.UpdateContents();
        }

        public static void ReadAmountUpdate(BitStream stream)
        {
            uint id = stream.mReadUInt();
            ushort amount = stream.mReadUShort();

            Item item;
            Player.Inventory.TryGetValue(id, out item);
            if (item == null) return;

            if (amount > 0)
            {
                item.Amount = amount;
            }
            else
            {
                Player.Inventory.Remove(id);
            }
            Menus.GUCMenus.Inventory.UpdateContents();
        }

        const int DelayBetweenMessages = 5000000; //500ms

        static long nextDropItemTime = 0;
        public static void WriteDropItem(object item, int amount)
        {
            if (item != null && DateTime.UtcNow.Ticks > nextDropItemTime && Player.Hero.HasFreeHands)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.InventoryDropItemMessage);
                stream.mWrite(((Item)item).ID);
                stream.mWrite((ushort)amount);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
                nextDropItemTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        static long nextUseItemTime = 0;
        public static void WriteUseItem(Item item)
        {
            if (item != null && DateTime.UtcNow.Ticks > nextUseItemTime && Player.Hero.HasFreeHands)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.InventoryUseItemMessage);
                stream.mWrite(item.ID);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
                nextUseItemTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }
    }
}
