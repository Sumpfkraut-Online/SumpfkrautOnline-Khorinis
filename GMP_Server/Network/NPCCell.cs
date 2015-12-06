using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Enumeration;
using System.Collections;
using GUC.Types;

namespace GUC.Server.Network
{
    internal class NPCCell
    {
        public const float cellSize = 600;

        public World world;
        public int x, z;
        public List<NPC> NPCList;

        public NPCCell(World world, int x, int z)
        {
            this.world = world;
            NPCList = new List<NPC>();
            this.x = x;
            this.z = z;
        }

        public void Add(NPC npc)
        {
            NPCList.Add(npc);
            npc.npcCell = this;
        }

        public void Remove(NPC npc)
        {
            NPCList.Remove(npc);
            npc.npcCell = null;

            if (NPCList.Count == 0)
            {
                world.npcCells[x].Remove(z);
                if (world.npcCells[x].Count == 0)
                {
                    world.npcCells.Remove(x);
                }
            }
        }
    }
}
