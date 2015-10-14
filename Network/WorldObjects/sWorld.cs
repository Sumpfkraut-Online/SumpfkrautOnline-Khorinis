using System;
using System.Collections.Generic;
using System.Text;
using GUC.WorldObjects.Character;

namespace GUC.WorldObjects
{
    internal partial class sWorld
    {
        protected static Dictionary<String, World> worldDict = new Dictionary<string, World>();
        protected static Dictionary<int, Vob> vobDict = new Dictionary<int, Vob>();

        protected static List<NPC> npcList = new List<NPC>();
        protected static List<NPC> playerList = new List<NPC>();
        protected static List<Item> itemList = new List<Item>();
        protected static List<Vob> vobList = new List<Vob>(); 

        public static Dictionary<String, World> WorldDict { get { return worldDict; } }
        public static Dictionary<int, Vob> VobDict { get { return vobDict; } }

        public static List<NPC> NpcList { get { return npcList; } }
        public static List<NPC> PlayerList { get { return playerList; } }
        public static List<Item> ItemList { get { return itemList; } }
        public static List<Vob> VobList { get { return vobList; } }//Only real Vobs! No Items, No NPCS!


        public static int Day = 1;
        public static byte Hour = 12;
        public static byte Minute = 0;

        public static byte WeatherType;
        public static byte StartRainHour;
        public static byte StartRainMinute;
        public static byte EndRainHour;
        public static byte EndRainMinute;

        public static World getWorld(String world)
        {
            world = getMapName(world);
            if (!worldDict.ContainsKey(world))
            {
                World w = new World();
                w.Map = world;
                worldDict.Add(world, w);
                return w;
            }
            return worldDict[world];
        }

        public static String getMapName(String name){
            name = name.Replace("/", "\\");
            name = name.ToUpper();

            return name;
        }
    }
}
