using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Scrolls
{
    public class SPELL_ITSC_FIREBOLT : Spell
    {
        static SPELL_ITSC_FIREBOLT ii;
        public static SPELL_ITSC_FIREBOLT get()
        {
            if (ii == null)
                ii = new SPELL_ITSC_FIREBOLT();
            return ii;
        }

        protected SPELL_ITSC_FIREBOLT()
            : base()
        {
            TimePerMana = 0;
            DamagePerLevel = 25;
            DamageType = Enumeration.DamageTypes.DAM_MAGIC;
            

            Name = "Feuerpfeil";
            AniName = "FBT";
            FXName = "Firebolt";

            Mana = 25;

            CreateSpell();



        }
    }

    public class ITSC_FIREBOLT : ItemInstance
    {
        static ITSC_FIREBOLT ii;
        public static ITSC_FIREBOLT get()
        {
            if (ii == null)
                ii = new ITSC_FIREBOLT();
            return ii;
        }


        protected ITSC_FIREBOLT()
            : base("ITSC_FIREBOLT")
        {
            Name = "Spruchrolle";
            MainFlags = Enumeration.MainFlags.ITEM_KAT_RUNE;
            Flags = Enumeration.Flags.ITEM_MULTI;

            Value = 25;

            Visual = "ItSc_Firebolt.3DS";
            Materials = Enumeration.MaterialType.MAT_LEATHER;
            Wear = Enumeration.ArmorFlags.WEAR_EFFECT;

            Effect = "SPELLFX_WEAKGLIMMER";
            Description = "Feuerpfeil";
            //SPELL_SHRINK.get();
            Spell = SPELL_ITSC_FIREBOLT.get();

            CreateItemInstance();
        }
    }
}
