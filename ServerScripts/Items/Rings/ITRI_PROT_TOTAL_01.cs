using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_PROT_TOTAL_01 : AbstractRings
    {
        static ITRI_PROT_TOTAL_01 ii;
        public static ITRI_PROT_TOTAL_01 get()
        {
            if (ii == null)
                ii = new ITRI_PROT_TOTAL_01();
            return ii;
        }


        protected ITRI_PROT_TOTAL_01()
            : base("ITRI_PROT_TOTAL_01")
        {
            Visual = "ItRi_Prot_Total_01.3ds";
            Description = "Ring der Unbezwingbarkeit";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.ProtectionEdge += 3;
            npc.ProtectionBlunt += 3;
            npc.ProtectionPoint += 3;
            npc.ProtectionFire += 3;
            npc.ProtectionMagic += 3;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.ProtectionEdge -= 3;
            npc.ProtectionBlunt -= 3;
            npc.ProtectionPoint -= 3;
            npc.ProtectionFire -= 3;
            npc.ProtectionMagic -= 3;
        }
    }
}
