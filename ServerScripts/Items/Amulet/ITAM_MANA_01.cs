using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_MANA_01 : AbstractAmulet
    {
        static ITAM_MANA_01 ii;
        public static ITAM_MANA_01 get()
        {
            if (ii == null)
                ii = new ITAM_MANA_01();
            return ii;
        }


        protected ITAM_MANA_01()
            : base("ITAM_MANA_01")
        {
            Visual = "ItAm_Mana_01.3ds";
            Description = "Amulett der Magie";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.MPMax += 40;
            npc.MP += 40;
        }

        protected void unequip(NPC npc, Item item)
        {

            if (npc.MP > 40)
                npc.MP -= 40;
            else
                npc.MP = 0;
            npc.MPMax -= 40;
        }
    }
}
