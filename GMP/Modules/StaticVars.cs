using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using Network;
using WinApi.FileFormat;

namespace GMP.Modules
{
    public class StaticVars
    {
        public static String StartWorld = null;
        public static String StartWP = "START";
        public static zCView printView;
        public static bool Ingame;
        public static ServerConfig serverConfig;
        public static bool ModuleLoad;

        public static oCNpc STRHELPER;


        public static List<NPC> npcList = new List<NPC>();
        public static List<NPC> npcControlList = new List<NPC>();
        public static List<Player> playerlist = new List<Player>();
        public static Dictionary<int, Player> AllPlayerDict = new Dictionary<int, Player>();
        public static Dictionary<int, Player> PlayerDict = new Dictionary<int, Player>();

        public static List<Player> spawnedPlayerList = new List<Player>();
        public static Dictionary<int, Player> spawnedPlayerDict = new Dictionary<int, Player>();


        public static IniFile Languages;


        public static Dictionary<int, int> sStats = new Dictionary<int, int>();
    }
}
