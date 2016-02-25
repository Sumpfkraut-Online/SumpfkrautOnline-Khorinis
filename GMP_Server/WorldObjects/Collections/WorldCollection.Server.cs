using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public static partial class WorldCollection
    {
        static List<int> freeIDs = new List<int>();
        static int idCount = 0;

        static partial void CheckID(World world)
        {
            if (world.ID < 0 || world.ID >= MAX_WORLDS) //search free ID
            {
                int id;

                while (freeIDs.Count > 0)
                {
                    id = freeIDs[0];
                    freeIDs.RemoveAt(0);
                    if (!worlds.ContainsKey(id)) // because ServerScripts can set IDs manually
                    {
                        world.ID = id;
                        return;
                    }
                }

                while (true)
                    if (idCount >= MAX_WORLDS)
                    {
                        throw new Exception("WorldCollection reached maximum! " + MAX_WORLDS);
                    }
                    else
                    {
                        id = idCount++;
                        if (!worlds.ContainsKey(id))
                        {
                            world.ID = id;
                            return;
                        }
                    }
            }
        }
    }
}
