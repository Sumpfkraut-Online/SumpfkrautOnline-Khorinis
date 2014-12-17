using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Potions.Health
{
    public class ITPO_PERM_MANA : ItemInstance
    {
        static ITPO_PERM_MANA ii;
        public static ITPO_PERM_MANA get()
        {
            if (ii == null)
                ii = new ITPO_PERM_MANA();
            return ii;
        }

        protected ITPO_PERM_MANA()
            : base("ITPO_PERM_MANA")
        {
            Name = "Trank";
            Value = 25;
            Visual = "ItPo_Perm_Mana.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialTypes.MAT_GLAS;
            Description = "Elixier des Geistes";

            Effect = "SPELLFX_MANAPOTION";

            Wear = ArmorFlags.WEAR_EFFECT;
            ScemeName = "POTIONFAST";

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.MPMax += 5;
            npc.MP += 5;
            
        }
    }
}
