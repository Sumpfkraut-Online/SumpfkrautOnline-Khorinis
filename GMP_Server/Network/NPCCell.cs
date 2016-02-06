using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Collections;

namespace GUC.Server.Network
{
    internal class NPCCell
    {
        public const float cellSize = 600;

        public World world;
        public int x, z;
        public Dictionary<int, NPC> Npcs = new Dictionary<int, NPC>();

        public NPCCell(World world, int x, int z)
        {
            this.world = world;
            this.x = x;
            this.z = z;
        }

        public void Add(NPC npc)
        {
            Npcs.Add(npc.ID, npc);
            npc.npcCell = this;
        }

        public void Remove(NPC npc)
        {
            Npcs.Remove(npc.ID);
            npc.npcCell = null;

            if (Npcs.Count == 0)
            {
                /*world.npcCells[x].Remove(z);
                if (world.npcCells[x].Count == 0)
                {
                    world.npcCells.Remove(x);
                }*/
            }
        }
    }
}
