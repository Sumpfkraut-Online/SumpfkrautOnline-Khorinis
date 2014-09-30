using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_MUSHROOM_02 : AbstractPlants
    {
        static ITPL_MUSHROOM_02 ii;
        public static ITPL_MUSHROOM_02 get()
        {
            if (ii == null)
                ii = new ITPL_MUSHROOM_02();
            return ii;
        }


        protected ITPL_MUSHROOM_02()
            : base("ITPL_MUSHROOM_02")
        {
            Name = "Buddlerfleisch";
            Visual = "ItPl_Mushroom_02.3ds";
            Description = Name;

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
