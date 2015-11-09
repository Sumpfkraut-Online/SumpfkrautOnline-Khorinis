using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Client.Network.Messages;

namespace GUC.Client.WorldObjects
{
    static class Player
    {
        public static uint ID;
        public static NPC Hero = null;
        public static List<AbstractVob> VobControlledList = new List<AbstractVob>();

        public static void DoFists()
        {
            if (Player.Hero.WeaponState == NPCWeaponState.None)
            {
                NPCMessage.WriteWeaponState(NPCWeaponState.Fists, false);
            }
            else if (Player.Hero.WeaponState == NPCWeaponState.Fists)
            {
                NPCMessage.WriteWeaponState(NPCWeaponState.None, true);
            }
        }

        #region Attributes & Talents

        #endregion

        public static Dictionary<uint, Item> Inventory = new Dictionary<uint, Item>();
    }
}
