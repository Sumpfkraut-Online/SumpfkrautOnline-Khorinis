using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_BOOZE : AbstractFood
    {
        static ITFO_BOOZE ii;
        public static ITFO_BOOZE get()
        {
            if (ii == null)
                ii = new ITFO_BOOZE();
            return ii;
        }


        protected ITFO_BOOZE()
            : base("ITFO_BOOZE")
        {
            Name = "Wacholder";
            Visual = "ItFo_Booze.3ds";
            Description = Name;
            ScemeName = "POTION";

            Materials = Enumeration.MaterialType.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 4;
            npc.MP += 1;
        }
    }
}
