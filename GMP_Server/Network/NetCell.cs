using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Collections;

namespace GUC.Server.Network
{
    internal class NetCell
    {
        public const float cellSize = 5000;

        public World world;
        public int x, z;

        public readonly VobCollection Vobs = new VobCollection();

        public NetCell(World world, int x, int z)
        {
            this.world = world;
            this.x = x;
            this.z = z;
        }

        /*public void AddVob(Vob vob)
        {
            Vobs.Add(vob);
            vob.Cell = this;
        }

        public void RemoveVob(Vob vob)
        {
            Vobs.Remove(vob);
            vob.Cell = null;

            if (Vobs.GetCount() == 0)
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
            foreach(NPC player in Vobs.Players.GetAll())
            {
                yield return player.Client;
            }
        }

        public IEnumerable<Client> SurroundingClients()
        {
            return SurroundingClients(null);
        }

        public IEnumerable<Client> SurroundingClients(Client exclude)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, NetCell> row;
                world.netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (int j = z - 1; j <= z + 1; j++)
                {
                    NetCell c;
                    row.TryGetValue(j, out c);
                    if (c == null) continue;

                    foreach (NPC player in Vobs.Players.GetAll())
                    {
                        if (player.Client != exclude)
                            yield return player.Client;
                    }
                }
            }
        }

        public IEnumerable<NPC> SurroundingPlayers()
        {
            return SurroundingPlayers(null);
        }

        public IEnumerable<NPC> SurroundingPlayers(NPC exclude)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, NetCell> row;
                world.netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (int j = z - 1; j <= z + 1; j++)
                {
                    NetCell c;
                    row.TryGetValue(j, out c);
                    if (c == null) continue;

                    foreach (NPC player in Vobs.Players.GetAll())
                    {
                        if (player != exclude)
                            yield return player;
                    }
                }
            }
        }

        public IEnumerable<NetCell> SurroundingCells()
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, NetCell> row;
                world.netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (int j = z - 1; j <= z + 1; j++)
                {
                    NetCell c;
                    row.TryGetValue(j, out c);
                    if (c == null) continue;

                    yield return c;
                }
            }
        }*/
    }
}
