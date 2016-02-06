using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Network;
using RakNet;
using GUC.Network;
using GUC.Server.Network.Messages;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class NPC : Vob
    {
        public partial interface IScriptNPC : IScriptVob
        {
            void OnCmdUseMob(MobInter mob);
            void OnCmdUseItem(Item item);
            void OnCmdMove(NPCStates state);
            void OnCmdMove(NPCStates state, NPC target);
            void OnCmdJump();
            void OnCmdDrawItem(Item item);
            void OnCmdPickupItem(Item item);
            void OnCmdDropItem(Item item);
        }

        public Client Client { get; internal set; }
        public bool IsPlayer { get { return Client != null; } }

        internal NPCCell npcCell = null;

        public MobInter UsedMob
        {
            get { return this.usedMob; }
            set { this.usedMob = value; }
        }

        public NPCStates State
        {
            get { return this.state; }
        }

        public override void Delete()
        {
            foreach (Item item in this.Inventory.GetAll())
            {
                item.Container = null; //so no messages are sent
                item.Delete();
            }
            
            base.Delete();
        }

        #region Networking

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
        
        internal static void ReadCmdMove(PacketReader stream, Client client, NPC character, World world)
        {
            uint id = stream.ReadUInt();
            NPC npc = (NPC)world.Npcs.Get(id);
            if (npc == null) return;

            NPCStates state = (NPCStates)stream.ReadByte();
            if (npc == character || (client.VobControlledList.Contains(npc) && state <= NPCStates.MoveBackward)) //is it a controlled NPC?
            {
                npc.ScriptObj.OnCmdMove(state);
            }
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
        
        internal static void ReadCmdTargetMove(PacketReader stream, Client client, NPC character, World world)
        {
            uint targetid = stream.ReadUInt();
            NPC target = (NPC)world.Npcs.Get(targetid);

            NPCStates state = (NPCStates)stream.ReadByte();            
            character.ScriptObj.OnCmdMove(state, target);
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
            if (item != null && slot != 0/* && item != Item.Fists */&& item.Container == this)
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
            if (item == null || (item.Container != this/* && item != Item.Fists*/))
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
        }*/

        #endregion
    }
}
