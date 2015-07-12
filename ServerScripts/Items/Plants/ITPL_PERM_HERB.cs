using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_PERM_HERB : AbstractPlants
    {
        static ITPL_PERM_HERB ii;
        public static ITPL_PERM_HERB get()
        {
            if (ii == null)
                ii = new ITPL_PERM_HERB();
            return ii;
        }


        protected ITPL_PERM_HERB()
            : base("ITPL_PERM_HERB")
        {
            Name = "Kronstöckel";
            Visual = "ItPl_Perm_Herb.3ds";
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
