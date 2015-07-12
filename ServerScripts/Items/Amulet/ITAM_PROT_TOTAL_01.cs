using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_PROT_TOTAL_01 : AbstractAmulet
    {
        static ITAM_PROT_TOTAL_01 ii;
        public static ITAM_PROT_TOTAL_01 get()
        {
            if (ii == null)
                ii = new ITAM_PROT_TOTAL_01();
            return ii;
        }


        protected ITAM_PROT_TOTAL_01()
            : base("ITAM_PROT_TOTAL_01")
        {
            Visual = "ItAm_Prot_Total_01.3ds";
            Description = "Amulett der Erzhaut";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.ProtectionEdge += 8;
            npc.ProtectionBlunt += 8;
            npc.ProtectionPoint += 8;
            npc.ProtectionFire += 8;
            npc.ProtectionMagic += 8;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.ProtectionEdge -= 8;
            npc.ProtectionBlunt -= 8;
            npc.ProtectionPoint -= 8;
            npc.ProtectionFire -= 8;
            npc.ProtectionMagic -= 8;
        }
    }
}
