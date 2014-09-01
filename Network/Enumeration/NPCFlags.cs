using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum NPCChangedFlags : int
    {
        EQUIP_NW = 1 << 0,
        EQUIP_RW = EQUIP_NW << 1,
        EQUIP_ARMOR = EQUIP_RW << 1,

        SLOT1 = EQUIP_ARMOR << 1,
        SLOT2 = SLOT1 << 1,
        SLOT3 = SLOT2 << 1,
        SLOT4 = SLOT3 << 1,
        SLOT5 = SLOT4 << 1,
        SLOT6 = SLOT5 << 1,
        SLOT7 = SLOT6 << 1,
        SLOT8 = SLOT7 << 1,
        SLOT9 = SLOT8 << 1,
        ACTIVE_SPELL = SLOT9 << 1,
        MOBINTERACT = ACTIVE_SPELL << 1,
        VOBFOCUS = MOBINTERACT << 1,
        WeaponMode = VOBFOCUS << 1,
        ENEMYFOCUS = WeaponMode << 1,
        ISDEAD = ENEMYFOCUS << 1,
        ISUNCONSCIOUS = ISDEAD << 1,
        ISSWIMMING = ISUNCONSCIOUS << 1,
        PORTALROOM = ISSWIMMING << 1
    }


    public enum NPCAttribute : byte
    {
        ATR_HITPOINTS				=  0,	// Lebenspunkte
        ATR_HITPOINTS_MAX			=  1,	// Max. Lebenspunkte
        ATR_MANA					=  2,	// Mana Mana
        ATR_MANA_MAX				=  3,	// Mana Max

        ATR_STRENGTH				=  4,	// Stärke
        ATR_DEXTERITY				=  5,	// Geschick
        ATR_REGENERATEHP			=  6,	// Regenerierung von HP alle x sekunden
        ATR_REGENERATEMANA		=  7,   // Regenerierung von Mana alle x sekunden

        ATR_MAX = 8
    }

    public enum NPCHitchance : byte
    {
        H1,
        H2,
        Bow,
        CrossBow
    }

    public enum NPCTalent : byte
    {
        Unknown,
        H1,
        H2,
        Bow,
        CrossBow,
        Picklock,
        Pickpocket,//g1 nicht verwenden?
        Mage,
        Sneak,
        Regenerate,
        Firemaster,
        acrobat,
        pickpocketG2,
        Smith,
        Runes,
        Alchemy,
        TakeAnimalTrophy,
        Foreignlanguage,
        MaxTalents
    }

    public enum FightMode
    {
        None = 0,
        Fist = 1,
        Meele = 2,
        Far = 5,
        Magic = 7,
    }
}
