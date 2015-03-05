using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_BEER : AbstractFood
    {
        static ITFO_BEER ii;
        public static ITFO_BEER get()
        {
            if (ii == null)
                ii = new ITFO_BEER();
            return ii;
        }


        protected ITFO_BEER()
            : base("ITFO_BEER")
        {
            Name = "Bier";
            Visual = "ItFo_Beer.3ds";
            Description = Name;
            ScemeName = "POTIONFAST";

            Materials = Enumeration.MaterialType.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 3;
            npc.MP += 1;
        }
    }
}
