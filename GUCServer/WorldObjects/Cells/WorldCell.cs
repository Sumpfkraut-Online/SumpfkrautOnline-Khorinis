using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.Cells
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
            this.coord = GetCoordinate(x, y);
        }

        protected static Vec2i GetCoords(Vec3f pos, int cellSize)
        {
            float unroundedX = pos.X / cellSize;
            float unroundedZ = pos.Z / cellSize;

            // calculate cell indices
            int x = (int)(pos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(pos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            return new Vec2i(x, z);
        }

        public static int GetCoordinate(int x, int y)
        {
            return (x << 16) | y & 0xFFFF;
        }
    }
}
