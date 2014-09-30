using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFOMUTTONRAW : AbstractFood
    {
        static ITFOMUTTONRAW ii;
        public static ITFOMUTTONRAW get()
        {
            if (ii == null)
                ii = new ITFOMUTTONRAW();
            return ii;
        }


        protected ITFOMUTTONRAW()
            : base("ITFOMUTTONRAW")
        {
            Name = "Rohes Fleisch";
            Visual = "ItFoMuttonRaw.3ds";
            Description = Name;
            ScemeName = "MEAT";

            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 6;
        }
    }
}
