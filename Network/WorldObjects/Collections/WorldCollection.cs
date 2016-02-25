using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public static partial class WorldCollection
    {
        /// <summary>
        /// The upper (excluded) limit for Worlds (byte.MaxValue+1).
        /// </summary>
        public const int MAX_WORLDS = 256;

        static Dictionary<int, World> worlds = new Dictionary<int, World>();

        #region Access

        public static World Get(int id)
        {
            World ret;
            worlds.TryGetValue(id, out ret);
            return ret;
        }

        public static void ForEach(Action<World> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach(World world in worlds.Values)
            {
                action(world);
            }
        }

        public static int GetCount()
        {
            return worlds.Count;
        }

        #endregion

        #region Add

        static partial void CheckID(World world);
        public static void Add(World world)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            CheckID(world);

            worlds.Add(world.ID, world);
        }

        #endregion

        #region Remove

        public static void Remove(World world)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            World other;
            if (worlds.TryGetValue(world.ID, out other))
            {
                if (other == world)
                {
                    worlds.Remove(world.ID);
                }
            }
        }

        #endregion
    }
}
