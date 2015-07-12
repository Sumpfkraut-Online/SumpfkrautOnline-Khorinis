using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_GROG : AbstractFood
    {
        static ITFO_ADDON_GROG ii;
        public static ITFO_ADDON_GROG get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_GROG();
            return ii;
        }


        protected ITFO_ADDON_GROG()
            : base("ITFO_ADDON_GROG")
        {
            Name = "Grog";
            Visual = "ItMi_Rum_02.3ds";
            Description = "Echter Seemanns Grog";

            ScemeName = "POTIONFAST";
            Materials = Enumeration.MaterialType.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 1;

        }
    }
}
