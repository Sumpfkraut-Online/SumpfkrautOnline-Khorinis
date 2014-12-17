using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_PROT_MAGE_02 : AbstractRings
    {
        static ITRI_PROT_MAGE_02 ii;
        public static ITRI_PROT_MAGE_02 get()
        {
            if (ii == null)
                ii = new ITRI_PROT_MAGE_02();
            return ii;
        }


        protected ITRI_PROT_MAGE_02()
            : base("ITRI_PROT_MAGE_02")
        {
            Visual = "ItRi_Prot_Mage_02.3ds";
            Description = "Ring der Verteidigung";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.ProtectionMagic += 5;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.ProtectionMagic -= 5;
        }
    }
}
