using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_FIRESTEW : AbstractFood
    {
        static ITFO_ADDON_FIRESTEW ii;
        public static ITFO_ADDON_FIRESTEW get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_FIRESTEW();
            return ii;
        }


        protected ITFO_ADDON_FIRESTEW()
            : base("ITFO_ADDON_FIRESTEW")
        {
            Name = "Feuergeschnetzeltes";
            Visual = "ItFo_Stew.3ds";
            Description = Name;
            ScemeName = "RICE";

            Materials = Enumeration.MaterialTypes.MAT_WOOD;
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.Strength += 1;
            npc.HP += 5;

        }
    }
}
