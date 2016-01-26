using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Server.Network.Messages;
using GUC.Types;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class World : WorldObject
    {
        internal Dictionary<int, Dictionary<int, WorldCell>> netCells = new Dictionary<int, Dictionary<int, WorldCell>>(); // for networking
        internal Dictionary<int, Dictionary<int, NPCCell>> npcCells = new Dictionary<int, Dictionary<int, NPCCell>>(); // small cells containing npcs for distance collections

        #region Spawn

        partial void pSpawnVob(Vob vob)
        {
            UpdatePosition(vob, vob is NPC ? ((NPC)vob).Client : null);
        }

        partial void pDespawnVob(Vob vob)
        {
            if (vob.cell != null)
            {
                vob.WriteDespawnMessage(vob.cell.SurroundingClients());
                vob.cell.RemoveVob(vob);
            }
        }

        #endregion

        #region WorldCells

        internal void UpdatePosition(Vob vob, Client exclude)
        {
            if (vob is NPC)
            {
                UpdateSmallCells((NPC)vob);
            }

            float unroundedX = vob.Position.X / WorldCell.cellSize;
            float unroundedZ = vob.Position.Z / WorldCell.cellSize;

            //calculate new cell indices
            int x = (int)(vob.Position.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(vob.Position.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            if (vob.cell == null)
            { //Vob has not been in the world yet
                ChangeCells(vob, x, z, exclude);
            }
            else
            {
                //vob moved to a new cell
                if (vob.cell.x != x || vob.cell.z != z)
                {
                    //check whether we're at least > 15% inside: 0.5f == between 2 cells
                    float xdiff = unroundedX - vob.cell.x;
                    float zdiff = unroundedZ - vob.cell.z;
                    if ((xdiff > 0.65f || xdiff < -0.65f) || (zdiff > 0.65f || zdiff < -0.65f))
                    {
                        ChangeCells(vob, x, z, exclude);
                        return;
                    }
                }

                //still in the old cell, updates for everyone!
                VobMessage.WritePosDir(vob.cell.SurroundingClients(exclude), vob);
            }
        }

        void UpdateSmallCells(NPC vob)
        {
            float unroundedX = vob.Position.X / NPCCell.cellSize;
            float unroundedZ = vob.Position.Z / NPCCell.cellSize;

            //calculate new cell indices
            int x = (int)(vob.Position.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(vob.Position.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            if (vob.npcCell != null)
            {
                if (vob.npcCell.x == x && vob.npcCell.z == z)
                    return;

                vob.npcCell.Remove(vob);
            }

            //find new cell
            Dictionary<int, NPCCell> row = null;
            npcCells.TryGetValue(x, out row);
            if (row == null)
            {
                row = new Dictionary<int, NPCCell>();
                npcCells.Add(x, row);
            }

            NPCCell cell = null;
            row.TryGetValue(z, out cell);
            if (cell == null)
            {
                cell = new NPCCell(this, x, z);
                row.Add(z, cell);
            }

            cell.Add(vob);
        }

        void ChangeCells(Vob vob, int x, int z, Client exclude)
        {
            //create the new cell
            Dictionary<int, WorldCell> row = null;
            netCells.TryGetValue(x, out row);
            if (row == null)
            {
                row = new Dictionary<int, WorldCell>();
                netCells.Add(x, row);
            }

            WorldCell newCell = null;
            row.TryGetValue(z, out newCell);
            if (newCell == null)
            {
                newCell = new WorldCell(this, x, z);
                row.Add(z, newCell);
            }

            if (vob.cell == null)
            {
                if (exclude != null)
                {
                    VobMessage.WritePosDir(new Client[1] { exclude }, vob);
                }
                //Send creation message to all players in surrounding cells
                vob.WriteSpawnMessage(newCell.SurroundingClients(exclude));

                //send all vobs in surrounding cells to the player
                if (vob is NPC && ((NPC)vob).IsPlayer)
                {
                    foreach (WorldCell c in newCell.SurroundingCells())
                    {
                        foreach (Vob v in c.Vobs.GetAll())
                        {
                            v.WriteSpawnMessage(new Client[1] { ((NPC)vob).Client });
                        }
                    }
                }
            }
            else
            {
                VobChangeDiffCells(vob, vob.cell, newCell, exclude);
                vob.cell.RemoveVob(vob);
            }

            newCell.AddVob(vob);
            /*
            #region vob controlling
            if (vob is NPC && ((NPC)vob).IsPlayer)
            {
                foreach (AbstractCtrlVob ctrl in new List<AbstractCtrlVob>(((NPC)vob).Client.VobControlledList))
                {
                    ctrl.FindNewController();
                }
                foreach (WorldCell c in newCell.SurroundingCells())
                    foreach (Vob v in c.AllVobs())
                        if (v is AbstractCtrlVob)
                        {
                            if (v is NPC && ((NPC)v).IsPlayer)
                                continue;

                            if (v is AbstractDropVob && !((AbstractDropVob)v).physicsEnabled)
                                continue;

                            ((AbstractCtrlVob)v).FindNewController();
                        }
            }
            else if (vob is AbstractCtrlVob)
            {
                ((AbstractCtrlVob)vob).FindNewController();
            }
            #endregion
            */
        }

        void VobChangeDiffCells(Vob vob, WorldCell from, WorldCell to, Client exclude)
        {
            int i, j;
            WorldCell cell;
            Dictionary<int, WorldCell> row;

            for (i = from.x - 1; i <= from.x + 1; i++)
            {
                row = null;
                netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (j = from.z - 1; j <= from.z + 1; j++)
                {
                    cell = null;
                    row.TryGetValue(j, out cell);
                    if (cell == null) continue;

                    if (i <= to.x + 1 && i >= to.x - 1 && j <= to.z + 1 && j >= to.z - 1)
                    {
                        //Position updates in shared cells
                        VobMessage.WritePosDir(cell.GetClients(exclude), vob);
                    }
                    else
                    {
                        //deletion updates in old cells
                        vob.WriteDespawnMessage(cell.GetClients());

                        //deletion updates for the player
                        if (vob is NPC && ((NPC)vob).IsPlayer)
                        {
                            foreach (Vob v in cell.Vobs.GetAll())
                            {
                                v.WriteDespawnMessage(new Client[1] { ((NPC)vob).Client });
                            }
                        }
                    }
                }
            }

            for (i = to.x - 1; i <= to.x + 1; i++)
            {
                row = null;
                netCells.TryGetValue(i, out row);
                if (row == null) continue;

                for (j = to.z - 1; j <= to.z + 1; j++)
                {
                    cell = null;
                    row.TryGetValue(j, out cell);
                    if (cell == null) continue;

                    if (i <= from.x + 1 && i >= from.x - 1 && j <= from.z + 1 && j >= from.z - 1)
                        continue;

                    if (exclude != null)
                    {
                        VobMessage.WritePosDir(new Client[1] { exclude }, vob);
                    }

                    //creation updates in the new cells
                    vob.WriteSpawnMessage(cell.GetClients(exclude));

                    //creation updates for the player
                    if (vob is NPC && ((NPC)vob).IsPlayer)
                    {
                        foreach (Vob v in cell.Vobs.GetAll())
                        {
                            v.WriteSpawnMessage(new Client[1] { ((NPC)vob).Client });
                        }
                    }
                }
            }
        }

        #endregion

        public IEnumerable<NPC> GetNPCs(Vec3f pos, float range)
        {
            float unroundedMinX = (pos.X - range) / NPCCell.cellSize;
            float unroundedMinZ = (pos.Z - range) / NPCCell.cellSize;

            int minX = (int)(pos.X >= 0 ? unroundedMinX + 0.5f : unroundedMinX - 0.5f);
            int minZ = (int)(pos.Z >= 0 ? unroundedMinZ + 0.5f : unroundedMinZ - 0.5f);

            float unroundedMaxX = (pos.X + range) / NPCCell.cellSize;
            float unroundedMaxZ = (pos.Z + range) / NPCCell.cellSize;

            int maxX = (int)(pos.X >= 0 ? unroundedMaxX + 0.5f : unroundedMaxX - 0.5f);
            int maxZ = (int)(pos.Z >= 0 ? unroundedMaxZ + 0.5f : unroundedMaxZ - 0.5f);

            for (int x = minX; x <= maxX; x++)
            {
                Dictionary<int, NPCCell> row = null;
                npcCells.TryGetValue(x, out row);
                if (row != null)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        NPCCell cell = null;
                        row.TryGetValue(z, out cell);
                        if (cell != null)
                        {
                            foreach (NPC npc in cell.Npcs.GetAll())
                                if (npc.Position.GetDistance(pos) <= range)
                                    yield return npc;
                        }
                    }
                }
            }
        }
    }
}
