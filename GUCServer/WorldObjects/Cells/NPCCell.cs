using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;

namespace GUC.Server.WorldObjects.Cells
{
    class NPCCell : WorldCell
    {
        public const int Size = 600;

        public readonly DynamicCollection<NPC> npcs = new DynamicCollection<NPC>();

        public NPCCell(World world, int x, int y) : base(world, x, y)
        {
        }
    }
}
