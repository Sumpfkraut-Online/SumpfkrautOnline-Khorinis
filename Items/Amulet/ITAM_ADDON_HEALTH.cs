using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_ADDON_HEALTH : AbstractAmulets
    {
        static ITAM_ADDON_HEALTH ii;
        public static ITAM_ADDON_HEALTH get()
        {
            if (ii == null)
                ii = new ITAM_ADDON_HEALTH();
            return ii;
        }


        protected ITAM_ADDON_HEALTH()
            : base("ITAM_ADDON_HEALTH")
        {
            Visual = "ItAm_Hp_01.3ds";
            Description = "Amulett der Heiler";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.HPMax += 50;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.HPMax -= 50;
        }
    }
}
