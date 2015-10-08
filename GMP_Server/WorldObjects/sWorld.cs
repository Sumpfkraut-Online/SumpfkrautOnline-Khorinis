using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Server.WorldObjects
{
    internal static class sWorld
    {
        public static List<World> WorldList = new List<World>();
        public static Dictionary<uint, AbstractVob> AllVobs = new Dictionary<uint, AbstractVob>(); //All the vobs!

        public static List<NPC> NPCList = new List<NPC>(); //bots
        public static List<NPC> PlayerList = new List<NPC>(); //players
        public static List<Item> ItemList = new List<Item>(); //items
        public static List<Vob> VobList = new List<Vob>(); //mobsis and simple objects (trees, crates, etc.)

        public static int Day = 1;
        public static byte Hour = 12;
        public static byte Minute = 0;

        public static byte WeatherType;
        public static byte StartRainHour;
        public static byte StartRainMinute;
        public static byte EndRainHour;
        public static byte EndRainMinute;

        public static void AddVob(AbstractVob vob)
        {
            //if (!VobDict.ContainsKey(vob.ID)) //is only added once in the Vob-Constructor anyway
            AllVobs.Add(vob.ID, vob);
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    PlayerList.Add((NPC)vob);
                else
                    NPCList.Add((NPC)vob);
            }
            else if (vob is Item)
            {
                ItemList.Add((Item)vob);
            }
            else if (vob is Vob)
            {
                VobList.Add((Vob)vob);
            }
        }

        public static void RemoveVob(AbstractVob vob)
        {
            AllVobs.Remove(vob.ID);
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    PlayerList.Remove((NPC)vob);
                else
                    NPCList.Remove((NPC)vob);
            }
            else if (vob is Item)
            {
                ItemList.Remove((Item)vob);
            }
            else if (vob is Vob)
            {
                VobList.Remove((Vob)vob);
            }
        }
    }
}
