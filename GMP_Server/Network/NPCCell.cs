using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Server.WorldObjects.Collections;

namespace GUC.Server.Network
{
    internal class NPCCell
    {
        public const float cellSize = 600;

        public World world;
        public int x, z;
        public VobObjDictionary<uint, NPC> Npcs = new VobObjDictionary<uint, NPC>();

        public NPCCell(World world, int x, int z)
        {
            this.world = world;
            this.x = x;
            this.z = z;
        }

        public void Add(NPC npc)
        {
            Npcs.Add(npc);
            npc.npcCell = this;
        }

        public void Remove(NPC npc)
        {
            Npcs.Remove(npc);
            npc.npcCell = null;

            if (Npcs.GetCount() == 0)
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
