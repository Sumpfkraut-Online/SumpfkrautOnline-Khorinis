using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_DEX_STRG_01 : AbstractAmulets
    {
        static ITAM_DEX_STRG_01 ii;
        public static ITAM_DEX_STRG_01 get()
        {
            if (ii == null)
                ii = new ITAM_DEX_STRG_01();
            return ii;
        }


        protected ITAM_DEX_STRG_01()
            : base("ITAM_DEX_STRG_01")
        {
            Visual = "ItAm_Dex_Strg_01.3ds";
            Description = "Amulett der Macht";
            Name = Description;

            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);
            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.Dexterity += 8;
            npc.Strength += 8;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.Dexterity -= 8;
            npc.Strength -= 8;
        }
    }
}
