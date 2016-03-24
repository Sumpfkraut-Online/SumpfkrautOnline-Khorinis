using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Client.Network.Messages;
using Gothic.zStruct;
using GUC.Client.Hooks;

namespace GUC.Client.WorldObjects
{
    static class Player
    {
        public static uint ID;
        public static NPC Hero = null;
        public static List<AbstractVob> VobControlledList = new List<AbstractVob>();

        public static Item lastUsedWeapon = null;

        public static void DoFists()
        {
            if (Player.Hero.DrawnItem == null)
            {
                NPCMessage.WriteDrawItem(0/*Item.Fists.Slot*/);
            }
            else if (Player.Hero.DrawnItem == Item.Fists)
            {
                NPCMessage.WriteUndrawItem();
            }
        }

        public static Dictionary<uint, Item> Inventory = new Dictionary<uint, Item>();


    }
}
