using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;

namespace GUC.Server.WorldObjects.Cells
{
    abstract class WorldCell
    {
        protected World world;
        public World World { get { return this.world; } }

        protected int x, y;
        public int X { get { return this.x; } }
        public int Y { get { return this.y; } }

        protected int coord;
        public int Coord { get { return this.coord; } }

        public WorldCell(World world, int x, int y)
        {
            this.world = world;
            this.x = x;
            this.y = y;
            this.coord = ((x << 16) | y & 0xFFFF);
        }
    }
}
