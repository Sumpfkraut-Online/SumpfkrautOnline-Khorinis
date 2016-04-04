using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.Server.WorldObjects.Cells
{
    /// <summary>
    /// Big cell which includes all types of vobs of a world. Is used to transmit surrounding vobs to a client.
    /// </summary>
    class NetCell : WorldCell
    {
        public readonly VobTypeCollection<BaseVob> Vobs = new VobTypeCollection<BaseVob>();
        public readonly DynamicCollection<GameClient> Clients = new DynamicCollection<GameClient>();

        public readonly VobTypeCollection<BaseVob> DynVobs = new VobTypeCollection<BaseVob>();

        // NetCell[] surroundingCells = new NetCell[9];

        public NetCell(World world, int x, int y) : base(world, x, y)
        {
        }

        public void ForEachSurroundingClient(Action<GameClient> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int x = this.x - 1; x <= this.x + 1; x++)
            {
                if (x < short.MinValue || x > short.MaxValue)
                    continue;

                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    if (y < short.MinValue || y > short.MaxValue)
                        continue;
                    NetCell cell;
                    if (this.world.TryGetCellFromCoords(x, y, out cell))
                    {
                        cell.Clients.ForEach(client => action(client));
                    }
                }
            }
        }

        public void ForEachSurroundingCell(Action<NetCell> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int x = this.x - 1; x <= this.x + 1; x++)
            {
                if (x < short.MinValue || x > short.MaxValue)
                    continue;
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    if (y < short.MinValue || y > short.MaxValue)
                        continue;
                    NetCell cell;
                    if (this.world.TryGetCellFromCoords(x, y, out cell))
                    {
                        action(cell);
                    }
                }
            }
        }

        public const int NumSurroundingCells = 9;
    }
}
