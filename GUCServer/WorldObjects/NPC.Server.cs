using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Network;
using RakNet;
using GUC.Network;
using GUC.WorldObjects.Mobs;
using GUC.Types;
using GUC.Server.Network.Messages;
using GUC.Server.WorldObjects.Cells;
using GUC.Animations;

namespace GUC.WorldObjects
{
    public partial class NPC
    {
        #region ScriptObject

        public partial interface IScriptNPC
        {
            void OnCmdMove(MoveState state);
            void OnCmdUseMob(MobInter mob);
            void OnCmdUseItem(Item item);
            void OnCmdDrawItem(Item item);
            void OnCmdPickupItem(Item item);
            void OnCmdDropItem(Item item);
            void OnCmdEquipItem(int slot, Item item);
            void OnCmdUnequipItem(Item item);
            void OnCmdAniStart(Animation ani);
            void OnCmdAniStart(Animation ani, object[] netArgs);
            void OnCmdAniStop(bool fadeOut);
        }

        #endregion
        
        public void UpdatePropertiesFast()
        {

        }

        public void UpdateProperties()
        {
            // send msg
        }

        partial void pSetMovement(MoveState state)
        {
            if (this.isCreated)
                NPCMessage.WriteMoveState(this, state);
        }

        #region Cells

        internal NPCCell npcCell = null;
        internal int npcCellID = -1;
        
        void UpdateNPCCell()
        {
            int[] coords = NPCCell.GetCoords(this.pos);

            if (coords[0] < short.MinValue || coords[0] > short.MaxValue || coords[1] < short.MinValue || coords[1] > short.MaxValue)
            {
                throw new Exception("Coords are out of cell range!");
            }

            if (this.npcCell == null || this.npcCell.X != coords[0] || this.npcCell.Y != coords[1])
            {
                int coord = (coords[0] << 16) | coords[1] & 0xFFFF;

                if (this.npcCell != null)
                {
                    this.npcCell.npcs.Remove(ref this.npcCellID);
                    if (this.npcCell.npcs.Count <= 0)
                        this.world.npcCells.Remove(this.npcCell.Coord);
                }

                NPCCell newCell;
                if (!this.world.npcCells.TryGetValue(coord, out newCell))
                {
                    newCell = new NPCCell(this.world, coords[0], coords[1]);
                    this.world.npcCells.Add(coord, newCell);
                }

                newCell.npcs.Add(this, ref this.npcCellID);
                this.npcCell = newCell;
            }
        }

        internal override void AddToNetCell(NetCell cell)
        {
            base.AddToNetCell(cell);
            if (this.IsPlayer)
            {
                cell.Clients.Add(this.client, ref this.client.cellID);
            }
        }

        internal override void RemoveFromNetCell()
        {
            if (this.IsPlayer)
            {
                this.Cell.Clients.Remove(ref this.client.cellID);
            }
            base.RemoveFromNetCell();
        }

        internal override void UpdatePosition(Vec3f newPos, Vec3f newDir, GameClient exclude)
        {
            base.UpdatePosition(newPos, newDir, exclude);
            this.UpdateNPCCell();
        }

        internal override void ChangeCells(int toX, int toY)
        {
            if (!this.IsPlayer)
            {
                base.ChangeCells(toX, toY);
            }
            else
            {
                NetCell from = this.Cell;
                this.RemoveFromNetCell();

                int i = 0;
                NetCell[] oldCells = new NetCell[NetCell.NumSurroundingCells];
                int oldVobCount = 0;
                from.ForEachSurroundingCell(cell =>
                {
                    if (cell.X <= toX + 1 && cell.X >= toX - 1 && cell.Y <= toY + 1 && cell.Y >= toY - 1)
                    {
                        if (cell.Clients.Count > 0)
                        {
                            //Position updates in shared cells
                            PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
                            stream.Write((ushort)this.ID);
                            stream.WriteCompressedPosition(this.pos);
                            stream.WriteCompressedDirection(this.dir);
                            cell.Clients.ForEach(c =>
                            {
                                c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
                            });
                        }
                    }
                    else
                    {
                        if (cell.Clients.Count > 0)
                        {
                            //deletion updates in old cells
                            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldDespawnMessage);
                            stream.Write((ushort)this.ID);
                            cell.Clients.ForEach(c =>
                            {
                                c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                            });
                        }

                        if (cell.DynVobs.GetCount() > 0)
                        {
                            oldCells[i++] = cell;
                            oldVobCount += cell.DynVobs.GetCount();
                        }
                    }
                });

                // new cells
                i = 0;
                NetCell[] newCells = new NetCell[NetCell.NumSurroundingCells];
                this.world.ForEachSurroundingCell(toX, toY, cell =>
                {
                    if (!(cell.X <= from.X + 1 && cell.X >= from.X - 1 && cell.Y <= from.Y + 1 && cell.Y >= from.Y - 1))
                    {
                        if (cell.Clients.Count > 0)
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

                        if (cell.DynVobs.GetCount() > 0)
                            newCells[i++] = cell;
                    }
                });

                WorldMessage.WriteCellMessage(newCells, oldCells, oldVobCount, this.client);

                this.AddToNetCell(this.world.GetCellFromCoords(toX, toY));
            }
        }

        #endregion

        #region Properties

        internal GameClient client = null;
        public GameClient Client { get { return client; } }
        public bool IsPlayer { get { return Client != null; } }

        partial void pSetHealth()
        {
            if (this.isCreated)
                NPCMessage.WriteHealthMessage(this);
        }

        #endregion

        #region Spawn

        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            if (this.IsPlayer)
            {
                if (world == null)
                    throw new ArgumentNullException("World is null!");

                if (this.isCreated)
                    throw new Exception("Vob is already spawned!");

                this.pos = position;
                this.dir = direction;
                this.world = world;

                WorldMessage.WriteLoadMessage(this.client, world); // tell the client to change worlds first
            }
            else
            {
                base.Spawn(world, position, direction);
                this.UpdateNPCCell();
            }
        }

        // wait until the client has loaded the map
        internal void InsertInWorld()
        {
            // Write surrounding vobs to this client
            int[] coords = NetCell.GetCoords(this.pos);
            NetCell[] arr = new NetCell[NetCell.NumSurroundingCells]; int i = 0;
            this.world.ForEachSurroundingCell(coords[0], coords[1], cell =>
            {
                if (cell.DynVobs.GetCount() > 0)
                    arr[i++] = cell; // save for cell message
            });
            WorldMessage.WriteCellMessage(arr, new NetCell[0], 0, this.client);

            if (!this.isCreated)
            {
                base.Spawn(this.world, pos, dir);
                this.UpdateNPCCell();
                world.AddToPlayers(this.client);
            }

            PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
            stream.Write((ushort)this.ID);
            this.WriteTakeControl(stream);
            this.Client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
        }

        partial void pDespawn()
        {
            if (this.IsPlayer)
            {
                this.client.SetControl(null);
            }
            this.npcCell.npcs.Remove(ref this.npcCellID);
            if (this.npcCell.npcs.Count <= 0)
                this.npcCell.World.npcCells.Remove(this.npcCell.Coord);
        }

        #endregion

        #region Equipment

        partial void pEquipItem(Item item)
        {
            if (this.isCreated)
            {
                NPCMessage.WriteEquipMessage(this, item);
                if (this.IsPlayer)
                    InventoryMessage.WriteEquipMessage(this, item);
            }
        }

        partial void pUnequipItem(Item item)
        {
            if (this.isCreated)
            {
                NPCMessage.WriteUnequipMessage(this, item.slot);
                if (this.IsPlayer)
                    InventoryMessage.WriteUnequipMessage(this, item.slot);
            }
        }

        #endregion

        #region Animations

        partial void pAddOverlay(Overlay overlay)
        {
            if (this.isCreated)
                NPCMessage.WriteApplyOverlayMessage(this, overlay);
        }

        partial void pRemoveOverlay(Overlay overlay)
        {
            if (this.isCreated)
                NPCMessage.WriteRemoveOverlayMessage(this, overlay);
        }

        public void StartAnimation(Animation ani, Action onStop, params object[] netArgs)
        {
            if (this.PlayAni(ani, onStop))
            {
                NPCMessage.WriteAniStart(this, ani, netArgs);
            }
        }

        partial void pStartAnimation(Animation ani)
        {
            NPCMessage.WriteAniStart(this, ani);
        }

        partial void pStopAnimation(Animation ani,bool fadeOut)
        {
            NPCMessage.WriteAniStop(this, ani, fadeOut);
        }

        #endregion

    }
}
