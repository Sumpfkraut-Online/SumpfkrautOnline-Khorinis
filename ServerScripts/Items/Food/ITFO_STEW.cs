using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_STEW : AbstractFood
    {
        static ITFO_STEW ii;
        public static ITFO_STEW get()
        {
            if (ii == null)
                ii = new ITFO_STEW();
            return ii;
        }


        protected ITFO_STEW()
            : base("ITFO_STEW")
        {
            Name = "Eintopf";
            Visual = "ItFo_Stew.3ds";
            Description = Name;
            ScemeName = "RICE";

            Materials = Enumeration.MaterialType.MAT_WOOD;
            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 20;
        }
    }
}
