using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Enumeration
{
    public enum Guilds
    {
        HUM_NONE = 0,
        HUM_SPERATOR = HUM_NONE + 1,

        MON_NONE = HUM_SPERATOR + 1,
        MON_WOLF = MON_NONE + 1,
        MON_SEPERATOR = MON_WOLF + 1,

        ORC_NONE = MON_SEPERATOR + 1,
        ORC_SEPERATOR = ORC_NONE + 1
    }
}
