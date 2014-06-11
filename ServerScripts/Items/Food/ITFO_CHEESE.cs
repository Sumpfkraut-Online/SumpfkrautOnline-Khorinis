using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_CHEESE : AbstractFood
    {
        static ITFO_CHEESE ii;
        public static ITFO_CHEESE get()
        {
            if (ii == null)
                ii = new ITFO_CHEESE();
            return ii;
        }


        protected ITFO_CHEESE()
            : base("ITFO_CHEESE")
        {
            Name = "Käse";
            Visual = "ItFo_Cheese.3ds";
            Description = Name;
            ScemeName = "FOODHUGE";

            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 15;
        }
    }
}
