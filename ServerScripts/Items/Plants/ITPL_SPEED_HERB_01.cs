using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Items.Potions;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_SPEED_HERB_01 : AbstractPlants
    {
        static ITPL_SPEED_HERB_01 ii;
        public static ITPL_SPEED_HERB_01 get()
        {
            if (ii == null)
                ii = new ITPL_SPEED_HERB_01();
            return ii;
        }


        protected ITPL_SPEED_HERB_01()
            : base("Snapperkraut")
        {
            Name = "Sumpfkraut";
            Visual = "ItPl_Speed_Herb_01.3ds";
            Description = Name;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.ApplyOverlay("HUMANS_SPRINT.MDS");

            ITPO_SPEED_TIMER timer = (ITPO_SPEED_TIMER)npc.getUserObjects("itpo_speed_timer");
            if (timer == null)
            {
                timer = new ITPO_SPEED_TIMER(npc);//5 Minuten
                timer.TimeSpan = 10000 * 1000 * 15;//15 Sekunden
                timer.Start();
                npc.setUserObject("itpo_speed_timer", timer);
            }
            else
            {
                timer.TimeSpan = 10000 * 1000 * 15;//15 Sekunden
                timer.End();
                timer.Start();
                
            }
        }
    }
}
