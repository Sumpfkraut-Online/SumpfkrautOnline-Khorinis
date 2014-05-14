using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Potions.Health
{
    public class ITPO_MEGADRINK : ItemInstance
    {
        static ITPO_MEGADRINK ii;
        public static ITPO_MEGADRINK get()
        {
            if (ii == null)
                ii = new ITPO_MEGADRINK();
            return ii;
        }

        protected ITPO_MEGADRINK()
            : base("ITPO_MEGADRINK")
        {
            Name = "Embarla Firgasto";
            Value = 25;
            Visual = "ItPo_Perm_Mana.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialTypes.MAT_GLAS;
            Description = Name;

            Effect = "SPELLFX_ITEMGLIMMER";

            Wear = ArmorFlags.WEAR_EFFECT;
            ScemeName = "POTIONFAST";

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == 0 && targetState == -1))
                return;

            if (npc.Strength < npc.Dexterity)
                npc.Dexterity += 15;
            else
                npc.Strength += 15;

            npc.MP = 0;
        }
    }
}
