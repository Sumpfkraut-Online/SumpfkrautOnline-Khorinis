using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.GameObjects.Collections;

namespace GUC.WorldObjects.Cells
{
    class NPCCell : WorldCell
    {
        public const int CellSize = 800;
        
        public NPCCell(World world, int x, int y) : base(world, x, y)
        {
        }

        #region NPCs

        DynamicCollection<NPC> npcs = new DynamicCollection<NPC>();

        #region Add & Remove

        public void AddNPC(NPC npc)
        {
            npcs.Add(npc, ref npc.npcCellID);
        }

        public void RemoveNPC(NPC npc)
        {
            npcs.Remove(ref npc.npcCellID);
        }

        #endregion

        #region Access

        public void ForEachNPC(Action<NPC> action)
        {
            npcs.ForEach(action);
        }

        public void ForEachNPCPredicate(Predicate<NPC> predicate)
        {
            npcs.ForEachPredicate(predicate);
        }

        public int NPCCount { get { return npcs.Count; } }

        #endregion

        #endregion

        public static Vec2i GetCoords(Vec3f pos)
        {
            return WorldCell.GetCoords(pos, CellSize);
        }
    }
}
