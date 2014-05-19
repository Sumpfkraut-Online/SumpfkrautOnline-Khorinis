using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Scrolls
{
    public class SPELL_ITSC_WINDFIST : Spell
    {
        static SPELL_ITSC_WINDFIST ii;
        public static SPELL_ITSC_WINDFIST get()
        {
            if (ii == null)
                ii = new SPELL_ITSC_WINDFIST();
            return ii;
        }

        protected SPELL_ITSC_WINDFIST()
            : base()
        {
            TimePerMana = 30;
            DamagePerLevel = 25;
            DamageType = Enumeration.DamageType.DAM_FLY;
            CanTurnDuringInvest = true;
            TargetCollectionAlgo = Enumeration.SPELL_TARGET_COLLECT.TARGET_COLLECT_FOCUS_FALLBACK_NONE;
            TargetCollectType = Enumeration.SPELL_TARGET_TYPE.TARGET_TYPE_NPCS;
            TargetCollectRange = 1000;

            Name = "Windfaust";
            AniName = "WND";
            FXName = "WindFist";

            ChargeMana = new int[] { 25, 25, 25, 25 };
            
            CreateSpell();


            
        }
    }

    public class ITSC_WINDFIST : ItemInstance
    {
        static ITSC_WINDFIST ii;
        public static ITSC_WINDFIST get()
        {
            if (ii == null)
                ii = new ITSC_WINDFIST();
            return ii;
        }


        protected ITSC_WINDFIST()
            : base("ITSC_WINDFIST")
        {
            Name = "Spruchrolle";
            MainFlags = Enumeration.MainFlags.ITEM_KAT_RUNE;
            Flags = Enumeration.Flags.ITEM_MULTI;

            Value = 25;

            Visual = "ItSc_Windfist.3DS";
            Materials = Enumeration.MaterialTypes.MAT_LEATHER;
            Wear = Enumeration.ArmorFlags.WEAR_EFFECT;

            Effect = "SPELLFX_WEAKGLIMMER";
            Description = "Windfaust";
            //SPELL_SHRINK.get();
            Spell = SPELL_ITSC_WINDFIST.get();

            CreateItemInstance();
        }
    }
}
