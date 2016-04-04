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
            {
                if (i < short.MinValue || i > short.MaxValue)
                    continue;

                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j < short.MinValue || j > short.MaxValue)
                        continue;

                    NetCell cell;
                    if (this.TryGetCellFromCoords(i, j, out cell))
                    {
                        action(cell);
                    }
                }
            }
        }

        #endregion

        public void ForEachNPCInRange(BaseVob vob, float range, Action<NPC> action)
        {
            if (vob == null)
                throw new ArgumentException("Vob is null!");
            this.ForEachNPCInRange(vob.GetPosition(), range, action);
        }

        public void ForEachNPCInRange(Vec3f pos, float range, Action<NPC> action)
        {
            if (action == null)
                throw new ArgumentException("Action is null!");

            float unroundedMinX = (pos.X - range) / NPCCell.Size;
            float unroundedMinZ = (pos.Z - range) / NPCCell.Size;

            int minX = (int)(pos.X >= 0 ? unroundedMinX + 0.5f : unroundedMinX - 0.5f);
            int minZ = (int)(pos.Z >= 0 ? unroundedMinZ + 0.5f : unroundedMinZ - 0.5f);

            float unroundedMaxX = (pos.X + range) / NPCCell.Size;
            float unroundedMaxZ = (pos.Z + range) / NPCCell.Size;

            int maxX = (int)(pos.X >= 0 ? unroundedMaxX + 0.5f : unroundedMaxX - 0.5f);
            int maxZ = (int)(pos.Z >= 0 ? unroundedMaxZ + 0.5f : unroundedMaxZ - 0.5f);

            for (int x = minX; x <= maxX; x++)
            {
                if (x < short.MinValue || x > short.MaxValue)
                    continue;

                for (int z = minZ; z <= maxZ; z++)
                {
                    if (z < short.MinValue || z > short.MaxValue)
                        continue;

                    int coord = (x << 16) | z & 0xFFFF;
                    NPCCell npcCell;
                    if (npcCells.TryGetValue(coord, out npcCell))
                    {
                        npcCell.npcs.ForEach(npc =>
                        {
                            if (npc.GetPosition().GetDistance(pos) < range)
                                action(npc);
                        });
                    }
                }
            }
        }
    }
}
