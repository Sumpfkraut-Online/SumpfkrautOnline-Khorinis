using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Scripting;
using GUC.Enumeration;
using RakNet;

namespace GUC.Network.Messages
{
    static class InventoryMessage
    {
        public static void ReadAddItem(PacketReader stream)
        {
            Item item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
            item.ID = stream.ReadByte();
            item.ReadInventoryProperties(stream);
            GameClient.Client.Character.ScriptObject.AddItem(item);
        }
        
        public static void ReadChangeItemAmount(PacketReader stream)
        {
            Item item;
            if (GameClient.Client.Character.Inventory.TryGetItem(stream.ReadByte(), out item))
            {
                item.ScriptObject.SetAmount(stream.ReadUShort());
            }
        }

        public static void ReadRemoveItem(PacketReader stream)
        {
            Item item;
            if (GameClient.Client.Character.Inventory.TryGetItem(stream.ReadByte(), out item))
            {
                GameClient.Client.Character.ScriptObject.RemoveItem(item);
            }
        }

        #region Equipment

        public static void WriteEquipMessage(int slot, Item item)
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.InventoryEquipMessage);
            stream.Write((byte)item.ID);
            stream.Write((byte)slot);
            GameClient.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void WriteUnequipMessage(Item item)
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.InventoryUnequipMessage);
            stream.Write((byte)item.ID);
            GameClient.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void ReadEquipMessage(PacketReader stream)
        {
            Item item;
            if (GameClient.Client.Character.Inventory.TryGetItem(stream.ReadByte(), out item))
            {
                GameClient.Client.Character.ScriptObject.EquipItem(stream.ReadByte(), item);
            }
        }

        public static void ReadUnequipMessage(PacketReader stream)
        {
            Item item;
            if (GameClient.Client.Character.Inventory.TryGetItem(stream.ReadByte(), out item))
            {
                GameClient.Client.Character.ScriptObject.UnequipItem(item);
            }
        }
        #endregion
    }
}
