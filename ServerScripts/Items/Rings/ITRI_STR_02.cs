using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_STR_02 : AbstractRings
    {
        static ITRI_STR_02 ii;
        public static ITRI_STR_02 get()
        {
            if (ii == null)
                ii = new ITRI_STR_02();
            return ii;
        }


        protected ITRI_STR_02()
            : base("ITRI_STR_02")
        {
            Visual = "ItRi_Str_02.3ds";
            Description = "Ring der Stärke";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.Strength += 5;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.Strength -= 5;
        }
    }
}
