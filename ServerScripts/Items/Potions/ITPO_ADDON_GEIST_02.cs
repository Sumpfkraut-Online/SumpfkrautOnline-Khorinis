using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting;

namespace GUC.Server.Scripts.Items.Potions
{
    
    public class ITPO_ADDON_GEIST_02: ItemInstance
    {
        static ITPO_ADDON_GEIST_02 ii;
        public static ITPO_ADDON_GEIST_02 get()
        {
            if (ii == null)
                ii = new ITPO_ADDON_GEIST_02();
            return ii;
        }

        protected ITPO_ADDON_GEIST_02()
            : base("ITPO_ADDON_GEIST_02")
        {
            Name = "Trank";
            Value = 25;
            Visual = "ItPo_Perm_STR.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialType.MAT_GLAS;
            Description = "Trank der Geistesveränderung";

            Effect = "SPELLFX_ITEMGLIMMER";

            Wear = ArmorFlags.WEAR_EFFECT;
            ScemeName = "POTIONFAST";

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            
        }

        
    }

}
