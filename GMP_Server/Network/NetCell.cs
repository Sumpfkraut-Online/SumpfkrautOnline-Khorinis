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

        public void AddVob(Vob vob)
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

        public void ForEachSurroundingClient(Action<Client> action, Client exclude = null)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, NetCell> row;
                if (world.netCells.TryGetValue(i, out row))
                    for (int j = z - 1; j <= z + 1; j++)
                    {
                        NetCell c;
                        if (row.TryGetValue(j, out c))
                            Vobs.ForEachPlayer(p =>
                            {
                                if (p.Client != exclude)
                                    action(p.Client);
                            });
                    }
            }
        }
    }
}
