using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_XPSTEW : AbstractFood
    {
        static ITFO_XPSTEW ii;
        public static ITFO_XPSTEW get()
        {
            if (ii == null)
                ii = new ITFO_XPSTEW();
            return ii;
        }


        protected ITFO_XPSTEW()
            : base("ITFO_XPSTEW")
        {
            Name = "Thekla's Eintopf";
            Visual = "ItFo_Stew.3ds";
            Description = Name;
            ScemeName = "RICE";

            Materials = Enumeration.MaterialTypes.MAT_WOOD;
            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 20;
            npc.Strength += 1;
        }
    }
}
