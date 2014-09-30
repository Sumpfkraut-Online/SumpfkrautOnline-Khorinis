using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_MILK : AbstractFood
    {
        static ITFO_MILK ii;
        public static ITFO_MILK get()
        {
            if (ii == null)
                ii = new ITFO_MILK();
            return ii;
        }


        protected ITFO_MILK()
            : base("ITFO_MILK")
        {
            Name = "Milch";
            Visual = "ItFo_Milk.3ds";
            Description = Name;
            ScemeName = "POTION";

            Materials = Enumeration.MaterialTypes.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 5;
            npc.MP += 1;
        }
    }
}
