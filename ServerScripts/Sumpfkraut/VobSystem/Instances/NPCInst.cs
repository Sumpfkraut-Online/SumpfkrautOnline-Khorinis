using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances
{
    /**
     *   Class which handles npc creation.
     */
    class NPCInst : VobInst
    {

        protected static String _staticName = "NPCInst (static)";
        protected String _objName = "NPCInst (default)";
        
        // definition on which basis the item was created
        private NPCDef npcDef;
        public NPCDef getNPCDef () { return this.npcDef; }
        public void setNPCDef (NPCDef npcDef) { this.npcDef = npcDef; }

        // the ingame-item created by using itemDef
        private NPC npc;
        public NPC getNPC () { return this.npc; }
        public void setNPC (NPC npc) { this.npc = npc; }

        // TO DO: changing worlds must also displace the npc ingame at the same time
        private WorldInst inWorld;
        public WorldInst getInWorld () { return inWorld; }
        public void setInWorld (WorldInst inWorld) { this.inWorld = inWorld; }


        //// does not spawn NPC directly into a world
        //public NPCInst (NPCDef def)
        //{
        //    this.npc = new NPC(def.getName());
        //}

        // 
        //public NPCInst (NPCDef def, WorldInst inWorld)
        //    : this(def, inWorld, )
        //{

        //}

    }
}
