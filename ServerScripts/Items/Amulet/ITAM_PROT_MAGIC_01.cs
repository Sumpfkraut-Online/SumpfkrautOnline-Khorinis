using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_PROT_MAGIC_01 : AbstractAmulet
    {
        static ITAM_PROT_MAGIC_01 ii;
        public static ITAM_PROT_MAGIC_01 get()
        {
            if (ii == null)
                ii = new ITAM_PROT_MAGIC_01();
            return ii;
        }


        protected ITAM_PROT_MAGIC_01()
            : base("ITAM_PROT_MAGIC_01")
        {
            Visual = "ItAm_Prot_Mage_01.3ds";
            Description = "Amulett der Geisteskraft";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.ProtectionMagic += 10;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.ProtectionMagic -= 10;
        }
    }
}
