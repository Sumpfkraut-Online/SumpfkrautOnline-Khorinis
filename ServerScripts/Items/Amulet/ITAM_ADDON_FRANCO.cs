using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_ADDON_FRANCO : AbstractAmulets
    {
        static ITAM_ADDON_FRANCO ii;
        public static ITAM_ADDON_FRANCO get()
        {
            if (ii == null)
                ii = new ITAM_ADDON_FRANCO();
            return ii;
        }


        protected ITAM_ADDON_FRANCO()
            : base("ITAM_ADDON_FRANCO")
        {
            Visual = "ItAm_Hp_01.3ds";
            Description = "Franco's Amulett";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.Strength += 4;
            npc.Dexterity += 4;
            npc.HPMax += 40;
            npc.HP += 40;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.Strength -= 4;
            npc.Dexterity -= 4;
            npc.HPMax -= 40;

            if (npc.HP > 42)
                npc.HP -= 40;
            else
                npc.HP = 2;
        }
    }
}
