using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_HP_01 : AbstractAmulets
    {
        static ITAM_HP_01 ii;
        public static ITAM_HP_01 get()
        {
            if (ii == null)
                ii = new ITAM_HP_01();
            return ii;
        }


        protected ITAM_HP_01()
            : base("ITAM_HP_01")
        {
            Visual = "ItAm_Hp_01.3ds";
            Description = "Amulett der Lebenskraft";

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
