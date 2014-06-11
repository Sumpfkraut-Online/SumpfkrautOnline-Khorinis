using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_WINE : AbstractFood
    {
        static ITFO_WINE ii;
        public static ITFO_WINE get()
        {
            if (ii == null)
                ii = new ITFO_WINE();
            return ii;
        }


        protected ITFO_WINE()
            : base("ITFO_WINE")
        {
            Name = "Wein";
            Visual = "ItFo_Wine.3ds";
            Description = Name;
            ScemeName = "POTION";

            Materials = Enumeration.MaterialTypes.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 2;
            npc.MP += 1;
        }
    }
}
