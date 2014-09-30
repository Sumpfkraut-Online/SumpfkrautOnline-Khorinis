using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_APPLE : AbstractFood
    {
        static ITFO_APPLE ii;
        public static ITFO_APPLE get()
        {
            if (ii == null)
                ii = new ITFO_APPLE();
            return ii;
        }


        protected ITFO_APPLE()
            : base("ITFO_APPLE")
        {
            Name = "Apfel";
            Visual = "ItFo_Apple.3ds";
            Description = Name;
            ScemeName = "FOOD";

            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 3;
        }
    }
}
