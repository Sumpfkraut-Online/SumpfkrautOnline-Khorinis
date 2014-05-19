using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_HP_02 : AbstractRings
    {
        static ITRI_HP_02 ii;
        public static ITRI_HP_02 get()
        {
            if (ii == null)
                ii = new ITRI_HP_02();
            return ii;
        }


        protected ITRI_HP_02()
            : base("ITRI_HP_02")
        {
            Visual = "ItRi_Hp_02.3ds";
            Description = "Ring der Lebendigkeit";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.HPMax += 40;
            npc.HP += 40;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            if (npc.HP > 40 + 2)
                npc.HP -= 40;
            else
                npc.HP = 2;
            npc.HPMax -= 40;
        }
    }
}
