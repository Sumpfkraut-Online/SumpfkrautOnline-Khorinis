using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;

namespace GUC.Client.WorldObjects
{
    static class Player
    {
        public static NPC Hero = null;
        public static List<uint> VobControlledList = new List<uint>();

        public static void DoFists()
        {
            //send
            if (Player.Hero.gNpc.WeaponMode == 0)
            {
                Player.Hero.DrawFists();
            }
            else if (Player.Hero.gNpc.WeaponMode == 1)
            {
                Player.Hero.gNpc.RemoveWeapon1();
            }
        }

        #region Attributes & Talents

        #endregion

        #region Inventory

        public static Dictionary<ItemInstance, int> Inventory = new Dictionary<ItemInstance, int>();
        public static void AddItem(ItemInstance instance, int amount)
        {
            if (!Inventory.ContainsKey(instance))
            {
                Inventory.Add(instance, amount);
            }
            else
            {
                Inventory[instance] += amount;
            }
        }

        public static void RemoveItem(ItemInstance instance, int amount)
        {
            int current;
            if (Inventory.TryGetValue(instance, out current))
            {
                if (current - amount <= 0)
                    Inventory.Remove(instance);
                else
                    Inventory[instance] -= amount;
            }
        }
        #endregion
    }
}
