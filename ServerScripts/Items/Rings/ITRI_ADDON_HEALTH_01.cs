using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_ADDON_HEALTH_01 : AbstractRings
    {
        static ITRI_ADDON_HEALTH_01 ii;
        public static ITRI_ADDON_HEALTH_01 get()
        {
            if (ii == null)
                ii = new ITRI_ADDON_HEALTH_01();
            return ii;
        }


        protected ITRI_ADDON_HEALTH_01()
            : base("ITRI_ADDON_HEALTH_01")
        {
            Visual = "ItRi_Prot_Total_01.3ds";
            Description = "Ring der Heiler";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.HPMax += 30;
            npc.HP += 30;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            if (npc.HP > 30 + 2)
                npc.HP -= 30;
            else
                npc.HP = 2;
            npc.HPMax -= 30;
        }
    }
}
