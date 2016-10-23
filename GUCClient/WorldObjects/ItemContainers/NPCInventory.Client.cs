using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.Types;

namespace GUC.WorldObjects.ItemContainers
{
    internal partial class NPCInventory : ItemInventory
    {
        #region Network Messages

        internal static class Messages
        {
            public static void ReadAddItem(PacketReader stream)
            {
                Item item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
                item.ID = stream.ReadByte();
                item.ReadInventoryProperties(stream);
                PlayerInventory.ScriptObject.AddItem(item);
            }

            public static void ReadRemoveItem(PacketReader stream)
            {
                Item item;
                if (PlayerInventory.TryGetItem(stream.ReadByte(), out item))
                {
                    PlayerInventory.ScriptObject.RemoveItem(item);
                }
            }
        }

        #endregion
    }
}
