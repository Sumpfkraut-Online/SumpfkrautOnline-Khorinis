using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Potions.Health
{
    public class ITPO_HEALTH_02 : ItemInstance
    {
        static ITPO_HEALTH_02 ii;
        public static ITPO_HEALTH_02 get()
        {
            if (ii == null)
                ii = new ITPO_HEALTH_02();
            return ii;
        }

        protected ITPO_HEALTH_02()
            : base("ITPO_HEALTH_02")
        {
            Name = "Trank";
            Value = 35;
            Visual = "ItPo_Health_02.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialTypes.MAT_GLAS;
            Description = "Extrakt der Heilung";

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
            
            npc.HP += 70;
            
        }
    }
}
