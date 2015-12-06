using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using RakNet;
using GUC.Enumeration;
using System.Collections;

namespace GUC.Server.Network
{
    internal class WorldCell
    {
        public const float cellSize = 5000;

        public World world;
        public int x, z;
        public List<NPC> PlayerList;
        public List<NPC> NPCList;
        public List<Item> ItemList;
        public List<Vob> VobList;

        public WorldCell(World world, int x, int z)
        {
            this.world = world;
            PlayerList = new List<NPC>();
            NPCList = new List<NPC>();
            ItemList = new List<Item>();
            VobList = new List<Vob>();
            this.x = x;
            this.z = z;
        }

        public void AddVob(AbstractVob vob)
        {
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    PlayerList.Add((NPC)vob);
                else
                    NPCList.Add((NPC)vob);
            }
            else if (vob is Item)
            {
                ItemList.Add((Item)vob);
            }
            else if (vob is Vob)
            {
                VobList.Add((Vob)vob);
            }
            vob.cell = this;
        }

        public void RemoveVob(AbstractVob vob)
        {
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    PlayerList.Remove((NPC)vob);
                else
                    NPCList.Remove((NPC)vob);
            }
            else if (vob is Item)
            {
                ItemList.Remove((Item)vob);
            }
            else if (vob is Vob)
            {
                VobList.Remove((Vob)vob);
            }
            vob.cell = null;

            if (PlayerList.Count == 0 && NPCList.Count == 0 && ItemList.Count == 0 && VobList.Count == 0)
            {
                world.netCells[x].Remove(z);
                if (world.netCells[x].Count == 0)
                {
                    world.netCells.Remove(x);
                }
            }
        }

        public IEnumerable<Client> GetClients()
        {
            return GetClients(null);
        }

        public IEnumerable<Client> GetClients(Client exclude)
        {
            Client c;
            for (int i = 0; i < PlayerList.Count; i++)
            {
                c = PlayerList[i].client;
                if (c != exclude)
                    yield return c;
            }
            yield break;
        }

        public IEnumerable<Client> SurroundingClients()
        {
            return SurroundingClients(null);
        }

        public IEnumerable<Client> SurroundingClients(Client exclude)
        {
            Client next;
            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, WorldCell> row;
                world.netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (int j = z - 1; j <= z + 1; j++)
                {
                    WorldCell c;
                    row.TryGetValue(j, out c);
                    if (c == null) continue;

                    for (int p = 0; p < c.PlayerList.Count; p++)
                    {
                        next = c.PlayerList[p].client;
                        if (next != exclude)
                            yield return next;
                    }
                }
            }
            yield break;
        }

        public IEnumerable<NPC> SurroundingPlayers()
        {
            return SurroundingPlayers(null);
        }

        public IEnumerable<NPC> SurroundingPlayers(NPC exclude)
        {
            NPC next;
            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, WorldCell> row;
                world.netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (int j = z - 1; j <= z + 1; j++)
                {
                    WorldCell c;
                    row.TryGetValue(j, out c);
                    if (c == null) continue;

                    for (int p = 0; p < c.PlayerList.Count; p++)
                    {
                        next = c.PlayerList[p];
                        if (next != exclude)
                            yield return next;
                    }
                }
            }
            yield break;
        }

        public IEnumerable<WorldCell> SurroundingCells()
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, WorldCell> row;
                world.netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (int j = z - 1; j <= z + 1; j++)
                {
                    WorldCell c;
                    row.TryGetValue(j, out c);
                    if (c == null) continue;

                    yield return c;
                }
            }
            yield break;
        }

        public IEnumerable<AbstractVob> AllVobs()
        {
            for (int i = 0; i < PlayerList.Count; i++)
                yield return PlayerList[i];

            for (int i = 0; i < NPCList.Count; i++)
                yield return NPCList[i];

            for (int i = 0; i < ItemList.Count; i++)
                yield return ItemList[i];

            for (int i = 0; i < VobList.Count; i++)
                yield return VobList[i];

            yield break;
        }
    }
}
