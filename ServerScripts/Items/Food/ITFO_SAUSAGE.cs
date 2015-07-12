using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_SAUSAGE : AbstractFood
    {
        static ITFO_SAUSAGE ii;
        public static ITFO_SAUSAGE get()
        {
            if (ii == null)
                ii = new ITFO_SAUSAGE();
            return ii;
        }


        protected ITFO_SAUSAGE()
            : base("ITFO_SAUSAGE")
        {
            Name = "Wurst";
            Visual = "ItFo_Sausage.3ds";
            Description = Name;
            ScemeName = "FOODHUGE";
            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 12;
        }
    }
}
