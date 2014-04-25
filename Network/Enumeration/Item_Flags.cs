using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum MainFlags : int
    {
        ITEM_KAT_NONE		= 1 <<  0,  // Sonstiges
        ITEM_KAT_NF		= 1 <<  1,  // Nahkampfwaffen
        ITEM_KAT_FF		= 1 <<  2,  // Fernkampfwaffen
        ITEM_KAT_MUN		= 1 <<  3,  // Munition (MULTI)
        ITEM_KAT_ARMOR	= 1 <<  4,  // Ruestungen
        ITEM_KAT_FOOD		= 1 <<  5,  // Nahrungsmittel (MULTI)
        ITEM_KAT_DOCS		= 1 <<  6,  // Dokumente
        ITEM_KAT_POTIONS	= 1 <<  7,  // Traenke
        ITEM_KAT_LIGHT	= 1 <<  8,  // Lichtquellen
        ITEM_KAT_RUNE		= 1 <<  9,  // Runen/Scrolls
        ITEM_KAT_MAGIC	= 1 << 31,  // Ringe/Amulette/Guertel
        ITEM_KAT_KEYS		= ITEM_KAT_NONE
    }

    public enum Flags : int
    {
        ITEM_DAG			= 1 << 13,  // (OBSOLETE!)
        ITEM_SWD			= 1 << 14,  // Schwert
        ITEM_AXE			= 1 << 15,  // Axt
        ITEM_2HD_SWD		= 1 << 16,  // Zweihaender
        ITEM_2HD_AXE		= 1 << 17,  // Streitaxt
        ITEM_SHIELD		= 1 << 18,  // (OBSOLETE!)
        ITEM_BOW			= 1 << 19,  // Bogen
        ITEM_CROSSBOW		= 1 << 20,  // Armbrust
        // magic type (flags)
        ITEM_RING			= 1 << 11,  // Ring
        ITEM_AMULET		= 1 << 22,  // Amulett
        ITEM_BELT			= 1 << 24,  // Guertel
        // attributes (flags)
        ITEM_DROPPED 		= 1 << 10,  // (INTERNAL!)
        ITEM_MISSION 		= 1 << 12,  // Missionsgegenstand
        ITEM_MULTI		= 1 << 21,  // Stapelbar
        ITEM_NFOCUS		= 1 << 23,  // (INTERNAL!)
        ITEM_CREATEAMMO	= 1 << 25,  // Erzeugt Munition selbst (magisch)
        ITEM_NSPLIT		= 1 << 26,  // Kein Split-Item (Waffe als Interact-Item!)
        ITEM_DRINK		= 1 << 27,  // (OBSOLETE!)
        ITEM_TORCH		= 1 << 28,  // Fackel
        ITEM_THROW		= 1 << 29,  // (OBSOLETE!)
        ITEM_ACTIVE		= 1 << 30  // (INTERNAL!)
    }

    public enum DamageTypeIndex : byte
    {
        DAM_INDEX_BARRIER								= 0						,  		//				 nur der Vollstandigkeit und Transparenz wegen hier definiert ( _NICHT_ verwenden )
        DAM_INDEX_BLUNT								= DAM_INDEX_BARRIER + 1	,
        DAM_INDEX_EDGE								= DAM_INDEX_BLUNT	+ 1	,
        DAM_INDEX_FIRE								= DAM_INDEX_EDGE	+ 1	,
        DAM_INDEX_FLY									= DAM_INDEX_FIRE	+ 1	,
        DAM_INDEX_MAGIC								= DAM_INDEX_FLY		+ 1	,
        DAM_INDEX_POINT								= DAM_INDEX_MAGIC	+ 1	,
        DAM_INDEX_FALL								= DAM_INDEX_POINT	+ 1	,  		//				 nur der Vollstandigkeit und Transparenz wegen hier definiert ( _NICHT_ verwenden )
        DAM_INDEX_MAX									= DAM_INDEX_FALL	+ 1
    }

    static class EnumExtensions
    {

        public static DamageTypeIndex getDamageTypeIndex(this DamageType s1)
        {
            switch (s1)
            {
                case DamageType.DAM_INVALID:
                    return DamageTypeIndex.DAM_INDEX_MAX;
                case DamageType.DAM_BARRIER:
                    return DamageTypeIndex.DAM_INDEX_BARRIER;
                case DamageType.DAM_BLUNT:
                    return DamageTypeIndex.DAM_INDEX_BLUNT;
                case DamageType.DAM_EDGE:
                    return DamageTypeIndex.DAM_INDEX_EDGE;
                case DamageType.DAM_FIRE:
                    return DamageTypeIndex.DAM_INDEX_FIRE;
                case DamageType.DAM_FLY:
                    return DamageTypeIndex.DAM_INDEX_FLY;
                case DamageType.DAM_MAGIC:
                    return DamageTypeIndex.DAM_INDEX_MAGIC;
                case DamageType.DAM_POINT:
                    return DamageTypeIndex.DAM_INDEX_POINT;
                case DamageType.DAM_FALL:
                    return DamageTypeIndex.DAM_INDEX_FALL;
                default:
                    return DamageTypeIndex.DAM_INDEX_BARRIER;;
            }
        }
    }




    public enum DamageType : byte
    {
        DAM_INVALID									= 0,       //	  0 - 0x00 - nur der Vollstandigkeit und Transparenz wegen hier definiert ( _NICHT_ verwenden )
        DAM_BARRIER									= 1,  		//	  1 - 0x01 - nur der Vollstandigkeit und Transparenz wegen hier definiert ( _NICHT_ verwenden )
        DAM_BLUNT										= DAM_BARRIER	<< 1,  		//	  2 - 0x02 - blunt ist das bit links von barrier
        DAM_EDGE										= DAM_BLUNT		<< 1,		//	  4 - 0x04 - edge 	ist das bit links von blunt
        DAM_FIRE										= DAM_EDGE		<< 1,  		//	  8 - 0x08 - fire 	ist das bit links von edge
        DAM_FLY										= DAM_FIRE		<< 1,		//	 16 - 0x10 - fly 	ist das bit links von fire
        DAM_MAGIC										= DAM_FLY		<< 1,  		//	 32 - 0x20 - magic	ist das bit links von fly
        DAM_POINT										= DAM_MAGIC		<< 1,  		//	 64 - 0x40 - point	ist das bit links von magic
        DAM_FALL										= DAM_POINT		<< 1 		//	128 - 0x80 - nur der Vollstandigkeit und Transparenz wegen hier definiert ( _NICHT_ verwenden )

    }

    public enum ArmorFlags : byte
    {
        WEAR_TORSO									=  1,	//	Oberkoerper	( Brustpanzer )
        WEAR_HEAD										=  2,		//	Kopf		( Helm )
        WEAR_EFFECT									=  16	
    }

    public enum MaterialTypes : byte
    {
        MAT_WOOD										= 0,
        MAT_STONE										= 1,
        MAT_METAL										= 2,
        MAT_LEATHER									= 3,
        MAT_CLAY										= 4,
        MAT_GLAS										= 5		// ??
    }
}
