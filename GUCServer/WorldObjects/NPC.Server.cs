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
            void OnCmdMove(NPCStates state);
            void OnCmdUseMob(MobInter mob);
            void OnCmdUseItem(Item item);
            void OnCmdDrawItem(Item item);
            void OnCmdPickupItem(Item item);
            void OnCmdDropItem(Item item);
            void OnCmdEquipItem(int slot, Item item);
            void OnCmdUnequipItem(Item item);
            void OnCmdAniStart(Animation ani);
            void OnCmdAniStop(bool fadeOut);
            void OnCmdJump();
        }

        #endregion
        
        public void UpdatePropertiesFast()
        {

        }

        public void UpdateProperties()
        {
            // send msg
        }

        partial void pJump()
        {
            NPCMessage.WriteJump(this);
        }

        partial void pSetState(NPCStates state)
        {
            if (this.isCreated)
                NPCMessage.WriteState(this, state);
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
                this.world.RemoveFromPlayers(this.client);
            }
            this.npcCell.npcs.Remove(ref this.npcCellID);
            if (this.npcCell.npcs.Count <= 0)
                this.world.npcCells.Remove(this.npcCell.Coord);
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

        partial void pStartAnimation(Animation ani)
        {
            NPCMessage.WriteAniStart(this, ani);
        }

        partial void pStopAnimation(bool fadeOut)
        {
            NPCMessage.WriteAniStop(this, fadeOut);
        }

        #endregion

        /* #region Networking

         #region Client commands

         internal static void ReadCmdUseMob(PacketReader stream, Client client, NPC character, World world)
         {
             uint id = stream.ReadUInt();
             Vob vob = world.MobInters.Get(id);

             character.ScriptObj.OnCmdUseMob((MobInter)vob);
         }

         internal static void ReadCmdUseItem(PacketReader stream, Client client, NPC character, World world)
         {
             uint id = stream.ReadUInt();

             Item item = character.Inventory.Get(id);
             character.ScriptObj.OnCmdUseItem(item);
         }

         internal static void ReadCmdJump(PacketReader stream, Client client, NPC character, World world)
         {
             uint id = stream.ReadUInt();
             NPC npc = (NPC)world.Npcs.Get(id);
             if (npc == null) return;

             if (npc != null && (npc == character || client.VobControlledList.Contains(npc))) //is it a controlled NPC?
             {
                 npc.ScriptObj.OnCmdJump();
             }
         }

         internal static void ReadCmdDrawItem(PacketReader stream, Client client, NPC character, World world)
         {
             uint id = stream.ReadUInt();

             Item item = character.Inventory.Get(id);
             character.ScriptObj.OnCmdDrawItem(item);
         }

         internal static void ReadDrawFists(PacketReader stream, Client client, NPC character, World world)
         {
             //character.ScriptObj.OnCmdDrawItem(Item.Fists);
         }



         internal static void ReadCmdPickupItem(PacketReader stream, Client client, NPC character, World world)
         {
             uint targetid = stream.ReadUInt();
             Item item = (Item)world.Items.Get(targetid);
             character.ScriptObj.OnCmdPickupItem(item);
         }

         internal static void ReadCmdDropItem(PacketReader stream, Client client, NPC character, World world)
         {
             uint targetid = stream.ReadUInt();
             Item item = character.Inventory.Get(targetid);
             character.ScriptObj.OnCmdDropItem(item);
         }

         #endregion

         internal override void WriteSpawnMessage(IEnumerable<Client> list)
         {
             PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldNPCSpawnMessage);
             this.WriteSpawnProperties(stream);

             foreach (Client client in list)
             {
                 client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
             }
         }

         public static Action<Client, NPC> OnWriteControl;
         internal static void WriteControl(Client client, NPC npc)
         {
             PacketWriter stream = GameServer.SetupStream(NetworkIDs.PlayerControlMessage);
             stream.Write(npc.ID);
             stream.Write(npc.World.FileName);


             client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'G');
         }

         public static Action<NPC> OnEnterWorld;
         internal static void ReadControl(PacketReader stream, Client client, NPC character, World world)
         {
             if (client.MainChar == null) // coming from the log-in menus, first spawn
             {
                 client.MainChar = character;
                 ConnectionMessage.WriteInstanceTables(client);

                 if (OnEnterWorld != null)
                     OnEnterWorld(character);
             }

             if (!character.IsSpawned)
             {
                 character.WriteSpawnMessage(new Client[1] { client }); // to self
                 client.Character.Spawn(world);
             }
         }

         #endregion

         #region Equipment

         /// <summary>
         /// Equip an Item.
         /// </summary>
         /// <param name="slot">Don't use 0!</param>
         public void EquipSlot(byte slot, Item item)
         {
             if (item != null && slot != 0/* && item != Item.Fists *//*&& item.Container == this)
             {
                 Item oldItem;
                 if (equippedSlots.TryGetValue(slot, out oldItem))
                 {
                     if (oldItem != null)
                     {
                         oldItem.Slot = 0;
                     }
                     equippedSlots[slot] = item;
                 }
                 else
                 {
                     equippedSlots.Add(slot, item);
                 }
                 item.Slot = slot;
                 NPCMessage.WriteEquipMessage(Cell.SurroundingClients(), this, item, slot);
             }
         }

         /// <summary>
         /// Unequip an Item slot.
         /// </summary>
         /// <param name="slot">Don't use 0!</param>
         public void UnequipSlot(byte slot)
         {
             if (slot == 0) return;

             Item item;
             if (equippedSlots.TryGetValue(slot, out item))
             {
                 if (item != null)
                 {
                     item.Slot = 0;
                 }

                 equippedSlots.Remove(slot);
                 NPCMessage.WriteUnequipMessage(Cell.SurroundingClients(), this, slot);
             }
         }

         /// <summary>
         /// Unequip an Item.
         /// </summary>
         public void UnequipItem(Item item)
         {
             if (item != null && item.Container == this)
             {
                 UnequipSlot(item.Slot);
             }
         }

         /// <summary>
         /// Get the equipped Item of a slot.
         /// </summary>
         /// <param name="slot">Don't use 0!</param>
         public Item GetEquipment(byte slot)
         {
             Item item = null;
             equippedSlots.TryGetValue(slot, out item);
             return item;
         }

         #endregion

         #region Mobs

         public void UseMob(MobInter mob)
         {
             if (mob != null)
             {
                 UsedMob = mob;

                 PacketWriter stream = GameServer.SetupStream(NetworkIDs.MobUseMessage);
                 stream.Write(this.ID);
                 stream.Write(mob.ID);

                 foreach (Client cl in this.Cell.SurroundingClients())
                 {
                     cl.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                 }
             }
         }

         public void UnuseMob()
         {
             if (UsedMob != null)
             {
                 UsedMob = null;

                 PacketWriter stream = GameServer.SetupStream(NetworkIDs.MobUnUseMessage);
                 stream.Write(this.ID);

                 foreach (Client cl in Cell.SurroundingClients())
                 {
                     cl.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                 }
             }
         }

         #endregion

         #region Movement

         /// <summary>
         /// Order the NPC to do a certain movement.
         /// </summary>
         public void SetMoveState(NPCStates state)
         {
             this.state = state;
             NPCMessage.WriteState(Cell.SurroundingClients(), this);
         }

         /// <summary>
         /// Order the NPC to do a certain movement on a target.
         /// </summary>
         public void SetMoveState(NPCStates state, NPC target)
         {
             this.state = state;
             NPCMessage.WriteTargetState(Cell.SurroundingClients(), this, target);
         }

         /// <summary>
         /// Let the NPC do a jump.
         /// </summary>
         public void Jump()
         {
             NPCMessage.WriteJump(Cell.SurroundingClients(), this);
         }

         #endregion

         #region Item Drawing

         /// <summary>
         /// Gets the current drawn Item in the hands of the NPC.
         /// </summary>
         public Item DrawnItem { get; internal set; }

         /// <summary>
         /// Take out an item.
         /// </summary>
         public void Drawitem(Item item)
         {
             Drawitem(item, false);
         }

         /// <summary>
         /// Take out an item.
         /// </summary>
         /// <param name="fast">True if the state should be set instantly, without playing an animation.</param>
         public void Drawitem(Item item, bool fast)
         {
             if (item == null || (item.Container != this/* && item != Item.Fists*//*))
                 return;

             DrawnItem = item;
             NPCMessage.WriteDrawItem(Cell.SurroundingClients(), this, item, fast);
         }

         /// <summary>
         /// Undraw the current Item.
         /// </summary>
         public void UndrawItem()
         {
             UndrawItem(false);
         }

         /// <summary>
         /// Undraw the current Item.
         /// </summary>
         /// <param name="fast">True if the state should be set instantly, without playing an animation.</param>
         public void UndrawItem(bool fast)
         {
             if (DrawnItem == null)
                 return;

             DrawnItem = null;
             NPCMessage.WriteUndrawItem(Cell.SurroundingClients(), this, fast, State == NPCStates.Stand);
         }

         #endregion

         #region AI Commands

         /*ControlCmd ControlState = ControlCmd.Stop;
         Vec3f ControlTargetPos;
         Vob ControlTargetVob;

         public void GoTo(Vec3f position)
         {
             GoTo(position, 100);
         }

         public void GoTo(Vec3f position, float range)
         {
             if (this.VobController == null)
             {
                 this.Position = position;
             }
             else
             {
                 PacketWriter stream = Network.Server.SetupStream(NetworkID.ControlCmdMessage);
                 stream.Write(this.ID);
                 stream.Write((byte)ControlCmd.GoToPos);
                 stream.Write(position);
                 stream.Write(range);

                 this.VobController.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
             }
         }

         public void GoTo(Vob vob)
         {
             GoTo(vob, 100);
         }

         public void GoTo(Vob vob, float range)
         {
             if (vob == null)
                 return;

             if (this.VobController == null)
             {
                 this.Position = vob.pos;
             }
             else
             {
                 PacketWriter stream = Network.Server.SetupStream(NetworkID.ControlCmdMessage);
                 stream.Write(this.ID);
                 stream.Write((byte)ControlCmd.GoToVob);
                 stream.Write(vob.ID);
                 stream.Write(range);

                 this.VobController.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
             }
         }

         public void GoStop()
         {
             if (this.VobController != null)
             {
                 PacketWriter stream = Network.Server.SetupStream(NetworkID.ControlCmdMessage);
                 stream.Write(this.ID);
                 stream.Write((byte)ControlCmd.Stop);

                 this.VobController.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
             }
         }*//*

         #endregion*/

    }
}
