using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_SWAMPHERB : AbstractPlants
    {
        static ITPL_SWAMPHERB ii;
        public static ITPL_SWAMPHERB get()
        {
            if (ii == null)
                ii = new ITPL_SWAMPHERB();
            return ii;
        }


        protected ITPL_SWAMPHERB()
            : base("ITPL_SWAMPHERB")
        {
            Name = "Sumpfkraut";
            Visual = "ItPl_SwampHerb.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            if (npc is Player)
                npc.PlayPlayerEffect((Player)npc, "SLOW_TIME", npc, 0, 0, 0, false);
        }
    }
}
