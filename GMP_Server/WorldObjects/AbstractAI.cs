using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects
{
    public abstract class AbstractAI
    {
        protected NPC npc;
        public NPC NPC { get { return npc; } }

        public AbstractAI(NPC npc)
        {
            this.npc = npc;
        }

        private AbstractAI() { }

        protected void Goto()
        {

        }

        public void Update()
        {

        }
    }
}
