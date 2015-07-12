using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_STR_10 : AbstractBelts
    {
        static ITBE_ADDON_STR_10 ii;
        public static ITBE_ADDON_STR_10 get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_STR_10();
            return ii;
        }


        protected ITBE_ADDON_STR_10()
            : base("ITBE_ADDON_STR_10")
        {
            Description = "Gürtel der Stärke";
            Visual = "ItMi_Belt_05.3ds";


            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);

            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.Strength += 10;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.Strength -= 10;
        }
    }
}
