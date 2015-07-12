using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_MC : AbstractBelts
    {
        static ITBE_ADDON_MC ii;
        public static ITBE_ADDON_MC get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_MC();
            return ii;
        }


        protected ITBE_ADDON_MC()
            : base("ITBE_ADDON_MC")
        {
            Description = "Minecrawler Gürtel";
            Visual = "ItMi_Belt_08.3ds";


            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);

            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.ProtectionEdge += 5;
            npc.ProtectionBlunt += 5;
            npc.ProtectionPoint += 5;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.ProtectionEdge -= 5;
            npc.ProtectionBlunt -= 5;
            npc.ProtectionPoint -= 5;
        }
    }
}
