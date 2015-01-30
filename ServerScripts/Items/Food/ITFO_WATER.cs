using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_WATER : AbstractFood
    {
        static ITFO_WATER ii;
        public static ITFO_WATER get()
        {
            if (ii == null)
                ii = new ITFO_WATER();
            return ii;
        }


        protected ITFO_WATER()
            : base("ITFO_WATER")
        {
            Name = "Wasser";
            Visual = "ItFo_Water.3ds";
            Description = Name;
            ScemeName = "POTION";

            Materials = Enumeration.MaterialType.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 8;
        }
    }
}
