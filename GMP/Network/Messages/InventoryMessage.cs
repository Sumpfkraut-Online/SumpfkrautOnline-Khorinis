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
            uint id = stream.mReadUInt();
            ItemInstance instance;
            if (ItemInstance.InstanceDict.TryGetValue(id, out instance))
            {
                Player.AddItem(instance, stream.mReadInt());
            }
        }

        public static void ReadRemoveItem(BitStream stream)
        {
            uint id = stream.mReadUInt();
            ItemInstance instance;
            if (ItemInstance.InstanceDict.TryGetValue(id, out instance))
            {
                Player.RemoveItem(instance, stream.mReadInt());
            }
        }

        public static void WriteDropItem(ItemInstance instance, int amount)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.InventoryDropItemMessage);
            stream.mWrite(instance.ID);
            stream.mWrite(amount);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void WriteUseItem(ItemInstance instance)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.InventoryUseItemMessage);
            stream.mWrite(instance.ID);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }
    }
}
