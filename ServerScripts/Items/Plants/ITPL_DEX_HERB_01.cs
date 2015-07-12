using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_DEX_HERB_01 : AbstractPlants
    {
        static ITPL_DEX_HERB_01 ii;
        public static ITPL_DEX_HERB_01 get()
        {
            if (ii == null)
                ii = new ITPL_DEX_HERB_01();
            return ii;
        }


        protected ITPL_DEX_HERB_01()
            : base("ITPL_DEX_HERB_01")
        {
            Name = "Goblin Beeren";
            Visual = "ItPl_Dex_Herb_01.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.Dexterity += 1;

        }
    }
}
