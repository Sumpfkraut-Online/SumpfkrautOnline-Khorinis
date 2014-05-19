using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_PROT_POINT_01 : AbstractRings
    {
        static ITRI_PROT_POINT_01 ii;
        public static ITRI_PROT_POINT_01 get()
        {
            if (ii == null)
                ii = new ITRI_PROT_POINT_01();
            return ii;
        }


        protected ITRI_PROT_POINT_01()
            : base("ITRI_PROT_POINT_01")
        {
            Visual = "ItRi_Prot_Point_01.3ds";
            Description = "Ring der Holzhaut";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.ProtectionPoint += 3;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.ProtectionPoint -= 3;
        }
    }
}
