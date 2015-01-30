using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_MEATSOUP : AbstractFood
    {
        static ITFO_ADDON_MEATSOUP ii;
        public static ITFO_ADDON_MEATSOUP get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_MEATSOUP();
            return ii;
        }


        protected ITFO_ADDON_MEATSOUP()
            : base("ITFO_ADDON_MEATSOUP")
        {
            Name = "Fleischsuppe";
            Visual = "ItFo_FishSoup.3ds";
            Description = "Dampfende Fleischsuppe";
            ScemeName = "RICE";

            Materials = Enumeration.MaterialType.MAT_WOOD;
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.Strength += 1;
        }
    }
}
