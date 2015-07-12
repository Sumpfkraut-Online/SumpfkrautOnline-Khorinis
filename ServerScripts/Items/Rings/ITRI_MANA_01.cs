using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_MANA_01 : AbstractRings
    {
        static ITRI_MANA_01 ii;
        public static ITRI_MANA_01 get()
        {
            if (ii == null)
                ii = new ITRI_MANA_01();
            return ii;
        }


        protected ITRI_MANA_01()
            : base("ITRI_MANA_01")
        {
            Visual = "ItRi_Mana_01.3ds";
            Description = "Ring der Magie";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.MPMax += 5;
            npc.MP += 5;
        }

        protected void unequip(NPC npc, Item item)
        {
            if (npc.MP > 5)
                npc.MP -= 5;
            else
                npc.MP = 0;
            npc.MPMax -= 5;
        }
    }
}
