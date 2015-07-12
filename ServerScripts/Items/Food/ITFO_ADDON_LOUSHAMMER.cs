using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_LOUSHAMMER : AbstractFood
    {
        static ITFO_ADDON_LOUSHAMMER ii;
        public static ITFO_ADDON_LOUSHAMMER get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_LOUSHAMMER();
            return ii;
        }


        protected ITFO_ADDON_LOUSHAMMER()
            : base("ITFO_ADDON_LOUSHAMMER")
        {
            Name = "Lou's Hammer";
            Visual = "ItMi_Rum_01.3ds";
            Description = "Lou's Hammer";

            ScemeName = "POTIONFAST";
            Materials = Enumeration.MaterialType.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.MPMax += 1;

        }
    }
}
