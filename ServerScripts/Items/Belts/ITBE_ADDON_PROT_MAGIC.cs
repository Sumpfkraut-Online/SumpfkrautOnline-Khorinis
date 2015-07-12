using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_PROT_MAGIC : AbstractBelts
    {
        static ITBE_ADDON_PROT_MAGIC ii;
        public static ITBE_ADDON_PROT_MAGIC get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_PROT_MAGIC();
            return ii;
        }


        protected ITBE_ADDON_PROT_MAGIC()
            : base("ITBE_ADDON_PROT_MAGIC")
        {
            Description = "Gürtel der magischen Abwehr";
            Visual = "ItMi_Belt_02.3ds";


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
