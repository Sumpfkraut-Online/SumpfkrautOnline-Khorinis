using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_HP_MANA_01 : AbstractAmulet
    {
        static ITAM_HP_MANA_01 ii;
        public static ITAM_HP_MANA_01 get()
        {
            if (ii == null)
                ii = new ITAM_HP_MANA_01();
            return ii;
        }


        protected ITAM_HP_MANA_01()
            : base("ITAM_HP_MANA_01")
        {
            Visual = "ItAm_Hp_Mana_01.3ds";
            Description = "Amulett der Erleuchtung";
            Name = Description;

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.MPMax += 10;
            npc.MP += 10;
            npc.HPMax += 30;
            npc.HP += 30;
        }

        protected void unequip(NPC npc, Item item)
        {

            if (npc.MP > 10)
                npc.MP -= 10;
            else
                npc.MP = 0;
            npc.MPMax -= 10;

            if (npc.HP > 30 + 2)
                npc.HP -= 30;
            else
                npc.HP = 2;
            npc.MPMax -= 30;
        }
    }
}
