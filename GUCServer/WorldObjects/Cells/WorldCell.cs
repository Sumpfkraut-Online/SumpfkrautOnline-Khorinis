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
            this.coord = ((x << 16) | y & 0xFFFF);
        }

        protected static int[] GetCoords(Vec3f pos, int cellSize)
        {
            float unroundedX = pos.X / cellSize;
            float unroundedZ = pos.Z / cellSize;

            // calculate new cell indices
            int x = (int)(pos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(pos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);
            
            return new int[2] { x, z };
        }

        public static int GetCoordFromCoords(int x, int y)
        {
            if (x < short.MinValue || x > short.MaxValue || y < short.MinValue || y > short.MaxValue)
            {
                throw new Exception("Coords are out of cell range! " + x + " " + y);
            }

            return (x << 16) | y & 0xFFFF;
        }
    }
}
