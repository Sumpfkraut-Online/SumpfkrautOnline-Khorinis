using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Server.WorldObjects.Cells;
using GUC.Network;
using GUC.Server.Network;
using GUC.Enumeration;
using RakNet;

namespace GUC.WorldObjects
{
    public partial class BaseVob
    {

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public void SetPosition(Vec3f pos)
        {
            throw new NotImplementedException();
        }

        public Vec3f GetPosition()
        {
            return this.pos;
        }

        public void SetDirection(Vec3f dir)
        {
            Vec3f d = dir.IsNull() ? new Vec3f(0, 0, 1) : dir;
            throw new NotImplementedException();
        }

        public Vec3f GetDirection()
        {
            return this.dir;
        }

        #region Cells

        internal virtual void UpdatePosition(Vec3f newPos, Vec3f newDir, GameClient exclude)
        {
            float unroundedX = newPos.X / NetCell.Size;
            float unroundedZ = newPos.Z / NetCell.Size;

            // calculate new cell indices
            int x = (int)(newPos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(newPos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            if (x < short.MinValue || x > short.MaxValue || z < short.MinValue || z > short.MaxValue)
            {
                throw new Exception("Vob position is out of cell range!");
            }

            int coord = (x << 16) | z & 0xFFFF;

            this.pos = newPos;
            this.dir = newDir;

            // Vob moved to a new cell
            if (this.Cell.Coord != coord)
            {
                // Check whether we're at least > 15% inside: 0.5f == between 2 cells
                float xdiff = unroundedX - this.Cell.X;
                float zdiff = unroundedZ - this.Cell.Y;
                if ((xdiff > 0.65f || xdiff < -0.65f) || (zdiff > 0.65f || zdiff < -0.65f))
                {
                    Log.Logger.Log("change cell to " + x + " " + z);

                    ChangeCells(x, z);
                    return;
                }
            }

            // still in the old cell, updates for everyone!
            if (this.Cell.Clients.Count > 0)
            {
                PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                stream.Write((ushort)this.ID);
                stream.Write(this.pos);
                stream.Write(this.dir);
                this.Cell.ForEachSurroundingClient(c =>
                {
                    if (c != exclude)
                    {
                        c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
                    }
                });
            }
        }

        internal virtual void ChangeCells(int toX, int toY)
        {
            NetCell from = this.Cell;
            this.RemoveFromNetCell();

            from.ForEachSurroundingCell(cell =>
            {
                if (cell.Clients.Count > 0)
                    if (cell.X <= toX + 1 && cell.X >= toX - 1 && cell.Y <= toY + 1 && cell.Y >= toY - 1)
                    {
                        //Position updates in shared cells
                        PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                        stream.Write((ushort)this.ID);
                        stream.Write(this.pos);
                        stream.Write(this.dir);
                        cell.Clients.ForEach(c =>
                        {
                            c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
                        });
                    }
                    else
                    {
                        //deletion updates in old cells
                        PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                        stream.Write((ushort)this.ID);
                        cell.Clients.ForEach(c =>
                        {
                            c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                        });
                    }
            });

            // new cells
            this.world.ForEachSurroundingCell(toX, toY, cell =>
            {
                if (cell.Clients.Count > 0)
                    if (!(cell.X <= from.X + 1 && cell.X >= from.X - 1 && cell.Y <= from.Y + 1 && cell.Y >= from.Y - 1))
                    {
                        // spawn updates in the new cells
                        PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
                        stream.Write((byte)this.VobType);
                        this.WriteStream(stream);
                        cell.Clients.ForEach(c =>
                        {
                            c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                        });
                    }
            });

            this.AddToNetCell(this.world.GetCellFromCoords(toX, toY));
        }

        internal int cellID = -1;
        internal int cellTypeID = -1;

        internal int dynCellID = -1;
        internal int dynCellTypeID = -1;

        internal NetCell Cell = null;

        internal virtual void AddToNetCell(NetCell cell)
        {
            cell.Vobs.Add(this, ref this.cellID, ref this.cellTypeID);
            if (!this.IsStatic)
                cell.DynVobs.Add(this, ref this.dynCellID, ref this.dynCellTypeID);

            this.Cell = cell;
        }

        internal virtual void RemoveFromNetCell()
        {
            this.Cell.Vobs.Remove(this, ref this.cellID, ref this.cellTypeID);
            if (!this.IsStatic)
                this.Cell.DynVobs.Remove(this, ref this.dynCellID, ref this.dynCellTypeID);

            if (this.Cell.Vobs.GetCount() == 0)
                this.world.netCells.Remove(this.Cell.Coord);

            this.Cell = null;
        }

        partial void pSpawn()
        {
            //Send creation message to all players in surrounding cells
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldSpawnMessage);
            stream.Write((byte)this.VobType);
            this.WriteStream(stream);
            this.Cell.ForEachSurroundingClient(c =>
            {
                c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
            });
        }

        partial void pDespawn()
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
            stream.Write((ushort)this.ID);
            this.Cell.ForEachSurroundingClient(c =>
            {
                c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
            });
        }

        #endregion



        public PacketWriter GetScriptVobStream()
        {
            if (!this.IsSpawned)
                throw new Exception("Vob is not ingame!");

            var strm = GameServer.SetupStream(NetworkIDs.ScriptVobMessage);
            strm.Write((ushort)this.ID);

            return strm;
        }

        public void SendScriptVobStream(PacketWriter stream)
        {
            if (stream == null)
                throw new Exception("Stream is null!");

            if (!this.IsSpawned)
                throw new Exception("Vob is not ingame!");

            this.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }
    }
}
