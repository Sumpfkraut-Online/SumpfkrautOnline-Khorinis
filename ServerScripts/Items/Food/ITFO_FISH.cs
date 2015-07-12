using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_FISH : AbstractFood
    {
        static ITFO_FISH ii;
        public static ITFO_FISH get()
        {
            if (ii == null)
                ii = new ITFO_FISH();
            return ii;
        }


        protected ITFO_FISH()
            : base("ITFO_FISH")
        {
            Name = "Fisch";
            Visual = "ItFo_Fish.3ds";
            Description = Name;
            ScemeName = "FOODHUGE";

            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 5;
        }
    }
}
