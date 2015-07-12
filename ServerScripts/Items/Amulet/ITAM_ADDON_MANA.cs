using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Amulet
{
    public class ITAM_ADDON_MANA : AbstractAmulet
    {
        public ITAM_ADDON_MANA() : base()
        {
            Visual = "ItAm_Hp_01.3ds";
            Name = "Amulett der Heiler";
            Description = Name;

            OnEquip += equip;
            OnUnequip += unequip;
        }

        protected void equip(NPC npc, Item item)
        {
        }

        protected void unequip(NPC npc, Item item)
        {
        }
    }
}
