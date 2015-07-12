using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_PROT_TOTAL : AbstractBelts
    {
        static ITBE_ADDON_PROT_TOTAL ii;
        public static ITBE_ADDON_PROT_TOTAL get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_PROT_TOTAL();
            return ii;
        }


        protected ITBE_ADDON_PROT_TOTAL()
            : base("ITBE_ADDON_PROT_TOTAL")
        {
            Description = "Beschützer-Gürtel";
            Visual = "ItMi_Belt_02.3ds";


            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);

            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.ProtectionEdge += 7;
            npc.ProtectionBlunt += 7;
            npc.ProtectionPoint += 7;
            npc.ProtectionMagic += 7;
            npc.ProtectionFire += 7;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.ProtectionEdge -= 7;
            npc.ProtectionBlunt -= 7;
            npc.ProtectionPoint -= 7;
            npc.ProtectionMagic -= 7;
            npc.ProtectionFire -= 7;
        }
    }
}
