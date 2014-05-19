using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_BREAD : AbstractFood
    {
        static ITFO_BREAD ii;
        public static ITFO_BREAD get()
        {
            if (ii == null)
                ii = new ITFO_BREAD();
            return ii;
        }


        protected ITFO_BREAD()
            : base("ITFO_BREAD")
        {
            Name = "Brot";
            Visual = "ItFo_Bread.3ds";
            Description = Name;
            ScemeName = "FOODHUGE";

            
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
