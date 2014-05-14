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
        MON_WOLF = MON_NONE + 1,
        MON_SCAVANGER = MON_WOLF + 1,
        MON_BLOODFLY = MON_SCAVANGER + 1,
        MON_ALLIGATOR = MON_BLOODFLY + 1,
        MON_WARAN = MON_ALLIGATOR + 1,
        MON_GOBBO = MON_WARAN + 1,
        MON_BLATTCRAWLER = MON_GOBBO + 1,
        MON_SHADOWBEAST = MON_BLATTCRAWLER + 1,
        MON_GIANT_RAT = MON_SHADOWBEAST + 1,
        MON_SNAPPER = MON_GIANT_RAT + 1,
        MON_STONEGUARDIAN = MON_SNAPPER + 1,
        MON_SEPERATOR = MON_STONEGUARDIAN + 1,

        

        ORC_NONE = MON_SEPERATOR + 1,
        ORC_SEPERATOR = ORC_NONE + 1
    }
}
