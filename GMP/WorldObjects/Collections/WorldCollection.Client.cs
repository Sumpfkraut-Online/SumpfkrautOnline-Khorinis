using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public static partial class WorldCollection
    {
        static partial void CheckID(World world)
        {
            if (world.ID < 0 || world.ID >= MAX_WORLDS)
                throw new ArgumentOutOfRangeException("World ID is out of range! 0.." + MAX_WORLDS);
        }
    }
}
