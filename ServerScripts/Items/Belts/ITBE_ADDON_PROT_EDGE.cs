using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_PROT_EDGE : AbstractBelts
    {
        static ITBE_ADDON_PROT_EDGE ii;
        public static ITBE_ADDON_PROT_EDGE get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_PROT_EDGE();
            return ii;
        }


        protected ITBE_ADDON_PROT_EDGE()
            : base("ITBE_ADDON_PROT_EDGE")
        {
            Description = "Gürtel des Schutzes";
            Visual = "ItMi_Belt_02.3ds";


            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);

            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.ProtectionEdge += 10;
            npc.ProtectionBlunt += 10;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.ProtectionEdge -= 10;
            npc.ProtectionBlunt -= 10;
        }
    }
}
