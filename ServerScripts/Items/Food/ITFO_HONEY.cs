using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_HONEY : AbstractFood
    {
        static ITFO_HONEY ii;
        public static ITFO_HONEY get()
        {
            if (ii == null)
                ii = new ITFO_HONEY();
            return ii;
        }


        protected ITFO_HONEY()
            : base("ITFO_HONEY")
        {
            Name = "Honig";
            Visual = "ItFo_Honey.3ds";
            Description = Name;
            ScemeName = "FOODHUGE";

            Materials = Enumeration.MaterialTypes.MAT_STONE;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 12;
        }
    }
}
