using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_CORAGONSBEER : AbstractFood
    {
        static ITFO_CORAGONSBEER ii;
        public static ITFO_CORAGONSBEER get()
        {
            if (ii == null)
                ii = new ITFO_CORAGONSBEER();
            return ii;
        }


        protected ITFO_CORAGONSBEER()
            : base("ITFO_CORAGONSBEER")
        {
            Name = "Bier";
            Visual = "ItFo_Beer.3ds";
            Description = Name;
            ScemeName = "POTIONFAST";

            Materials = Enumeration.MaterialTypes.MAT_GLAS;
            
            OnUse += new Scripting.Events.UseItemEventHandler(useItem);

            CreateItemInstance();
        }

        protected void useItem(NPCProto npc, Item item, short state, short targetState)
        {
            if (!(state == -1 && targetState == 0))
                return;

            npc.MPMax += 1;
            npc.HPMax += 3;
            npc.HP += 3;
            npc.MP += 1;
        }
    }
}
