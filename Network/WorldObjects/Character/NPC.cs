using System;
using System.Collections.Generic;
using System.Text;
using GUC.Enumeration;

namespace GUC.WorldObjects.Character
{
    internal partial class NPC : NPCProto
    {
        public Player NpcController = null;


        public NPC()
            : base()
        {
            this.type = (int)VobTypes.Npc;
        }

        

        
        public override void StealItem(Vob other, String item, int amount) { }
        public override void StealItem(Vob other, Item item, int amount) { }
    }
}
