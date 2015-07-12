using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_ADDON_MANA_01 : AbstractRings
    {
        static ITRI_ADDON_MANA_01 ii;
        public static ITRI_ADDON_MANA_01 get()
        {
            if (ii == null)
                ii = new ITRI_ADDON_MANA_01();
            return ii;
        }


        protected ITRI_ADDON_MANA_01()
            : base("ITRI_ADDON_MANA_01")
        {
            Visual = "ItRi_Prot_Total_01.3ds";
            Description = "Ring der Priester";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.MPMax += 30;
            npc.MP += 30;
        }

        protected void unequip(NPC npc, Item item)
        {
            if (npc.MP > 30 + 2)
                npc.MP -= 30;
            else
                npc.MP = 2;
            npc.MPMax -= 30;
        }
    }
}
