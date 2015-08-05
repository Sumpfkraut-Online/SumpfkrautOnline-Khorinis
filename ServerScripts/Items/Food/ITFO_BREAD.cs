using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_BREAD : AbstractFood
    {
        public ITFO_BREAD() : base()
        {
            Name = "Brot";
            Visual = "ItFo_Bread.3ds";
            Description = Name;
            ScemeName = "FOODHUGE";
        }

        protected void useItem(NPC npc)
        {

        }
    }
}
