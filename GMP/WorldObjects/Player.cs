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
        public static uint ID;
        public static NPC Hero = null;
        public static List<AbstractVob> VobControlledList = new List<AbstractVob>();

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

        public static Dictionary<uint, Item> Inventory = new Dictionary<uint, Item>();
    }
}
