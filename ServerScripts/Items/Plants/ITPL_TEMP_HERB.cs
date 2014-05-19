using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_TEMP_HERB : AbstractPlants
    {
        static ITPL_TEMP_HERB ii;
        public static ITPL_TEMP_HERB get()
        {
            if (ii == null)
                ii = new ITPL_TEMP_HERB();
            return ii;
        }


        protected ITPL_TEMP_HERB()
            : base("ITPL_TEMP_HERB")
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
