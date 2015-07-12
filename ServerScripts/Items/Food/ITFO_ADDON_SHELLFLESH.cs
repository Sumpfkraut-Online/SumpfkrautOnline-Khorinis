using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_SHELLFLESH : AbstractFood
    {
        static ITFO_ADDON_SHELLFLESH ii;
        public static ITFO_ADDON_SHELLFLESH get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_SHELLFLESH();
            return ii;
        }


        protected ITFO_ADDON_SHELLFLESH()
            : base("ITFO_ADDON_SHELLFLESH")
        {
            Name = "Muschelfleisch";
            Visual = "ItAt_Meatbugflesh.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.HP += 20;

        }
    }
}
