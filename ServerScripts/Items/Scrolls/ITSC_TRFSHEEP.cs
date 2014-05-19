using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting;

namespace GUC.Server.Scripts.Items
{
    public class SPELL_TRFSHEEP : Spell
    {
        static SPELL_TRFSHEEP shrink;
        public static SPELL_TRFSHEEP get()
        {
            if (shrink == null)
                shrink = new SPELL_TRFSHEEP();
            return shrink;
        }

        protected SPELL_TRFSHEEP()
            : base()
        {
            TimePerMana = 0;
            SpellType = Enumeration.SPELL_TYPE.NEUTRAL;
            TargetCollectionAlgo = Enumeration.SPELL_TARGET_COLLECT.TARGET_COLLECT_CASTER;
            CanTurnDuringInvest = false;

            Mana = 5;

            Name = "Verwandlung Schaf";
            AniName = "TRF";
            FXName = "Transform";

            this.OnCastSpell += new Events.CastSpell(cast);

            CreateSpell();
        }

        protected void cast(NPCProto caster, Spell spell, Vob target)
        {
            caster.setWeaponMode(1);
            caster.setVisual("Sheep.mds", "Sheep_Body", 0, 0, "", 0, 0);
            //caster.setVisual("Sheep.mds", "Sheep_Body", 0, 0, "", 0, 0);
        }
    }
    public class ITSC_TRFSHEEP : ItemInstance
    {
        static ITSC_TRFSHEEP shrink;
        public static ITSC_TRFSHEEP get()
        {
            if (shrink == null)
                shrink = new ITSC_TRFSHEEP();
            return shrink;
        }


        protected ITSC_TRFSHEEP()
            : base("ITSC_TRFSHEEP")
        {
            Name = "Spruchrolle";
            MainFlags = Enumeration.MainFlags.ITEM_KAT_RUNE;
            Flags = Enumeration.Flags.ITEM_MULTI;

            Value = 25;

            Visual = "ItSc_TrfSheep.3DS";
            Materials = Enumeration.MaterialTypes.MAT_LEATHER;
            Wear = Enumeration.ArmorFlags.WEAR_EFFECT;

            Effect = "SPELLFX_WEAKGLIMMER";
            Description = "Verwandlung Schaf";
            
            Spell = SPELL_TRFSHEEP.get();

            CreateItemInstance();
        }
    }
}
