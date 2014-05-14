using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Potions.Health
{
    public class ITPO_PERM_HEALTH : ItemInstance
    {
        static ITPO_PERM_HEALTH ii;
        public static ITPO_PERM_HEALTH get()
        {
            if (ii == null)
                ii = new ITPO_PERM_HEALTH();
            return ii;
        }

        protected ITPO_PERM_HEALTH()
            : base("ITPO_PERM_HEALTH")
        {
            Name = "Trank";
            Value = 25;
            Visual = "ItPo_Perm_Health.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialTypes.MAT_GLAS;
            Description = "Elixier des Lebens";

            Effect = "SPELLFX_HEALTHPOTION";

            Wear = ArmorFlags.WEAR_EFFECT;
            ScemeName = "POTIONFAST";

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == 0 && targetState == -1))
                return;

            npc.HPMax += 20;
            npc.HP += 20;
            
        }
    }
}
