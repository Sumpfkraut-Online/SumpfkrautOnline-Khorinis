using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class EquipItemMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int playerID = 0, itemID = 0;
            bool equip = false;
            stream.Read(out playerID);
            stream.Read(out itemID);
            stream.Read(out equip);

            if (!sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Player-ID was not found: "+playerID+" Equipped: "+equip);
            if (!sWorld.VobDict.ContainsKey(itemID))
                throw new Exception("Item-ID was not found: " + itemID + " Equipped: " + equip + " playerID: "+playerID);


            NPCProto player = (NPCProto)sWorld.VobDict[playerID];
            Item item = (Item)sWorld.VobDict[itemID];

            if (equip)
            {
                if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR))
                    player.Armor = item;
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF))
                    player.Weapon = item;
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF))
                    player.RangeWeapon = item;

                player.EquippedList.Add(item);
                Scripting.Objects.Character.NPCProto.isOnEquip(player.ScriptingNPC, item.ScriptingProto);
            }
            else
            {
                if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR))
                    player.Armor = null;
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF))
                    player.Weapon = null;
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF))
                    player.RangeWeapon = null;

                player.EquippedList.Remove(item);
                Scripting.Objects.Character.NPCProto.isOnUnEquip(player.ScriptingNPC, item.ScriptingProto);
            }


            stream.ResetReadPointer();
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, true);

        }
    }
}
