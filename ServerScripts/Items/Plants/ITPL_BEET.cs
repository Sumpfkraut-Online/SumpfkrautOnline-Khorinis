using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_BEET : AbstractPlants
    {
        static ITPL_BEET ii;
        public static ITPL_BEET get()
        {
            if (ii == null)
                ii = new ITPL_BEET();
            return ii;
        }


        protected ITPL_BEET()
            : base("ITPL_BEET")
        {
            Name = "Feldrübe";
            Visual = "ItPl_Beet.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 2;
        }
    }
}
