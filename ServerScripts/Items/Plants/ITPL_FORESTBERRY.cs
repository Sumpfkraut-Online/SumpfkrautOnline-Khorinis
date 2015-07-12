using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_FORESTBERRY : AbstractPlants
    {
        static ITPL_FORESTBERRY ii;
        public static ITPL_FORESTBERRY get()
        {
            if (ii == null)
                ii = new ITPL_FORESTBERRY();
            return ii;
        }


        protected ITPL_FORESTBERRY()
            : base("ITPL_FORESTBERRY")
        {
            Name = "Waldbeere";
            Visual = "ItPl_Forestberry.3ds";
            Description = Name;

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
