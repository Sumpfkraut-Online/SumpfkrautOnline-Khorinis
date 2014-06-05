using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Enumeration
{
    public enum Guilds
    {
        HUM_NONE = 0,
        HUM_MIL = HUM_NONE + 1,
        HUM_SPERATOR = HUM_MIL + 1,

        MON_NONE = HUM_SPERATOR + 1,
        MON_SHEEP = MON_NONE + 1,
        MON_MEATBUG = MON_SHEEP + 1,
        MON_WOLF = MON_MEATBUG + 1,
        MON_SCAVANGER = MON_WOLF + 1,
        MON_MOLERAT = MON_SCAVANGER + 1,
        MON_BLOODFLY = MON_MOLERAT + 1,
        MON_LURKER = MON_BLOODFLY + 1,
        MON_MINECRAWLER = MON_LURKER + 1,
        MON_ALLIGATOR = MON_MINECRAWLER + 1,
        MON_WARAN = MON_ALLIGATOR + 1,
        MON_GOBBO = MON_WARAN + 1,
        MON_BLATTCRAWLER = MON_GOBBO + 1,
        MON_SHADOWBEAST = MON_BLATTCRAWLER + 1,
        MON_GIANT_RAT = MON_SHADOWBEAST + 1,
        MON_SNAPPER = MON_GIANT_RAT + 1,
        MON_TROLL = MON_SNAPPER + 1,
        MON_STONEGOLEM = MON_TROLL + 1,
        MON_STONEGUARDIAN = MON_STONEGOLEM + 1,
        MON_GIANT_BUG = MON_STONEGUARDIAN + 1,
        MON_SKELETON = MON_GIANT_BUG + 1,
        MON_ZOMBIE = MON_SKELETON + 1,
        MON_SEPERATOR = MON_ZOMBIE + 1,

        

        ORC_NONE = MON_SEPERATOR + 1,
        ORC_SEPERATOR = ORC_NONE + 1
    }
}
