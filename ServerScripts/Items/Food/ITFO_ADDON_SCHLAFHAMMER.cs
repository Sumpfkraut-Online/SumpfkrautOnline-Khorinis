using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_SCHLAFHAMMER : AbstractFood
    {
        static ITFO_ADDON_SCHLAFHAMMER ii;
        public static ITFO_ADDON_SCHLAFHAMMER get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_SCHLAFHAMMER();
            return ii;
        }


        protected ITFO_ADDON_SCHLAFHAMMER()
            : base("ITFO_ADDON_SCHLAFHAMMER")
        {
            Name = "Doppelter Hammer";
            Visual = "ItMi_Rum_01.3ds";
            Description = "Doppelter Hammer";

            ScemeName = "POTIONFAST";
            Materials = Enumeration.MaterialTypes.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;
            if (npc.HP > 2)
                npc.HP /= 2;
        }
    }
}
