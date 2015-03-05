using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Potions.Mana
{
    public class ITPO_MANA_01 : ItemInstance
    {
        static ITPO_MANA_01 ii;
        public static ITPO_MANA_01 get()
        {
            if (ii == null)
                ii = new ITPO_MANA_01();
            return ii;
        }

        protected ITPO_MANA_01()
            : base("ITPO_MANA_01")
        {
            Name = "Trank";
            Value = 25;
            Visual = "ItPo_Mana_01.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialTypes.MAT_GLAS;
            Description = "Mana Essenz";

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
            
            npc.HP += 50;
            
        }
    }
}
