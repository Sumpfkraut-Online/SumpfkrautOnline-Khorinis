using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Belts
{
    public class ITBE_ADDON_DEX_10 : AbstractBelts
    {
        static ITBE_ADDON_DEX_10 ii;
        public static ITBE_ADDON_DEX_10 get()
        {
            if (ii == null)
                ii = new ITBE_ADDON_DEX_10();
            return ii;
        }


        protected ITBE_ADDON_DEX_10()
            : base("ITBE_ADDON_DEX_10")
        {
            Description = "Gürtel der Geschicklichkeit";
            Visual = "ItMi_Belt_05.3ds";


            OnEquip += new Scripting.Events.NPCEquipEventHandler(equip);
            OnUnEquip += new Scripting.Events.NPCEquipEventHandler(unequip);

            CreateItemInstance();
        }

        protected void equip(NPCProto npc, Item item)
        {
            npc.Dexterity += 10;
        }

        protected void unequip(NPCProto npc, Item item)
        {
            npc.Dexterity -= 10;
        }
    }
}
