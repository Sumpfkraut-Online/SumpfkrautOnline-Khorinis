using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Enumeration
{
    /**
    * The *_Seperators are for conveniently checking if an NPC is Human, Monster or Orc. This mirrors the Gothic Scripts. A full list of the gothic 2 guildattitudes may be seen here: http://pastebin.com/pSdR53Zi
    * @see IsHuman()
    * @see IsMonster()
    * @see IsOrc()
    */
    public enum Guilds
    {
        HUM_NONE,
        HUM_MIL,
        HUM_SEPERATOR,

        MON_NONE,
        MON_SHEEP,
        MON_MEATBUG,
        MON_WOLF,
        MON_SCAVENGER,
        MON_MOLERAT,
        MON_BLOODFLY,
        MON_LURKER,
        MON_MINECRAWLER,
        MON_ALLIGATOR,
        MON_WARAN,
        MON_GOBBO,
        MON_BLATTCRAWLER,
        MON_SHADOWBEAST,
        MON_GIANT_RAT,
        MON_SNAPPER,
        MON_TROLL,
        MON_STONEGOLEM,
        MON_STONEGUARDIAN,
        MON_GIANT_BUG,
        MON_SKELETON,
        MON_ZOMBIE,
        MON_KEILER,
        MON_WARG,
        MON_ORCBITER,
        MON_SEPERATOR,        

        ORC_NONE,
        ORC_SEPERATOR
    }
}
