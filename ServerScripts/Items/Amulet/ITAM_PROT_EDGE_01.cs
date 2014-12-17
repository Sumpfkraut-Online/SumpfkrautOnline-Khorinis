using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_PROT_EDGE_01 : AbstractAmulets
    {
        static ITAM_PROT_EDGE_01 ii;
        public static ITAM_PROT_EDGE_01 get()
        {
            if (ii == null)
                ii = new ITAM_PROT_EDGE_01();
            return ii;
        }


        protected ITAM_PROT_EDGE_01()
            : base("ITAM_PROT_EDGE_01")
        {
            Visual = "ItAm_Prot_Edge_01.3ds";
            Description = "Harnischamulett";

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
