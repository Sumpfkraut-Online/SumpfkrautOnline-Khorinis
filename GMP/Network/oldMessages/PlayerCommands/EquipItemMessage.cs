using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using GUC.Hooks;
using GUC.Enumeration;

namespace GUC.Network.Messages.PlayerCommands
{
    class EquipItemMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0, itemID = 0;
            bool equip = false;
            stream.Read(out playerID);
            stream.Read(out itemID);
            stream.Read(out equip);

            NPC player = (NPC)sWorld.VobDict[playerID];
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
                if (player.Address != 0)
                {
                    hNpc.blockSendEquip = true;
                    new oCNpc(Process.ThisProcess(), player.Address).Equip(new oCItem(Process.ThisProcess(), item.Address));
                }
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
                if (player.Address != 0)
                {
                    hNpc.blockSendUnEquip = true;
                    new oCNpc(Process.ThisProcess(), player.Address).UnequipItem(new oCItem(Process.ThisProcess(), item.Address));
                }
            }


        }
    }
}
