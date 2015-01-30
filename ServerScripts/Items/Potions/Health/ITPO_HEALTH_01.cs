using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Potions.Health
{
    public class ITPO_HEALTH_01 : ItemInstance
    {
        static ITPO_HEALTH_01 ii;
        public static ITPO_HEALTH_01 get()
        {
            if (ii == null)
                ii = new ITPO_HEALTH_01();
            return ii;
        }

        protected ITPO_HEALTH_01()
            : base("ITPO_HEALTH_01")
        {
            Name = "Trank";
            Value = 25;
            Visual = "ItPo_Health_01.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialType.MAT_GLAS;
            Description = "Essenz der Heilung";

            Effect = "SPELLFX_HEALTHPOTION";

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
