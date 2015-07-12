using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_MANA_02 : AbstractRings
    {
        static ITRI_MANA_02 ii;
        public static ITRI_MANA_02 get()
        {
            if (ii == null)
                ii = new ITRI_MANA_02();
            return ii;
        }


        protected ITRI_MANA_02()
            : base("ITRI_MANA_02")
        {
            Visual = "ItRi_Mana_02.3ds";
            Description = "Ring der astralen Kraft";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.MPMax += 10;
            npc.MP += 10;
        }

        protected void unequip(NPC npc, Item item)
        {
            if (npc.MP > 10)
                npc.MP -= 10;
            else
                npc.MP = 0;
            npc.MPMax -= 10;
        }
    }
}
