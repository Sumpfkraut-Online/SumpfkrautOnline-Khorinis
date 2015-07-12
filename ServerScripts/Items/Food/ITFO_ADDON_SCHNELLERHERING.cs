using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Items.Potions;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_SCHNELLERHERING : AbstractFood
    {
        static ITFO_ADDON_SCHNELLERHERING ii;
        public static ITFO_ADDON_SCHNELLERHERING get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_SCHNELLERHERING();
            return ii;
        }


        protected ITFO_ADDON_SCHNELLERHERING()
            : base("ITFO_ADDON_SCHNELLERHERING")
        {
            Name = "Schneller Hering";
            Visual = "ItMi_Rum_01.3ds";
            Description = "Sieht gefährlich aus!";

            ScemeName = "POTIONFAST";
            Materials = Enumeration.MaterialType.MAT_GLAS;

            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPC npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == -2))
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
                timer.TimeSpan = 10000 * 1000 * 60 * 2;//15 Sekunden
                timer.End();
                timer.Start();

            }
        }
    }
}
