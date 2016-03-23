using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Scripting;
using GUC.Enumeration;

namespace GUC.Client.Network.Messages
{
    static class InventoryMessage
    {
        public static void ReadAddItem(PacketReader stream)
        {
            Item item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
            item.ID = stream.ReadByte();
            item.ReadInventoryProperties(stream);
            GameClient.Client.character.ScriptObject.AddItem(item);
        }

        public static void ReadRemoveItem(PacketReader stream)
        {
            Item item;
            if (GameClient.Client.character.Inventory.TryGetItem(stream.ReadByte(), out item))
            {
                GameClient.Client.character.ScriptObject.RemoveItem(item);
            }
        }
    }
}
