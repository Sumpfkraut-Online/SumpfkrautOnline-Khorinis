using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_BLUEPLANT : AbstractPlants
    {
        static ITPL_BLUEPLANT ii;
        public static ITPL_BLUEPLANT get()
        {
            if (ii == null)
                ii = new ITPL_BLUEPLANT();
            return ii;
        }


        protected ITPL_BLUEPLANT()
            : base("ITPL_BLUEPLANT")
        {
            Name = "Blauflieder";
            Visual = "ItPl_Blueplant.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 5;
            npc.MP += 5;
        }
    }
}
