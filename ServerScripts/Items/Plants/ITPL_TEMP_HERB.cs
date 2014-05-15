using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_TERMP_HERB : AbstractPlants
    {
        static ITPL_TERMP_HERB ii;
        public static ITPL_TERMP_HERB get()
        {
            if (ii == null)
                ii = new ITPL_TERMP_HERB();
            return ii;
        }


        protected ITPL_TERMP_HERB()
            : base("ITPL_TERMP_HERB")
        {
            Name = "Feldknöterich";
            Visual = "ItPl_Temp_Herb.3ds";
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
