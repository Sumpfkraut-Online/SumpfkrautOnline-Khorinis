using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.WorldObjects
{
    internal partial class Spell
    {
        protected int id = 0; //id starts with 101 so it will not collide with gothic-spells

        public int[] processMana = new int[] { 0 };
        public String FXName = "";
        public String AniName = "";
        public String Name = "";

        public int ID { get { return id; } }


        public float TimePerMana = 500;
        public int DamagePerLevel = 1;
        public GUC.Enumeration.DamageTypes DamageType = GUC.Enumeration.DamageTypes.DAM_MAGIC;
        public SPELL_TYPE SpellType = SPELL_TYPE.BAD;
        public bool CanTurnDuringInvest = true;
        public bool CanChangeTargetDuringInvest = true;
        public bool isMultiEffect = false;
        public SPELL_TARGET_COLLECT TargetCollectionAlgo = SPELL_TARGET_COLLECT.TARGET_COLLECT_FOCUS_FALLBACK_NONE;
        public SPELL_TARGET_TYPES TargetCollectType = SPELL_TARGET_TYPES.TARGET_TYPE_NPCS;
        public int TargetCollectRange = 10000;
        public int TargetCollectAzi = 60;
        public int TargetCollectElev = 60;


        #region Statics

        protected static Dictionary<int, Spell> spellDict = new Dictionary<int, Spell>();
        protected static List<Spell> spellList = new List<Spell>();

        public static Dictionary<int, Spell> SpellDict { get { return spellDict; } }
        public static List<Spell> SpellList { get { return spellList; } }


        public static void addItemInstance(Spell spell)
        {
            spellDict.Add(spell.ID, spell);
            spellList.Add(spell);
        }
        #endregion

        protected SpellParameters getParams()
        {
            SpellParameters parameters = 0;

            if (TimePerMana != 500)
                parameters |= SpellParameters.TimePerMana;
            if (DamagePerLevel != 1)
                parameters |= SpellParameters.DamagePerLevel;
            if (DamageType != GUC.Enumeration.DamageTypes.DAM_MAGIC)
                parameters |= SpellParameters.DamageType;
            if (SpellType != SPELL_TYPE.BAD)
                parameters |= SpellParameters.SpellType;
            if (CanTurnDuringInvest != true)
                parameters |= SpellParameters.CanTurnDuringInvest;
            if (CanChangeTargetDuringInvest != true)
                parameters |= SpellParameters.CanChangeTargetDuringInvest;
            if (isMultiEffect != true)
                parameters |= SpellParameters.isMultiEffect;
            if (TargetCollectionAlgo != SPELL_TARGET_COLLECT.TARGET_COLLECT_FOCUS_FALLBACK_NONE)
                parameters |= SpellParameters.TargetCollectionAlgo;
            if (TargetCollectType != SPELL_TARGET_TYPES.TARGET_TYPE_NPCS)
                parameters |= SpellParameters.TargetCollectType;
            if (TargetCollectRange != 10000)
                parameters |= SpellParameters.TargetCollectRange;
            if (TargetCollectAzi != 60)
                parameters |= SpellParameters.TargetCollectAzi;
            if (TargetCollectElev != 60)
                parameters |= SpellParameters.TargetCollectElev;

            return parameters;
        }

        protected enum SpellParameters : ushort
        {
            TimePerMana = 1 << 0,
            DamagePerLevel = TimePerMana << 1,
            DamageType = DamagePerLevel << 1,
            SpellType = DamageType << 1,
            CanTurnDuringInvest = SpellType << 1,
            CanChangeTargetDuringInvest = CanTurnDuringInvest << 1,
            isMultiEffect = CanChangeTargetDuringInvest << 1,
            TargetCollectionAlgo = isMultiEffect << 1,
            TargetCollectType = TargetCollectionAlgo << 1,
            TargetCollectRange = TargetCollectType << 1,
            TargetCollectAzi = TargetCollectRange << 1,
            TargetCollectElev = TargetCollectAzi << 1,


        }
    }
}
