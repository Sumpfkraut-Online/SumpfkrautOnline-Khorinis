using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Network;
using RakNet;
using GUC.Network;
using GUC.Server.Network.Messages;
using GUC.Types;
using GUC.Server.Scripting;
using GUC.Server.WorldObjects.Mobs;

namespace GUC.Server.WorldObjects
{
    public class NPC : Vob
    {
        new public NPCInstance Instance { get; protected set; }

        #region Appearance

        public string Name { get { return Instance.Name; } }

        protected string customName = "";
        /// <summary>Set this for a different name from the instance-name.</summary>
        public string CustomName
        {
            get { return customName; }
            set
            {
                if (value == null || value == Instance.Name)
                {
                    customName = "";
                }
                else
                {
                    customName = value;
                }
            }
        }

        public string BodyMesh { get { return Instance.BodyMesh; } }

        /// <summary>This field will be only used with the "_Male"- or "_Female"-Instance.</summary>
        public HumBodyTex HumanBodyTex = HumBodyTex.G1Hero;
        /// <summary>This field will be only used with the "_Male"- or "_Female"-Instance.</summary>
        public HumHeadMesh HumanHeadMesh = HumHeadMesh.HUM_HEAD_PONY;
        /// <summary>This field will be only used with the "_Male"- or "_Female"-Instance.</summary>
        public HumHeadTex HumanHeadTex = HumHeadTex.Face_N_Player;
        /// <summary>This field will be only used with the "_Male"- or "_Female"-Instance.</summary>
        public HumVoice HumanVoice = HumVoice.Hero;

        /// <summary>Body height in percent. (0% ... 255%)</summary>
        public byte BodyHeight;
        /// <summary>Body width in percent. (0% ... 255%)</summary>
        public byte BodyWidth;
        /// <summary>Fatness in percent. (-32768% ... +32767%)</summary>
        public short BodyFatness;

        #endregion

        #region Stats

        public ushort Health;
        public ushort HealthMax;

        #endregion

        #region States

        public NPCState State { get; protected set; }
        public MobInter UsedMob { get; protected set; }

        #endregion

        public Client client { get; internal set; }
        public bool isPlayer { get { return client != null; } }

        internal NPCCell npcCell = null;

        public NPC(NPCInstance instance, object scriptObject) : base(instance, scriptObject)
        {
            this.BodyHeight = instance.BodyHeight;
            this.BodyHeight = instance.BodyHeight;
            this.BodyWidth = instance.BodyWidth;
            this.BodyFatness = instance.Fatness;

            this.HealthMax = instance.HealthMax;
            this.Health = instance.HealthMax;

            this.State = NPCState.Stand;

            Network.Server.sAllNpcsDict.Add(this.ID, this);
            Network.Server.sNpcDict.Add(this.ID, this);
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (Item item in this.GetItems())
            {
                item.Owner = null; //so no messages are sent
                item.Dispose();
            }

            Network.Server.sAllNpcsDict.Remove(this.ID);
            if (this.isPlayer)
            {
                Network.Server.sPlayerDict.Remove(this.ID);
            }
            else
            {
                Network.Server.sNpcDict.Remove(this.ID);
            }
        }

        #region Networking

        #region Client commands

        public static Action<NPC, MobInter> CmdOnUseMob;
        internal static void ReadCmdUseMob(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnUseMob == null)
                return;

            uint id = stream.ReadUInt();

            Vob vob = character.World.GetVob(id);
            if (vob != null && vob is MobInter)
            {
                CmdOnUseMob(character, (MobInter)vob);
            }
        }

        public static Action<NPC> CmdOnUnUseMob;
        internal static void ReadCmdUnuseMob(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnUnUseMob == null)
                return;

            if (character.UsedMob != null)
            {
                CmdOnUnUseMob(character);
            }
        }

        public static Action<NPC, Item> CmdOnUseItem;
        internal static void ReadCmdUseItem(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnUseItem == null)
                return;

            uint id = stream.ReadUInt();

            Item item = character.GetItem(id);
            if (item != null)
            {
                CmdOnUseItem(character, item);
            }
        }

        public static Action<NPC, NPCState> CmdOnMove;
        internal static void ReadCmdMove(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnMove == null)
                return;

            uint id = stream.ReadUInt();
            NPCState state = (NPCState)stream.ReadByte();

            NPC npc = character.World.GetNpcOrPlayer(id);
            if (npc != null && (npc == character || (client.VobControlledList.Contains(npc) && state <= NPCState.MoveBackward))) //is it a controlled NPC?
            {
                CmdOnMove(npc, state);
            }
        }

        public static Action<NPC> CmdOnJump;
        internal static void ReadCmdJump(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnJump == null)
                return;

            uint id = stream.ReadUInt();

            NPC npc = character.World.GetNpcOrPlayer(id);
            if (npc != null && (npc == character || client.VobControlledList.Contains(npc))) //is it a controlled NPC?
            {
                CmdOnJump(npc);
            }
        }
        
        public static Action<NPC, Item> CmdOnDrawItem;
        internal static void ReadCmdDrawItem(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnDrawItem == null)
                return;

            uint id = stream.ReadUInt();

            Item item = character.GetItem(id);
            CmdOnDrawItem(character, item == null ? Item.Fists : item);
        }

        public static Action<NPC> CmdOnUndrawItem;
        internal static void ReadCmdUndrawItem(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnUndrawItem == null)
                return;

            if (character.DrawnItem != null)
            {
                CmdOnUndrawItem(character);
            }
        }
        
        public static Action<NPC, NPC, NPCState> CmdOnTargetMove;
        internal static void ReadCmdTargetMove(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnTargetMove == null)
                return;

            uint targetid = stream.ReadUInt();
            NPCState state = (NPCState)stream.ReadByte();

            NPC target = character.World.GetNpcOrPlayer(targetid);
            CmdOnTargetMove(character, target, state);
        }

        public static Action<NPC, Item> CmdOnPickup;
        internal static void ReadCmdPickupItem(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnPickup == null)
                return;

            uint targetid = stream.ReadUInt();

            Item item = character.World.GetItem(targetid);
            if (item != null)
            {
                CmdOnPickup(character, item);
            }
        }

        public static Action<NPC, Item> CmdOnDrop;
        internal static void ReadCmdDropItem(PacketReader stream, Client client, NPC character)
        {
            if (CmdOnPickup == null)
                return;

            uint targetid = stream.ReadUInt();

            Item item = character.GetItem(targetid);
            if (item != null)
            {
                CmdOnDrop(character, item);
            }
        }

        #endregion

        new public static Action<NPC, PacketWriter> OnWriteSpawn;
        internal override void WriteSpawn(PacketWriter stream)
        {
            base.WriteSpawn(stream);

            if (Instance.ID <= 2)
            {
                stream.Write((byte)HumanBodyTex);
                stream.Write((byte)HumanHeadMesh);
                stream.Write((byte)HumanHeadTex);
                stream.Write((byte)HumanVoice);
            }
            stream.Write(BodyHeight);
            stream.Write(BodyWidth);
            stream.Write(BodyFatness);

            stream.Write(CustomName);

            stream.Write(HealthMax);
            stream.Write(Health);

            stream.Write((byte)equippedSlots.Count);
            foreach (KeyValuePair<byte, Item> slot in equippedSlots)
            {
                stream.Write(slot.Key);
                slot.Value.WriteEquipped(stream);
            }

            if (DrawnItem == null)
            {
                stream.Write(false);
            }
            else
            {
                stream.Write(true);
                DrawnItem.WriteEquipped(stream);
            }

            //Overlays

            if (NPC.OnWriteSpawn != null)
                NPC.OnWriteSpawn(this, stream);
        }

        internal override void WriteSpawnMessage(IEnumerable<Client> list)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.WorldNPCSpawnMessage);
            this.WriteSpawn(stream);

            foreach (Client client in list)
            {
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            }
        }

        public static Action<Client, NPC> OnWriteControl;
        internal static void WriteControl(Client client, NPC npc)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.PlayerControlMessage);
            stream.Write(npc.ID);
            stream.Write(npc.World.FileName);

            stream.Write(npc.inventory.Count);
            foreach (Item item in npc.GetItems())
            {
                item.WriteInventory(stream);
            }

            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'G');
        }

        public static Action<NPC> OnEnterWorld;
        internal static void ReadControl(PacketReader stream, Client client, NPC character)
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
                client.Character.Spawn(character.World);
            }
        }

        #endregion

        #region Equipment
        Dictionary<byte, Item> equippedSlots = new Dictionary<byte, Item>();

        /// <summary>
        /// Equip an Item.
        /// </summary>
        /// <param name="slot">Don't use 0!</param>
        public void EquipSlot(byte slot, Item item)
        {
            if (item != null && slot != 0 && item != Item.Fists && item.Owner == this)
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
                NPCMessage.WriteEquipMessage(cell.SurroundingClients(), this, item, slot);
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
                NPCMessage.WriteUnequipMessage(cell.SurroundingClients(), this, slot);
            }
        }

        /// <summary>
        /// Unequip an Item.
        /// </summary>
        public void UnequipItem(Item item)
        {
            if (item != null && item.Owner == this)
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

        #region Itemcontainer

        protected Dictionary<uint, Item> inventory = new Dictionary<uint, Item>();

        public IEnumerable<Item> GetItems() { return inventory.Values; }
        public Item GetItem(uint id) { Item item; inventory.TryGetValue(id, out item); return item; }

        public void AddItem(Item item)
        {
            if (item != null && item.ID > 0)
            {
                inventory[item.ID] = item;
                InventoryMessage.WriteAddItem(client, item);
            }
        }

        public void RemoveItem(Item item)
        {
            if (item != null && item.ID > 0)
            {
                inventory.Remove(item.ID);
                InventoryMessage.WriteAmountUpdate(client, item, 0);
            }
        }

        #endregion

        #region Mobs

        public void UseMob(MobInter mob)
        {
            if (mob != null)
            {
                UsedMob = mob;

                PacketWriter stream = Program.server.SetupStream(NetworkID.MobUseMessage);
                stream.Write(this.ID);
                stream.Write(mob.ID);

                foreach (Client cl in this.cell.SurroundingClients())
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

                PacketWriter stream = Program.server.SetupStream(NetworkID.MobUnUseMessage);
                stream.Write(this.ID);

                foreach (Client cl in cell.SurroundingClients())
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
        public void SetMoveState(NPCState state)
        {
            this.State = state;
            NPCMessage.WriteState(cell.SurroundingClients(), this);
        }

        /// <summary>
        /// Order the NPC to do a certain movement on a target.
        /// </summary>
        public void SetMoveState(NPCState state, NPC target)
        {
            this.State = state;
            NPCMessage.WriteTargetState(cell.SurroundingClients(), this, target);
        }

        /// <summary>
        /// Let the NPC do a jump.
        /// </summary>
        public void Jump()
        {
            NPCMessage.WriteJump(cell.SurroundingClients(), this);
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
            if (item == null || (item.Owner != this && item != Item.Fists))
                return;

            DrawnItem = item;
            NPCMessage.WriteDrawItem(cell.SurroundingClients(), this, item, fast);
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
            NPCMessage.WriteUndrawItem(cell.SurroundingClients(), this, fast, State == NPCState.Stand);
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
                PacketWriter stream = Program.server.SetupStream(NetworkID.ControlCmdMessage);
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
                PacketWriter stream = Program.server.SetupStream(NetworkID.ControlCmdMessage);
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
                PacketWriter stream = Program.server.SetupStream(NetworkID.ControlCmdMessage);
                stream.Write(this.ID);
                stream.Write((byte)ControlCmd.Stop);

                this.VobController.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            }
        }*/

        #endregion
    }
}
