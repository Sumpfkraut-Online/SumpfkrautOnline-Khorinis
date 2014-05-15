using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting;

namespace GUC.Server.Scripts.Items.Potions
{
    
    public class ITPO_SPEED: ItemInstance
    {
        static ITPO_SPEED ii;
        public static ITPO_SPEED get()
        {
            if (ii == null)
                ii = new ITPO_SPEED();
            return ii;
        }

        protected ITPO_SPEED()
            : base("ITPO_SPEED")
        {
            Name = "Trank";
            Value = 25;
            Visual = "ItPo_Speed.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialTypes.MAT_GLAS;
            Description = "Trank der Geschwindigkeit";

            Effect = "SPELLFX_ITEMGLIMMER";

            Wear = ArmorFlags.WEAR_EFFECT;
            ScemeName = "POTIONFAST";

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
                timer.Start();
                npc.setUserObject("itpo_speed_timer", timer);
            }
            else
            {
                timer.End();
                timer.Start();
            }
        }

        
    }

    public class ITPO_SPEED_TIMER : Timer
    {
        NPCProto npc = null;

        public ITPO_SPEED_TIMER(NPCProto proto)
            : base(3000000000)
        {
            npc = proto;

            OnTick += new Events.TimerEvent(endTimer);
        }

        protected void endTimer()
        {
            this.End();
            npc.setUserObject("itpo_speed_timer", null);

            npc.RemoveOverlay("itpo_speed_timer");
        }
    }

}
