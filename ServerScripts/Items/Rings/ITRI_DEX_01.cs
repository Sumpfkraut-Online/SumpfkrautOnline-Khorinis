using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Rings
{
    public class ITRI_DEX_01 : AbstractRings
    {
        static ITRI_DEX_01 ii;
        public static ITRI_DEX_01 get()
        {
            if (ii == null)
                ii = new ITRI_DEX_01();
            return ii;
        }


        protected ITRI_DEX_01()
            : base("ITRI_DEX_01")
        {
            Visual = "ItRi_Dex_01.3ds";
            Description = "Ring der Gewandtheit";

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPC npc, Item item)
        {
            npc.Dexterity += 3;
        }

        protected void unequip(NPC npc, Item item)
        {
            npc.Dexterity -= 3;
        }
    }
}
