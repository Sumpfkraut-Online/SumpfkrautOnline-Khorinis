using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    
    public enum SPELL_TARGET_COLLECT : byte
    {
        TARGET_COLLECT_NONE = 0,
        TARGET_COLLECT_CASTER = 1,
        TARGET_COLLECT_FOCUS = 2,
        TARGET_COLLECT_ALL = 3,
        TARGET_COLLECT_FOCUS_FALLBACK_NONE = 4,
        TARGET_COLLECT_FOCUS_FALLBACK_CASTER = 5,
        TARGET_COLLECT_ALL_FALLBACK_NONE = 6,
        TARGET_COLLECT_ALL_FALLBACK_CASTER = 7
    }

    public enum SPELL_TARGET_TYPES : byte
    {
        TARGET_TYPE_ALL = 1,
        TARGET_TYPE_ITEMS = 2,
        TARGET_TYPE_NPCS = 4,
        TARGET_TYPE_ORCS = 8,
        TARGET_TYPE_HUMANS = 16,
        TARGET_TYPE_UNDEAD = 32,
    }

    public enum SPELL_TYPE : byte
    {
        GOOD = 0,
        NEUTRAL = 1,
        BAD = 2
    }
    
}
