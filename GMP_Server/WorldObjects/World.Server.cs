using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Server.Network.Messages;
using GUC.Types;
using GUC.WorldObjects.Collections;
using GUC.Log;

namespace GUC.WorldObjects
{
    public partial class World
    {
        public const int MAX_WORLDS = 256; // byte + 1

        #region Static world collection
        static readonly World[] worlds = new World[MAX_WORLDS];
        public static World GetWorldByID(int id)
        {
            if (id >= 0 && id < MAX_WORLDS)
                return worlds[id];
            return null;
        }

        public static void AddWorld(World world)
        {
            if (world == null)
            {
                throw new ArgumentNullException("World.AddWorld: World is null!");
            }

            if (world.ID >= 0 && world.ID < MAX_WORLDS) // world has already a legit ID?
            {
                World slot = worlds[world.ID];
                if (slot != null) // world ID is already taken?
                {
                    if (slot == world) // same world
                    {
                        Logger.LogWarning("World.AddWorld: World is already added!");
                        return;
                    }
                    else // different world!
                    {
                        throw new Exception("World.AddWorld: ID is already taken!");
                    }
                }
                worlds[world.ID] = world;
                return;
            }
            else // search a free ID
            {
                for (int i = 0; i < MAX_WORLDS; i++)
                    if (worlds[i] == null)
                    {
                        worlds[i] = world;
                        world.ID = i;
                        return;
                    }
            }
            throw new Exception("World.AddWorld: World maximum reached! " + MAX_WORLDS);
        }

        public static void RemoveWorld(World world)
        {
            if (world == null)
            {
                throw new ArgumentNullException("World.RemoveWorld: World is null!");
            }

            if (GetWorldByID(world.ID) == world)
            {
                worlds[world.ID] = null;
                world.ID = -1;
            }
            else
            {
                Logger.LogWarning("World.RemoveWorld: World is not added!");
            }
        }
        #endregion

        List<int> freeIDs = new List<int>();
        int idCount = 0;

        partial void pSpawnVob(BaseVob vob)
        {
            if (freeIDs.Count > 0)
            {
                vob.WorldID = freeIDs[0];
                freeIDs.RemoveAt(0);
            }
            else
            {
                for (int i = 0; i < MAX_VOBS; i++)
                {
                    if (vobs[idCount] == null)
                    {
                        vob.WorldID = idCount;
                        break;
                    }

                    idCount++;
                    if (idCount >= MAX_VOBS)
                        idCount = 0;
                }
            }
        }
    }
}
