using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_PROT_TOTAL_02 : AbstractRings
    {
        static ITRI_PROT_TOTAL_02 ii;
        public static ITRI_PROT_TOTAL_02 get()
        {
            if (ii == null)
                ii = new ITRI_PROT_TOTAL_02();
            return ii;
        }


        protected ITRI_PROT_TOTAL_02()
            : base("ITRI_PROT_TOTAL_02")
        {
            Visual = "ItRi_Prot_Total_02.3ds";
            Description = "Ring der Unbesiegbarkeit";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.ProtectionEdge += 5;
            npc.ProtectionBlunt += 5;
            npc.ProtectionPoint += 5;
            npc.ProtectionFire += 5;
            npc.ProtectionMagic += 5;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.ProtectionEdge -= 5;
            npc.ProtectionBlunt -= 5;
            npc.ProtectionPoint -= 5;
            npc.ProtectionFire -= 5;
            npc.ProtectionMagic -= 5;
        }
    }
}
