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
        Dictionary<int, NPC> npcs = new Dictionary<int, NPC>();

        public NPCCell(World world, int x, int z)
        {
            this.world = world;
            this.x = x;
            this.z = z;
        }

        public void Add(NPC npc)
        {
            npcs.Add(npc.ID, npc);
            npc.npcCell = this;
        }

        public void Remove(NPC npc)
        {
            npcs.Remove(npc.ID);
            npc.npcCell = null;

            if (npcs.Count == 0)
            {
                world.npcCells[x].Remove(z);
                if (world.npcCells[x].Count == 0)
                {
                    world.npcCells.Remove(x);
                }
            }
        }

        public void ForEach(Action<NPC> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach (NPC npc in npcs.Values)
                action(npc);
        }
    }
}
