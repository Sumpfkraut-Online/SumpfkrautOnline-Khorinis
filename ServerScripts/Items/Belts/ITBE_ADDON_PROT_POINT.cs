using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_PROT_POINT : AbstractBelts
    {
        static ITBE_ADDON_PROT_POINT ii;
        public static ITBE_ADDON_PROT_POINT get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_PROT_POINT();
            return ii;
        }


        protected ITBE_ADDON_PROT_POINT()
            : base("ITBE_ADDON_PROT_POINT")
        {
            Description = "Gürtel der Abwehr";
            Visual = "ItMi_Belt_02.3ds";


            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);

            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.ProtectionPoint += 10;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.ProtectionPoint -= 10;
        }
    }
}
