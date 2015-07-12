using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_MANA_HERB_03 : AbstractPlants
    {
        static ITPL_MANA_HERB_03 ii;
        public static ITPL_MANA_HERB_03 get()
        {
            if (ii == null)
                ii = new ITPL_MANA_HERB_03();
            return ii;
        }


        protected ITPL_MANA_HERB_03()
            : base("ITPL_MANA_HERB_03")
        {
            Name = "Feuerwurzel";
            Visual = "ItPl_Mana_Herb_03.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.MP += 20;

        }
    }
}
