using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects.Cells;
using GUC.GameObjects.Collections;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class World
    {
        #region Network Messages

        internal static class Messages
        {
            #region Load & Leave

            public static void WriteLoadWorld(GameClient client, World world)
            {
                client.Loading = true;
                PacketWriter stream = GameServer.SetupStream(ServerMessages.LoadWorldMessage);
                world.WriteStream(stream);
                stream.Write(world.Clock.IsRunning);
                client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, '\0');
            }

            public static void WriteLeaveWorld(GameClient client)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldLeaveMessage);
                client.Send(stream, NetPriority.Low, NetReliability.ReliableOrdered, 'W');
            }

            #endregion
        }

        #endregion

        #region Spawn ranges

        static float spawnInsertRange = 7000; //8000;
        static float spawnRemoveRange = 8000; //10000;

        public static float SpawnInsertRange
        {
            get { return spawnInsertRange; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Value is must be greater than zero! Is " + value);

                if (value > spawnRemoveRange)
                    throw new ArgumentOutOfRangeException("Value must be smaller than RemoveRange! Is " + value);

                throw new NotImplementedException();
            }
        }

        public static float SpawnRemoveRange
        {
            get { return spawnRemoveRange; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Value is must be greater than zero! Is " + value);

                if (value < spawnInsertRange)
                    throw new ArgumentOutOfRangeException("Value must be greater than InsertRange! Is " + value);

                throw new NotImplementedException();
            }
        }

        #endregion
        
        #region Clients

        DynamicCollection<GameClient> clients = new DynamicCollection<GameClient>();

        #region Add & Remove

        internal void AddClient(GameClient client)
        {
            clients.Add(client, ref client.dynID);
        }

        internal void RemoveClient(GameClient client)
        {
            clients.Remove(ref client.dynID);
        }

        #endregion

        #region Access

        public void ForEachClient(Action<GameClient> action)
        {
            clients.ForEach(action);
        }

        public void ForEachClientPredicate(Predicate<GameClient> predicate)
        {
            clients.ForEachPredicate(predicate);
        }

        public int ClientCount { get { return clients.Count; } }

        #endregion

        #endregion

        #region Big cells

        Dictionary<int, BigCell> cells = new Dictionary<int, BigCell>();

        // for spectators
        internal void UpdateSpectatorCell(GameClient client, Vec3f pos)
        {
            if (client == null)
                throw new ArgumentNullException("Client is null!");

            Vec2i coords = BigCell.GetCoords(pos);
            if (coords.X != client.SpecCell.X || coords.Y != client.SpecCell.Y)
            {
                client.SpecCell.RemoveClient(client);
                CheckCellRemove(client.SpecCell);

                int coord = BigCell.GetCoordinate(coords.X, coords.Y);
                BigCell cell;
                if (!cells.TryGetValue(coord, out cell))
                {
                    cell = new BigCell(this, coords.X, coords.Y);
                    cells.Add(coord, cell);
                }
                cell.AddClient(client);
                client.SpecCell = cell;
            }
        }

        internal void UpdateVobCell(GUCBaseVobInst vob, Vec3f pos)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            Vec2i coords = BigCell.GetCoords(pos);
            if (coords.X != vob.Cell.X || coords.Y != vob.Cell.Y)
            {
                vob.RemoveFromCell();

                int coord = BigCell.GetCoordinate(coords.X, coords.Y);
                BigCell cell;
                if (!cells.TryGetValue(coord, out cell))
                {
                    cell = new BigCell(this, coords.X, coords.Y);
                    cells.Add(coord, cell);
                }

                vob.AddToCell(cell);
            }
        }

        #region Add & Remove

        internal void CheckCellRemove(BigCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException("Cell is null!");

            if (cell.DynVobCount == 0 && cell.ClientCount == 0)
            {
                cells.Remove(cell.Coord);
            }
        }

        internal void AddSpectatorToCells(GameClient client)
        {
            if (client == null)
                throw new ArgumentNullException("Client is null!");

            Vec2i coords = BigCell.GetCoords(client.SpecGetPos());
            int coord = BigCell.GetCoordinate(coords.X, coords.Y);
            BigCell cell;
            if (!cells.TryGetValue(coord, out cell))
            {
                cell = new BigCell(this, coords.X, coords.Y);
                cells.Add(coord, cell);
            }
            cell.AddClient(client);
            client.SpecCell = cell;
        }

        internal void RemoveSpectatorFromCells(GameClient client)
        {
            if (client == null)
                throw new ArgumentNullException("Client is null!");

            client.SpecCell.RemoveClient(client);
            CheckCellRemove(client.SpecCell);
            client.SpecCell = null;
        }

        partial void pAfterAddVob(GUCBaseVobInst vob)
        {
            if (!vob.IsStatic)
            {
                // find the cell for this vob
                Vec2i coords = BigCell.GetCoords(vob.Position);
                int coord = BigCell.GetCoordinate(coords.X, coords.Y);
                BigCell cell;
                if (!cells.TryGetValue(coord, out cell))
                {
                    cell = new BigCell(this, coords.X, coords.Y);
                    cells.Add(coord, cell);
                }

                vob.AddToCell(cell);
            }
        }

        partial void pBeforeRemoveVob(GUCBaseVobInst vob)
        {
            if (!vob.IsStatic)
            {
                vob.RemoveFromCell();
            }
        }

        #endregion

        #region Access

        #region Dynamic Vobs

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachDynVobRougher(GUCBaseVobInst vob, float radius, Action<GUCBaseVobInst> action)
        {
            if (vob == null) throw new ArgumentNullException("Vob is null!");
            this.ForEachDynVobRougher(vob.Position, radius, action);
        }

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachDynVobRougher(Vec3f pos, float radius, Action<GUCBaseVobInst> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            Vec2i min = BigCell.GetCoords(new Vec3f(pos.X - radius, pos.Y, pos.Z - radius));
            Vec2i max = BigCell.GetCoords(new Vec3f(pos.X + radius, pos.Y, pos.Z + radius));
            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    BigCell cell;
                    if (cells.TryGetValue(BigCell.GetCoordinate(x, y), out cell))
                    {
                        cell.ForEachDynVob(action);
                    }
                }
            }
        }

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachDynVobRougherPredicate(GUCBaseVobInst vob, float radius, Predicate<GUCBaseVobInst> predicate)
        {
            if (vob == null) throw new ArgumentNullException("Vob is null!");
            this.ForEachDynVobRougherPredicate(vob.Position, radius, predicate);
        }

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachDynVobRougherPredicate(Vec3f pos, float radius, Predicate<GUCBaseVobInst> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            Vec2i min = BigCell.GetCoords(new Vec3f(pos.X - radius, pos.Y, pos.Z - radius));
            Vec2i max = BigCell.GetCoords(new Vec3f(pos.X + radius, pos.Y, pos.Z + radius));
            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    BigCell cell;
                    if (cells.TryGetValue(BigCell.GetCoordinate(x, y), out cell))
                    {
                        cell.ForEachDynVobPredicate(predicate);
                    }
                }
            }
        }

        #endregion

        #region Clients

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachClientRougher(GUCBaseVobInst vob, float radius, Action<GameClient> action)
        {
            if (vob == null) throw new ArgumentNullException("Vob is null!");
            this.ForEachClientRougher(vob.Position, radius, action);
        }

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachClientRougher(Vec3f pos, float radius, Action<GameClient> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            Vec2i min = BigCell.GetCoords(new Vec3f(pos.X - radius, pos.Y, pos.Z - radius));
            Vec2i max = BigCell.GetCoords(new Vec3f(pos.X + radius, pos.Y, pos.Z + radius));
            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    BigCell cell;
                    if (cells.TryGetValue(BigCell.GetCoordinate(x, y), out cell))
                    {
                        cell.ForEachClient(action);
                    }
                }
            }
        }

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachClientRougherPredicate(GUCBaseVobInst vob, float radius, Predicate<GameClient> predicate)
        {
            if (vob == null) throw new ArgumentNullException("Vob is null!");
            this.ForEachClientRougherPredicate(vob.Position, radius, predicate);
        }

        /// <summary>
        /// 5000 units accuracy
        /// </summary>
        public void ForEachClientRougherPredicate(Vec3f pos, float radius, Predicate<GameClient> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            Vec2i min = BigCell.GetCoords(new Vec3f(pos.X - radius, pos.Y, pos.Z - radius));
            Vec2i max = BigCell.GetCoords(new Vec3f(pos.X + radius, pos.Y, pos.Z + radius));
            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    BigCell cell;
                    if (cells.TryGetValue(BigCell.GetCoordinate(x, y), out cell))
                    {
                        cell.ForEachClientPredicate(predicate);
                    }
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region NPC Cells

        Dictionary<int, NPCCell> npcCells = new Dictionary<int, NPCCell>();

        internal void UpdateNPCCell(GUCNPCInst npc, Vec3f pos)
        {
            if (npc == null)
                throw new ArgumentNullException("NPC is null!");

            Vec2i coords = NPCCell.GetCoords(pos);
            if (coords.X != npc.NpcCell.X || coords.Y != npc.NpcCell.Y)
            {
                npc.NpcCell.RemoveNPC(npc);
                CheckCellRemove(npc.NpcCell);

                int coord = NPCCell.GetCoordinate(coords.X, coords.Y);
                NPCCell cell;
                if (!npcCells.TryGetValue(coord, out cell))
                {
                    cell = new NPCCell(this, coords.X, coords.Y);
                    npcCells.Add(coord, cell);
                }
                cell.AddNPC(npc);
                npc.NpcCell = cell;
            }
        }

        #region Add & Remove

        internal void CheckCellRemove(NPCCell cell)
        {
            if (cell.NPCCount == 0)
            {
                npcCells.Remove(cell.Coord);
            }
        }

        internal void AddToNPCCells(GUCNPCInst npc)
        {
            // find the cell for this npc
            Vec2i coords = NPCCell.GetCoords(npc.Position);
            int coord = NPCCell.GetCoordinate(coords.X, coords.Y);
            NPCCell cell;
            if (!npcCells.TryGetValue(coord, out cell))
            {
                cell = new NPCCell(this, coords.X, coords.Y);
                npcCells.Add(coord, cell);
            }
            cell.AddNPC(npc);
            npc.NpcCell = cell;
        }

        internal void RemoveFromNPCCells(GUCNPCInst npc)
        {
            npc.NpcCell.RemoveNPC(npc);
            CheckCellRemove(npc.NpcCell);
            npc.NpcCell = null;
        }

        #endregion

        #region Access

        /// <summary>
        /// 1000 ingame units accuracy
        /// </summary>
        public void ForEachNPCRough(GUCBaseVobInst vob, float radius, Action<GUCNPCInst> action)
        {
            if (vob == null)
                throw new ArgumentException("Vob is null!");
            this.ForEachNPCRough(vob.Position, radius, action);
        }

        /// <summary>
        /// 1000 ingame units accuracy
        /// </summary>
        public void ForEachNPCRough(Vec3f pos, float radius, Action<GUCNPCInst> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            Vec2i min = NPCCell.GetCoords(new Vec3f(pos.X - radius, pos.Y, pos.Z - radius));
            Vec2i max = NPCCell.GetCoords(new Vec3f(pos.X + radius, pos.Y, pos.Z + radius));
            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    NPCCell cell;
                    if (npcCells.TryGetValue(NPCCell.GetCoordinate(x, y), out cell))
                    {
                        cell.ForEachNPC(action);
                    }
                }
            }
        }

        /// <summary>
        /// 1000 ingame units accuracy
        /// </summary>
        public void ForEachNPCRoughPredicate(GUCBaseVobInst vob, float radius, Predicate<GUCNPCInst> predicate)
        {
            if (vob == null)
                throw new ArgumentException("Vob is null!");
            this.ForEachNPCRoughPredicate(vob.Position, radius, predicate);
        }

        /// <summary>
        /// 1000 ingame units accuracy
        /// </summary>
        public void ForEachNPCRoughPredicate(Vec3f pos, float radius, Predicate<GUCNPCInst> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            Vec2i min = NPCCell.GetCoords(new Vec3f(pos.X - radius, pos.Y, pos.Z - radius));
            Vec2i max = NPCCell.GetCoords(new Vec3f(pos.X + radius, pos.Y, pos.Z + radius));
            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    if (npcCells.TryGetValue(NPCCell.GetCoordinate(x, y), out NPCCell cell))
                    {
                        cell.ForEachNPCPredicate(predicate);
                    }
                }
            }
        }

        #endregion

        #endregion

        partial void pBeforeDelete()
        {
            ForEachClient(c => c.SetControl(null));
            ForEachVob(v => v.Despawn());
        }
    }
}
