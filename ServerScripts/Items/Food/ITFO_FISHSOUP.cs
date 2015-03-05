using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_FISHSOUP : AbstractFood
    {
        static ITFO_FISHSOUP ii;
        public static ITFO_FISHSOUP get()
        {
            if (ii == null)
                ii = new ITFO_FISHSOUP();
            return ii;
        }


        protected ITFO_FISHSOUP()
            : base("ITFO_FISHSOUP")
        {
            Name = "Fischsuppe";
            Visual = "ItFo_FishSoup.3ds";
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

            npc.HP += 10;
        }
    }
}
