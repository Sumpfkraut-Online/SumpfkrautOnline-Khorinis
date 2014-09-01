using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_RUM : AbstractFood
    {
        static ITFO_ADDON_RUM ii;
        public static ITFO_ADDON_RUM get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_RUM();
            return ii;
        }


        protected ITFO_ADDON_RUM()
            : base("ITFO_ADDON_RUM")
        {
            Name = "Rum";
            Visual = "ItMi_Rum_02.3ds";
            Description = "Weißer Rum";
            ScemeName = "POTIONFAST";

            Materials = Enumeration.MaterialType.MAT_GLAS;
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.MP += 10;

        }
    }
}
