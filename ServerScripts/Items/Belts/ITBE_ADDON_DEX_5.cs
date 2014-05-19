using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_DEX_5 : AbstractBelts
    {
        static ITBE_ADDON_DEX_5 ii;
        public static ITBE_ADDON_DEX_5 get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_DEX_5();
            return ii;
        }


        protected ITBE_ADDON_DEX_5()
            : base("ITBE_ADDON_DEX_5")
        {
            Description = "Gürtel der Gewandtheit";
            Visual = "ItMi_Belt_08.3ds";


            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);

            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.Dexterity += 5;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.Dexterity -= 5;
        }
    }
}
