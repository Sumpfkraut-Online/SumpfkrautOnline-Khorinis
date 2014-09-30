using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_PLANEBERRY : AbstractPlants
    {
        static ITPL_PLANEBERRY ii;
        public static ITPL_PLANEBERRY get()
        {
            if (ii == null)
                ii = new ITPL_PLANEBERRY();
            return ii;
        }


        protected ITPL_PLANEBERRY()
            : base("ITPL_PLANEBERRY")
        {
            Name = "Weidenbeere";
            Visual = "ItPl_Planeberry.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 5;
            
        }
    }
}
