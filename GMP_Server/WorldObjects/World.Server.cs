using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Enumeration;
using GUC.Network;
using GUC.Types;
using RakNet;

namespace GUC.WorldObjects
{
    public partial class World
    {
        internal Dictionary<int, Dictionary<int, NetCell>> netCells = new Dictionary<int, Dictionary<int, NetCell>>();
        internal Dictionary<int, Dictionary<int, NPCCell>> npcCells = new Dictionary<int, Dictionary<int, NPCCell>>();

        partial void pAddVob(BaseVob vob)
        {
            // find the cell for this vob
            Vec3f pos = vob.GetPosition();
            float unroundedX = pos.X / NetCell.cellSize;
            float unroundedZ = pos.Z / NetCell.cellSize;
            int x = (int)(pos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(pos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            GetCellFromCoords(x, z).AddVob(vob);

            //Send creation message to all players in surrounding cells
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
            stream.Write((byte)vob.VobType);
            vob.WriteStream(stream);
            vob.Cell.ForEachSurroundingClient(c =>
            {
                Log.Logger.Log("Send " + (c == null) + " " + (c == null ? "" : (c.guid == null).ToString()));
                Log.Logger.Log("To: " + c.guid.g + " " + c.SystemAddress);
                c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
            }, vob is NPC ? ((NPC)vob).Client : null);
            
            if (vob is NPC && ((NPC)vob).IsPlayer) //send all vobs in surrounding cells to the player
            {
                vob.Cell.ForEachSurroundingCell(c =>
                {
                    c.Vobs.ForEachDynamic(v =>
                    {
                        stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                        stream.Write((byte)v.VobType);
                        v.WriteStream(stream);
                        ((NPC)vob).Client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                    });
                });
            }
        }

        partial void pDespawnVob(BaseVob vob)
        {
            if (vob.Cell != null)
            {
                PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                stream.Write(vob.ID);
                vob.Cell.ForEachSurroundingClient(c =>
                {
                    c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                });
                vob.Cell.RemoveVob(vob);
            }
        }

        #region WorldCells

        NetCell GetCellFromCoords(int x, int z)
        {
            //create the new cell
            Dictionary<int, NetCell> row = null;
            if (!netCells.TryGetValue(x, out row))
            {
                row = new Dictionary<int, NetCell>();
                netCells.Add(x, row);
            }

            NetCell cell = null;
            if (!row.TryGetValue(z, out cell))
            {
                cell = new NetCell(this, x, z);
                row.Add(z, cell);
            }

            return cell;
        }

        internal void UpdatePosition(BaseVob vob, GameClient exclude)
        {
            Vec3f pos = vob.GetPosition();

            float unroundedX = pos.X / NetCell.cellSize;
            float unroundedZ = pos.Z / NetCell.cellSize;

            // calculate new cell indices
            int x = (int)(pos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(pos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            if (vob.Cell == null)
            { // Vob has not been in the world yet
                ChangeCells(vob, x, z, exclude);
            }
            else
            {
                // Vob moved to a new cell
                if (vob.Cell.x != x || vob.Cell.z != z)
                {
                    // Check whether we're at least > 15% inside: 0.5f == between 2 cells
                    float xdiff = unroundedX - vob.Cell.x;
                    float zdiff = unroundedZ - vob.Cell.z;
                    if ((xdiff > 0.65f || xdiff < -0.65f) || (zdiff > 0.65f || zdiff < -0.65f))
                    {
                        ChangeCells(vob, x, z, exclude);
                        return;
                    }
                }

                // still in the old cell, updates for everyone!
                PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                stream.Write(vob.ID);
                stream.Write(vob.pos);
                stream.Write(vob.dir);
                vob.Cell.ForEachSurroundingClient(c =>
                {
                    c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
                }, exclude);
            }
        }

        void ChangeCells(BaseVob vob, int x, int z, GameClient exclude)
        {
            NetCell newCell = GetCellFromCoords(x, z);

            if (vob.Cell == null) // freshly spawned
            {
                /*if (exclude != null)
                {
                    stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                    stream.Write(vob.ID);
                    stream.Write(vob.pos);
                    stream.Write(vob.dir);
                    exclude.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
                }*/

                //Send creation message to all players in surrounding cells
                PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                stream.Write((byte)vob.VobType);
                vob.WriteStream(stream);
                newCell.ForEachSurroundingClient(c =>
                {
                    c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                }, exclude);

                //send all vobs in surrounding cells to the player
                if (vob is NPC && ((NPC)vob).IsPlayer)
                {
                    newCell.ForEachSurroundingCell(c =>
                    {
                        c.Vobs.ForEachDynamic(v =>
                        {
                            stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                            stream.Write((byte)v.VobType);
                            v.WriteStream(stream);
                            ((NPC)vob).Client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                        });
                    });
                }
            }
            else
            {
                VobChangeDiffCells(vob, vob.Cell, newCell, exclude);
                vob.Cell.RemoveVob(vob);
            }

            newCell.AddVob(vob);
        }

        void VobChangeDiffCells(BaseVob vob, NetCell from, NetCell to, GameClient exclude)
        {
            PacketWriter stream;
            int i, j;
            NetCell cell;
            Dictionary<int, NetCell> row;

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
                        stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                        stream.Write(vob.ID);
                        stream.Write(vob.pos);
                        stream.Write(vob.dir);
                        cell.ForEachClient(c =>
                        {
                            c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
                        }, exclude);
                    }
                    else
                    {
                        //deletion updates in old cells
                        stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                        stream.Write(vob.ID);
                        cell.ForEachClient(c =>
                        {
                            c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                        }, exclude);

                        //deletion updates for the player
                        if (vob is NPC && ((NPC)vob).IsPlayer)
                        {
                            cell.Vobs.ForEachDynamic(v =>
                            {
                                stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                                stream.Write(v.ID);
                                ((NPC)vob).Client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                            });
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

                    /*if (exclude != null)
                    {
                        stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                        stream.Write(vob.ID);
                        stream.Write(vob.pos);
                        stream.Write(vob.dir);
                        exclude.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
                    }*/

                    //creation updates in the new cells
                    stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                    stream.Write((byte)vob.VobType);
                    vob.WriteStream(stream);
                    cell.ForEachClient(c =>
                    {
                        c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                    }, exclude);

                    //creation updates for the player
                    if (vob is NPC && ((NPC)vob).IsPlayer)
                    {
                        cell.Vobs.ForEachDynamic(v =>
                        {
                            stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                            stream.Write((byte)v.VobType);
                            v.WriteStream(stream);
                            ((NPC)vob).Client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
                        });
                    }
                }
            }
        }

        #endregion

        #region Network Messages

        internal static void SendWorldMessage(GameClient client, World world)
        {
            if (client == null)
                throw new ArgumentNullException("GameClient is null!");
            if (world == null)
                throw new ArgumentNullException("World is null!");

            PacketWriter stream = GameServer.SetupStream(NetworkIDs.LoadWorldMessage);
            world.WriteStream(stream);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
        }

        #endregion
    }
}
