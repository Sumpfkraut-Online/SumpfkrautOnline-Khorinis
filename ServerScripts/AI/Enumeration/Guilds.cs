using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Enumeration
{
    /** Guilds available to the AI
     * See individual documentation for identifiers. 
     * Currently monstertypes are "guilds".
     */
    public enum Guilds
    {
        HUM_NONE = 0, /**< Without a guild*/
        HUM_MIL = HUM_NONE + 1, /**< Militia*/
        HUM_SPERATOR = HUM_MIL + 1, /**< internal Seperator HUM<->MON. DO NOT USE*/

        MON_NONE = HUM_SPERATOR + 1, /**< Monster without any specific guild/type.*/
        MON_SHEEP = MON_NONE + 1, /**< Sheep*/
        MON_MEATBUG = MON_SHEEP + 1, /**< Meatbug*/
        MON_WOLF = MON_MEATBUG + 1, /**< Wolf*/
        MON_SCAVANGER = MON_WOLF + 1, /**< Scavenger*/
        MON_MOLERAT = MON_SCAVANGER + 1, /**< Molerat*/
        MON_BLOODFLY = MON_MOLERAT + 1, /**< Bloodfly*/
        MON_LURKER = MON_BLOODFLY + 1, /**< Lurker*/
        MON_MINECRAWLER = MON_LURKER + 1, /**< Minecrawler*/
        MON_ALLIGATOR = MON_MINECRAWLER + 1, /**< Alligator*/
        MON_WARAN = MON_ALLIGATOR + 1, /**< Waran (eng sp?)*/
        MON_GOBBO = MON_WARAN + 1, /**< Goblin*/
        MON_BLATTCRAWLER = MON_GOBBO + 1, /**< Giant locust (actual name?)*/
        MON_SHADOWBEAST = MON_BLATTCRAWLER + 1, /**< Shadowbeast*/
        MON_GIANT_RAT = MON_SHADOWBEAST + 1, /**< Giant Rat*/
        MON_SNAPPER = MON_GIANT_RAT + 1, /**< Snapper*/
        MON_TROLL = MON_SNAPPER + 1, /**< Troll*/
        MON_STONEGOLEM = MON_TROLL + 1, /**< Stonegolem*/
        MON_STONEGUARDIAN = MON_STONEGOLEM + 1, /**< Stoneguardian*/
        MON_GIANT_BUG = MON_STONEGUARDIAN + 1, /**< Giant Bug (???)*/
        MON_SKELETON = MON_GIANT_BUG + 1, /**< Skeleton*/
        MON_ZOMBIE = MON_SKELETON + 1, /**< Zombie*/
        MON_SEPERATOR = MON_ZOMBIE + 1, /**< internal Seperator MON<->ORC. DO NOT USE*/

        

        ORC_NONE = MON_SEPERATOR + 1, /**< Generic orc*/
        ORC_SEPERATOR = ORC_NONE + 1 /**< internal Seperator ORC<->EOF. DO NOT USE*/
    }
}
