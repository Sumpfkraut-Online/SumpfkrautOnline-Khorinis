using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_APPLE : AbstractFood
    {
        public ITFO_APPLE() : base()
        {
            Name = "Apfel";
            Visual = "ItFo_Apple.3ds";
            Description = Name;
            ScemeName = "FOOD";
            Text[0] = "Lebensenergie wiederherstellen:"; Count[0] = 3;
            Text[5] = "Gewicht:"; Count[5] = Weight;
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

        }
    }
}
