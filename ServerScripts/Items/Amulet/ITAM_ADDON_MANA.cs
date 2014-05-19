using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_ADDON_MANA : AbstractAmulets
    {
        static ITAM_ADDON_MANA ii;
        public static ITAM_ADDON_MANA get()
        {
            if (ii == null)
                ii = new ITAM_ADDON_MANA();
            return ii;
        }


        protected ITAM_ADDON_MANA()
            : base("ITAM_ADDON_MANA")
        {
            Visual = "ItAm_Hp_01.3ds";
            Description = "Amulett der Heiler";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.MPMax += 50;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.MPMax -= 50;
        }
    }
}
