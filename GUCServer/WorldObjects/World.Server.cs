using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Enumeration;
using GUC.Network;
using GUC.Types;
using RakNet;
using GUC.Server.WorldObjects.Cells;

namespace GUC.WorldObjects
{
    public partial class World
    {
        #region Players

        DynamicCollection<GameClient> clients = new DynamicCollection<GameClient>();

        internal void AddToPlayers(GameClient client)
        {
            clients.Add(client, ref client.dynID);
        }

        internal void RemoveFromPlayers(GameClient client)
        {
            clients.Remove(ref client.dynID);
        }

        public void ForEachClient(Action<GameClient> action)
        {
            clients.ForEach(action);
        }

        #endregion

        
        public override void Update()
        {
            throw new NotImplementedException();
        }

        internal Dictionary<int, NetCell> netCells = new Dictionary<int, NetCell>();
        internal Dictionary<int, NPCCell> npcCells = new Dictionary<int, NPCCell>();

        partial void pAddVob(BaseVob vob)
        {
            // find the cell for this vob
            int[] coords = vob.GetNetCellCoords();
            NetCell cell = GetCellFromCoords(coords[0], coords[1]);
            vob.AddToNetCell(cell);
        }

        partial void pRemoveVob(BaseVob vob)
        {
            vob.RemoveFromNetCell();
        }

        #region WorldCells

        internal bool TryGetCellFromCoords(int x, int y, out NetCell cell)
        {
            if (x < short.MinValue || x > short.MaxValue || y < short.MinValue || y > short.MaxValue)
            {
                throw new Exception("Coords are out of cell range!");
            }

            return this.netCells.TryGetValue((x << 16) | y & 0xFFFF, out cell);
        }

        internal NetCell GetCellFromCoords(int x, int y)
        {
            NetCell cell;
            if (!TryGetCellFromCoords(x, y, out cell))
            {
                //create the new cell
                cell = new NetCell(this, x, y);
                netCells.Add((x << 16) | y & 0xFFFF, cell);
            }
            
            return cell;
        }


        internal void ForEachSurroundingCell(int x, int y, Action<NetCell> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                {
                    NetCell cell;
                    if (this.TryGetCellFromCoords(i, j, out cell))
                    {
                        action(cell);
                    }
                }
        }

        #endregion
    }
}
