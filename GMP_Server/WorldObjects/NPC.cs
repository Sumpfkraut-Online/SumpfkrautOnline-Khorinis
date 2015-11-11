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

namespace GUC.Server.WorldObjects
{
    public class NPC : AbstractCtrlVob
    {
        /// <summary>
        /// Gets the NPCInstance of this NPC.
        /// </summary>
        public NPCInstance Instance { get; protected set; }

        #region Constructors

        /// <summary>
        /// Creates and returns an NPC object from the given NPCInstance-ID.
        /// Returns NULL when the ID is not found!
        /// </summary>
        public static NPC Create(ushort instanceID)
        {
            NPCInstance inst = NPCInstance.Table.Get(instanceID);
            if (inst == null)
            {
                Log.Logger.logError("NPC creation failed! Instance ID not found: " + instanceID);
                return null;
            }
            return Create(inst);
        }

        /// <summary>
        /// Creates and returns an NPC object from the given NPCInstance-Name.
        /// Returns NULL when the name is not found!
        /// </summary>
        public static NPC Create(string instanceName)
        {
            NPCInstance inst = NPCInstance.Table.Get(instanceName);
            if (inst == null)
            {
                Log.Logger.logError("NPC creation failed! Instance name not found: " + instanceName);
                return null;
            }
            return Create(inst);
        }

        /// <summary>
        /// Creates and returns an NPC object from the given NPCInstance.
        /// Returns NULL when the NPCInstance is NULL!
        /// </summary>
        public static NPC Create(NPCInstance instance)
        {
            if (instance != null)
            {
                NPC npc = new NPC();
                npc.Instance = instance;
                npc.BodyHeight = instance.BodyHeight;
                npc.BodyHeight = instance.BodyHeight;
                npc.BodyWidth = instance.BodyWidth;
                npc.BodyFatness = instance.Fatness;

                npc.AttrHealthMax = instance.AttrHealthMax;
                npc.AttrHealth = instance.AttrHealthMax;

                npc.AttrManaMax = instance.AttrManaMax;
                npc.AttrMana = instance.AttrManaMax;
                npc.AttrStaminaMax = instance.AttrStaminaMax;
                npc.AttrStamina = instance.AttrStaminaMax;

                npc.AttrStrength = instance.AttrStrength;
                npc.AttrDexterity = instance.AttrDexterity;

                npc.AttrCapacity = 100;

                npc.State = NPCState.Stand;
                npc.WeaponState = NPCWeaponState.None;
                return npc;
            }
            else
            {
                Log.Logger.logError("NPC creation failed! Instance can't be NULL!");
                return null;
            }
        }

        protected NPC()
        {
        }

        #endregion

        #region Appearance

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

        //Things only the playing client should know
        #region Player stats

        public ushort AttrHealth;
        public ushort AttrHealthMax;

        public ushort AttrMana;
        public ushort AttrManaMax;
        public ushort AttrStamina;
        public ushort AttrStaminaMax;

        public ushort AttrStrength;
        public ushort AttrDexterity;

        public ushort AttrCapacity;

        #region Health

        public void AttrHealthUpdate()
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCHitMessage);
            stream.mWrite(ID);
            stream.mWrite(AttrHealthMax);
            stream.mWrite(AttrHealth);

            foreach (Client cl in cell.SurroundingClients())
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', cl.guid, false);
        }

        public void AttrManaStaminaUpdate()
        {
            if (isPlayer)
            {
                BitStream stream = Program.server.SetupStream(NetworkID.PlayerAttributeMSMessage);
                stream.mWrite(AttrManaMax);
                stream.mWrite(AttrMana);
                stream.mWrite(AttrStaminaMax);
                stream.mWrite(AttrStamina);
                Program.server.ServerInterface.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
            }
        }

        public void AttrUpdate()
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCHitMessage);
            stream.mWrite(ID);
            stream.mWrite(AttrHealthMax);
            stream.mWrite(AttrHealth);

            foreach (Client cl in cell.SurroundingClients(client))
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', cl.guid, false);

            if (isPlayer)
            {
                // Update all the stats!
                stream = Program.server.SetupStream(NetworkID.PlayerAttributeMessage);
                stream.mWrite(AttrHealthMax);
                stream.mWrite(AttrHealth);
                stream.mWrite(AttrManaMax);
                stream.mWrite(AttrMana);
                stream.mWrite(AttrStaminaMax);
                stream.mWrite(AttrStamina);
                stream.mWrite(AttrCapacity);
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
            }
        }

        #endregion

        #endregion

        #region States

        public NPCState State { get; protected set; }
        public NPCWeaponState WeaponState { get; protected set; }

        #endregion

        public MobInter UsedMob { get; protected set; }

        #region Networking

        internal Client client;
        public bool isPlayer { get { return client != null; } }

        #region Client commands
        public delegate void CmdUseMobHandler(MobInter mob, NPC user);
        public static event CmdUseMobHandler CmdOnUseMob;
        public static event CmdUseMobHandler CmdOnUnUseMob;

        internal static void ReadCmdUseMob(BitStream stream, Client client)
        {
            if (CmdOnUseMob == null)
                return;

            uint ID = stream.mReadUInt();

            Vob mob;
            if (client.character.World.VobDict.TryGetValue(ID, out mob) && mob is MobInter)
            {
                CmdOnUseMob((MobInter)mob, client.character);
            }
        }

        internal static void ReadCmdUnuseMob(BitStream stream, Client client)
        {
            if (CmdOnUnUseMob != null && client.character.UsedMob != null)
            {
                CmdOnUnUseMob(client.character.UsedMob, client.character);
            }
        }

        public delegate void CmdUseItemHandler(Item item, NPC user);
        public static event CmdUseItemHandler CmdOnUseItem;

        internal static void ReadCmdUseItem(BitStream stream, Client client)
        {
            if (CmdOnUseItem == null)
                return;

            uint ID = stream.mReadUInt();
            Item item;
            if (sWorld.ItemDict.TryGetValue(ID, out item) && item.Owner == client.character)
            {
                CmdOnUseItem(item, client.character);
            }
        }

        public delegate void CmdMoveHandler(NPC npc, NPCState state);
        public static event CmdMoveHandler CmdOnMove;

        internal static void ReadCmdState(BitStream stream, Client client)
        {
            if (CmdOnMove == null)
                return;

            uint id = stream.mReadUInt();
            NPCState state = (NPCState)stream.mReadByte();
            if (id == client.character.ID)
            {
                CmdOnMove(client.character, state);
            }
            else //is it a controlled NPC?
            {
                NPC npc;
                if (sWorld.NPCDict.TryGetValue(id, out npc) && client.VobControlledList.Find(v => v == npc) != null)
                {
                    if (state <= NPCState.MoveRight)
                    {
                        CmdOnMove(npc, state);
                    }
                }
            }
        }

        public delegate void CmdJumpHandler(NPC npc);
        public static event CmdJumpHandler CmdOnJump;

        internal static void ReadCmdJump(BitStream stream, Client client)
        {
            if (CmdOnJump == null)
                return;

            uint id = stream.mReadUInt();
            if (id == client.character.ID)
            {
                CmdOnJump(client.character);
            }
            else //is it a controlled NPC?
            {
                NPC npc;
                if (sWorld.NPCDict.TryGetValue(id, out npc) && client.VobControlledList.Find(v => v == npc) != null)
                {
                    CmdOnJump(npc);
                }
            }
        }

        public delegate void CmdDrawEquipmentHandler(NPC npc, Item item);
        public static event CmdDrawEquipmentHandler CmdOnDrawEquipment;
        public static event CmdDrawEquipmentHandler CmdOnUndrawItem;

        internal static void ReadCmdDrawEquipment(BitStream stream, Client client)
        {
            if (CmdOnDrawEquipment == null)
                return;

            byte slot = stream.mReadByte();
            Item item = client.character.GetEquipment(slot);

            CmdOnDrawEquipment(client.character, item == null ? Item.Fists : item);
        }

        internal static void ReadCmdUndrawItem(BitStream stream, Client client)
        {
            if (CmdOnUndrawItem == null || client.character.DrawnItem == null)
                return;

            CmdOnUndrawItem(client.character, client.character.DrawnItem);
        }

        #endregion

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldNPCSpawnMessage);
            stream.mWrite(ID);
            stream.mWrite(Instance.ID);
            stream.mWrite(pos);
            stream.mWrite(dir);
            if (Instance.ID <= 2)
            {
                stream.mWrite((byte)HumanBodyTex);
                stream.mWrite((byte)HumanHeadMesh);
                stream.mWrite((byte)HumanHeadTex);
                stream.mWrite((byte)HumanVoice);
            }
            stream.mWrite(BodyHeight);
            stream.mWrite(BodyWidth);
            stream.mWrite(BodyFatness);

            stream.mWrite(CustomName);

            stream.mWrite(AttrHealthMax);
            stream.mWrite(AttrHealth);

            stream.mWrite((byte)equippedSlots.Count);
            foreach (KeyValuePair<byte, Item> slot in equippedSlots)
            {
                stream.mWrite(slot.Key);
                stream.mWrite(slot.Value.ID);
                stream.mWrite(slot.Value.Instance.ID);
                stream.mWrite(slot.Value.Condition);
            }

            foreach (Client cl in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', cl.guid, false);
        }

        public delegate void OnEnterWorldHandler(NPC player);
        public static event OnEnterWorldHandler OnEnterWorld;

        internal static void WriteControl(Client client, NPC npc)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.PlayerControlMessage);
            stream.mWrite(npc.ID);
            stream.mWrite(npc.World.MapName);
            //write stats & inventory
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'G', client.guid, false);
        }

        internal static void ReadControl(BitStream stream, Client client)
        {
            if (client.mainChar == null) // coming from the log-in menus, first spawn
            {
                client.mainChar = client.character;
                Network.Messages.ConnectionMessage.WriteInstanceTables(client);

                if (OnEnterWorld != null)
                {
                    OnEnterWorld(client.mainChar);
                }

                Item item = Item.Create("itfo_apple");
                item.Amount = 3;
                item.Spawn(client.character.World);

                item = Item.Create("itmw_wolfszahn");
                item.Condition = 200;
                item.SpecialLine = "Geschmiedet von Malak Akbar.";
                item.Spawn(client.character.World, new Types.Vec3f(200, 0, 200), new Types.Vec3f(0, 0, 1));

                //NPC scav = NPC.Create("scavenger");
                //scav.Spawn(client.character.World);
            }

            if (!client.character.Spawned)
            {
                client.character.Spawn(client.character.World);
                client.character.WriteSpawn(new Client[1] { client });
            }
        }

        internal static void ReadPickUpItem(BitStream stream, Client client)
        {
            Item item;
            client.character.World.ItemDict.TryGetValue(stream.mReadUInt(), out item);
            if (item == null) return;

            client.character.AddItem(item);
        }

        public delegate void TargetMovementHandler(NPC npc, NPC target, NPCState state, Vec3f position, Vec3f direction);
        public static TargetMovementHandler sOnTargetMovement;
        public TargetMovementHandler OnTargetMovement;

        internal static void ReadTargetState(BitStream stream, Client client)
        {
            NPCState state = (NPCState)stream.mReadByte();
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();
            NPC target = client.character.World.GetNpcOrPlayer(stream.mReadUInt());

            if (sOnTargetMovement != null)
            {
                sOnTargetMovement(client.character, target, state, pos, dir);
            }
            if (client.character.OnTargetMovement != null)
            {
                client.character.OnTargetMovement(client.character, target, state, pos, dir);
            }
        }

        public void DoTargetMovement(NPCState state, Vec3f position, Vec3f direction, NPC target)
        {
            this.State = state;
            this.pos = position;
            this.dir = direction;
            NPCMessage.WriteTargetState(cell.SurroundingClients(), this, target);
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

                if (DrawnItem == item)
                {
                    DoUndrawItem(true); //set to fists?
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

        protected Dictionary<ItemInstance, List<Item>> inventory = new Dictionary<ItemInstance, List<Item>>();

        /// <summary>
        /// Gets a list of the items this NPC is carrying.
        /// </summary>
        public List<Item> ItemList
        {
            get
            {
                List<Item> itemList = new List<Item>();
                foreach (List<Item> list in inventory.Values)
                {
                    itemList.AddRange(list);
                }
                return itemList;
            }
        }

        /// <summary>
        /// Checks whether this NPC has the Item with the given ID.
        /// </summary>
        public bool HasItem(uint itemID)
        {
            Item item = null;
            if (sWorld.ItemDict.TryGetValue(itemID, out item))
            {
                return HasItem(item);
            }
            return false;
        }

        /// <summary>
        /// Checks whether this NPC has the given Item.
        /// </summary>
        public bool HasItem(Item item)
        {
            List<Item> list;
            if (inventory.TryGetValue(item.Instance, out list))
            {
                return list.Contains(item);
            }
            return false;
        }

        /// <summary>
        /// Checks whether this NPC an Item of the given ItemInstance.
        /// Only stackable Items are considered, i.e. no unique items like user-written scrolls, worn weapons etc.
        /// </summary>
        public bool HasItem(ItemInstance instance)
        {
            return HasItem(instance, 1);
        }

        /// <summary>
        /// Checks whether this NPC has the amount of Items of the given ItemInstance.
        /// Only stackable Items are considered, i.e. no unique items like user-written scrolls, worn weapons etc.
        /// </summary>
        public bool HasItem(ItemInstance instance, ushort amount)
        {
            List<Item> list;
            if (inventory.TryGetValue(instance, out list))
            {
                Item item = list.Find(i => i.Stackable == true);
                if (item != null)
                {
                    if (item.amount >= amount)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to add the given Item to the NPC's inventory.
        /// Stackable items may be deleted by this method!
        /// </summary>
        public void AddItem(Item item)
        {
            if (item.Spawned)
            {
                item.Despawn(); //Fixme?: Send despawn + additem msg in one msg to the new owner
            }
            //else
            if (item.Owner != null)
            {
                item.Owner.RemoveItem(item);
            }

            List<Item> list;
            if (inventory.TryGetValue(item.Instance, out list))
            {
                if (item.Stackable)
                {
                    Item other = list.Find(i => i.Stackable == true);
                    if (other != null) //merge the items
                    {
                        other.amount += item.amount;
                        item.RemoveFromServer();
                        InventoryMessage.WriteAmountUpdate(this.client, other);
                        item.Owner = this;
                        return;
                    }
                }
            }
            else
            {
                list = new List<Item>(1);
                inventory.Add(item.Instance, list);
            }
            list.Add(item);
            InventoryMessage.WriteAddItem(this.client, item);
            item.Owner = this;
        }

        /// <summary>
        /// Removes the given Item from the NPC's inventory if he owns it.
        /// If you want to delete the Item call "item.RemoveFromServer()" instead.
        /// </summary>
        public void RemoveItem(Item item)
        {
            if (item.Owner == this)
            {
                UnequipItem(item);
                if (DrawnItem == item)
                {
                    DoUndrawItem(true); //set to fists?
                }

                item.Owner = null;
                List<Item> list;
                if (inventory.TryGetValue(item.Instance, out list))
                {
                    list.Remove(item);
                    if (list.Count == 0)
                    {
                        inventory.Remove(item.Instance);
                    }
                }

                InventoryMessage.WriteAmountUpdate(this.client, item, 0);
            }
        }

        /// <summary>
        /// Removes one Item of the given ItemInstance from the NPC's inventory.
        /// Only stackable Items are considered, i.e. no unique items like user-written scrolls, worn weapons etc.
        /// </summary>
        public void RemoveItem(ItemInstance instance)
        {
            RemoveItem(instance, 1);
        }

        /// <summary>
        /// Removes the amount of Items of the given ItemInstance from the NPC's inventory.
        /// Only stackable Items are considered, i.e. no unique items like user-written scrolls, worn weapons etc.
        /// </summary>
        public void RemoveItem(ItemInstance instance, ushort amount)
        {
            List<Item> list;
            if (inventory.TryGetValue(instance, out list))
            {
                Item item = list.Find(i => i.Stackable == true);
                if (item != null)
                {
                    int newAmount = item.amount - amount;
                    if (newAmount > 0)
                    {
                        item.amount = (ushort)newAmount;
                        InventoryMessage.WriteAmountUpdate(this.client, item);
                    }
                    else
                    {
                        item.RemoveFromServer();
                    }
                }
            }
        }

        #endregion

        #region Events
        public delegate void OnHitHandler(NPC attacker, NPC target);
        public OnHitHandler OnHit;
        public static OnHitHandler sOnHit;

        internal void DoHit(NPC attacker)
        {
            if (sOnHit != null)
            {
                sOnHit(attacker, this);
            }
            if (OnHit != null)
            {
                OnHit(attacker, this);
            }
        }
        #endregion

        public void DoUseMob(MobInter mob)
        {
            if (mob != null)
            {
                UsedMob = mob;

                BitStream stream = Program.server.SetupStream(NetworkID.MobUseMessage);
                stream.mWrite(this.ID);
                stream.mWrite(mob.ID);

                foreach (Client cl in this.cell.SurroundingClients())
                {
                    Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', cl.guid, false);
                }
            }
        }

        public void DoUnUseMob()
        {
            if (UsedMob != null)
            {
                UsedMob = null;

                BitStream stream = Program.server.SetupStream(NetworkID.MobUnUseMessage);
                stream.mWrite(this.ID);

                foreach (Client cl in cell.SurroundingClients())
                {
                    Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', cl.guid, false);
                }
            }
        }

        /// <summary>
        /// Order the NPC to do a certain movement.
        /// </summary>
        public void DoMoveState(NPCState state)
        {
            this.State = state;
            NPCMessage.WriteState(cell.SurroundingClients(), this);
        }

        /// <summary>
        /// Let the NPC do a jump.
        /// </summary>
        public void DoJump()
        {
            NPCMessage.WriteJump(cell.SurroundingClients(), this);
        }

        /// <summary>
        /// Gets the current drawn Item in the hands of the NPC.
        /// </summary>
        public Item DrawnItem { get; internal set; }

        /// <summary>
        /// Take out an item.
        /// </summary>
        public void DoDrawitem(Item item)
        {
            DoDrawitem(item, false);
        }

        /// <summary>
        /// Take out an item.
        /// </summary>
        /// <param name="fast">True if the state should be set instantly, without playing an animation.</param>
        public void DoDrawitem(Item item, bool fast)
        {
            if (item == null || item.Owner != this)
                return;

            DrawnItem = item;
            NPCMessage.WriteDrawItem(cell.SurroundingClients(), this, item, fast);
        }

        /// <summary>
        /// Undraw the current Item.
        /// </summary>
        public void DoUndrawItem()
        {
            DoUndrawItem(false);
        }

        /// <summary>
        /// Undraw the current Item.
        /// </summary>
        /// <param name="fast">True if the state should be set instantly, without playing an animation.</param>
        public void DoUndrawItem(bool fast)
        {
            if (DrawnItem == null)
                return;

            DrawnItem = null;
            NPCMessage.WriteUndrawItem(cell.SurroundingClients(), this, fast, State == NPCState.Stand);
        }
    }
}
