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

                npc.State = NPCState.Stand;

                npc.AttackEndTimer = new Timer(OnAttackEnd, (object)npc);
                npc.AttackHitTimer = new Timer(OnAttackHit, (object)npc);

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

        #region Player stats

        public ushort AttrHealth;
        public ushort AttrHealthMax;

        byte talent1H = 0;
        public byte Talent1H { get { return talent1H; } set { talent1H = value > 2 ? (byte)2 : value; } }
        byte talent2H = 0;
        public byte Talent2H { get { return talent2H; } set { talent2H = value > 2 ? (byte)2 : value; } }

        public void AttrHealthUpdate()
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCHealthMessage);
            stream.mWrite(ID);
            stream.mWrite(AttrHealthMax);
            stream.mWrite(AttrHealth);

            foreach (Client cl in cell.SurroundingClients())
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', cl.guid, false);
        }

        #endregion

        public NPCState State { get; protected set; }

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
                    if (state <= NPCState.MoveBackward)
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

        public delegate void CmdTargetMoveHandler(NPC npc, NPC target, NPCState state);
        public static CmdTargetMoveHandler CmdOnTargetMove;

        internal static void ReadCmdTargetState(BitStream stream, Client client)
        {
            if (CmdOnTargetMove == null)
                return;

            NPCState state = (NPCState)stream.mReadByte();
            NPC target = client.character.World.GetNpcOrPlayer(stream.mReadUInt());

            CmdOnTargetMove(client.character, target, state);
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

            if (DrawnItem == null)
            {
                stream.Write0();
            }
            else
            {
                stream.Write1();
                NPCMessage.WriteStrmDrawItem(stream, this, DrawnItem);
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

                NPC scav = NPC.Create("scavenger");
                scav.DrawnItem = Item.Fists;
                scav.Spawn(client.character.World);

                Vob mob = Vob.Create("forge");
                mob.Spawn(client.character.World, new Types.Vec3f(-200, -100, 200), new Types.Vec3f(0, 0, 1));
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
        /// <param name="state">Stand, MoveForward, MoveBackward</param>
        public void DoMoveState(NPCState state)
        {
            switch (state)
            {
                case NPCState.Stand:
                    DoStand();
                    break;
                case NPCState.MoveForward:
                    DoMoveForward();
                    break;
                case NPCState.MoveBackward:
                    DoMoveBackward();
                    break;
            }
        }

        /// <summary>
        /// Order the NPC to do a certain movement on a target.
        /// </summary>
        /// <param name="state">MoveLeft, MoveRight, AttackForward, AttackLeft, AttackRight, AttackRun, Parry, DodgeBack</param>
        public void DoMoveState(NPCState state, NPC target)
        {
            switch (state)
            {
                case NPCState.MoveLeft:
                    DoMoveLeft(target);
                    break;
                case NPCState.MoveRight:
                    DoMoveRight(target);
                    break;
                case NPCState.AttackForward:
                    DoAttackForward(target);
                    break;
                case NPCState.AttackLeft:
                    DoAttackLeft(target);
                    break;
                case NPCState.AttackRight:
                    DoAttackRight(target);
                    break;
                case NPCState.AttackRun:
                    DoAttackRun(target);
                    break;
                case NPCState.Parry:
                    DoParry(target);
                    break;
                case NPCState.DodgeBack:
                    DoDodgeBack(target);
                    break;
            }
        }

        public void DoStand()
        {
            this.State = NPCState.Stand;
            NPCMessage.WriteState(cell.SurroundingClients(), this);
        }

        public void DoMoveForward()
        {
            this.State = NPCState.MoveForward;
            NPCMessage.WriteState(cell.SurroundingClients(), this);
        }

        public void DoMoveBackward()
        {
            this.State = NPCState.MoveBackward;
            NPCMessage.WriteState(cell.SurroundingClients(), this);
        }

        public void DoMoveLeft(NPC target)
        {
            this.State = NPCState.MoveLeft;
            NPCMessage.WriteTargetState(cell.SurroundingClients(), this, target);
        }

        public void DoMoveRight(NPC target)
        {
            this.State = NPCState.MoveRight;
            NPCMessage.WriteTargetState(cell.SurroundingClients(), this, target);
        }

        internal NPCCell npcCell = null;

        internal AnimationControl AniCtrl { get { return Instance.AniCtrl; } }
        public int ComboNum { get; protected set; }
        long comboTime = 0;
        Timer AttackEndTimer; // fixme? only one timer?
        Timer AttackHitTimer;
        bool TriedAttack = false;

        public void DoAttackForward(NPC target)
        {
            AnimationControl.Attacks attacks;
            if (!GetAttacks(out attacks))
                return;

            if (ComboNum >= attacks.ComboNum - 1) //can not combo yet or is in last combo
                return;

            int newComboNum;
            if (State == NPCState.AttackForward) //already in attack
            {
                newComboNum = ComboNum + 1;
            }
            else
            {
                newComboNum = 0;
            }

            DoAttack(target, attacks.Forward[newComboNum], NPCState.AttackForward, newComboNum);
        }

        static void OnAttackEnd(object obj)
        {
            NPC attacker = (NPC)obj;
            attacker.State = NPCState.Stand;
            attacker.ComboNum = 0;
            attacker.TriedAttack = false;
            Log.Logger.log("Stand");
        }

        static void OnAttackHit(object obj)
        {
            if (obj != null && sOnHit != null)
            {
                NPC attacker = (NPC)obj;
                if (attacker.DrawnItem == null)
                    return;

                float range = attacker.DrawnItem == Item.Fists ? 100 : attacker.DrawnItem.Instance.Range;
                foreach (NPC target in attacker.World.GetNPCs(attacker.Position, range))
                {
                    if (attacker == target) continue;

                    Vec3f dir = (attacker.Position - target.Position).normalise();
                    float dot = attacker.Direction.Z * dir.Z + dir.X * attacker.Direction.X;

                    if (dot > 0) continue; //target is behind attacker
                    if (dot > -0.6f) continue; //target is too far right or left

                    sOnHit(attacker, target);
                    if (target.OnHit != null)
                    {
                        target.OnHit(attacker, target);
                    }
                    Log.Logger.log("HIT: " + target.ID);
                }
            }
        }

        public delegate void OnHitHandler(NPC attacker, NPC target);
        public OnHitHandler OnHit;
        public static OnHitHandler sOnHit;

        bool GetAttacks(out AnimationControl.Attacks attacks)
        {
            if (DrawnItem != null)
            {
                if (DrawnItem == Item.Fists)
                {
                    attacks = AniCtrl.Fist;
                    return true;
                }
                else if (DrawnItem.Type == ItemType.Blunt_1H || DrawnItem.Type == ItemType.Sword_1H)
                {
                    attacks = AniCtrl._1H[Talent1H];
                    return true;
                }
                else if (DrawnItem.Type == ItemType.Blunt_2H || DrawnItem.Type == ItemType.Sword_2H)
                {
                    attacks = AniCtrl._2H[Talent2H];
                    return true;
                }
            }
            attacks = default(AnimationControl.Attacks);
            return false;
        }

        void DoAttack(NPC target, AnimationControl.Attacks.Info attackTimes, NPCState attack, int comboNum)
        {
            if (attack >= NPCState.AttackForward && attackTimes.Attack > 0 && !TriedAttack)
            {
                if (ServerTime.Now.Ticks >= comboTime)
                {
                    AttackEndTimer.CallTime = ServerTime.Now.Ticks + attackTimes.Attack;
                    AttackEndTimer.Start();

                    if (attackTimes.Combo > 0)
                    {
                        comboTime = ServerTime.Now.Ticks + attackTimes.Combo;
                    }
                    else //no combo time defined
                    {
                        comboTime = AttackEndTimer.CallTime;
                    }

                    if (AttackHitTimer.Started) //just in case, that the last attack's timer has not fired yet
                    {
                        OnAttackHit(this);
                    }

                    if (attackTimes.Hit > 0)
                    {
                        AttackHitTimer.CallTime = ServerTime.Now.Ticks + attackTimes.Hit;
                        AttackHitTimer.Start();
                    }

                    ComboNum = comboNum;
                    State = attack;
                    TriedAttack = false;
                    NPCMessage.WriteTargetState(cell.SurroundingClients(), this, target);
                }
                else
                {
                    TriedAttack = true;
                }
            } 
        }

        public void DoAttackLeft(NPC target)
        {
            AnimationControl.Attacks attacks;
            if (!GetAttacks(out attacks))
            {
                return;
            }

            if (State == NPCState.AttackLeft)
            {
                return;
            }

            DoAttack(target, attacks.Left, NPCState.AttackLeft, 0);
        }

        public void DoAttackRight(NPC target)
        {
            AnimationControl.Attacks attacks;
            if (!GetAttacks(out attacks))
            {
                return;
            }

            if (State == NPCState.AttackRight)
            {
                return;
            }

            DoAttack(target, attacks.Right, NPCState.AttackRight, 0);
        }

        public void DoAttackRun(NPC target)
        {
            AnimationControl.Attacks attacks;
            if (!GetAttacks(out attacks))
            {
                return;
            }

            if (State == NPCState.AttackRun)
            {
                return;
            }

            DoAttack(target, attacks.Run, NPCState.AttackRun, 0);
        }

        public void DoParry(NPC target)
        {
            AnimationControl.Attacks attacks;
            if (!GetAttacks(out attacks))
            {
                return;
            }

            if (State == NPCState.Parry)
            {
                return;
            }

            DoAttack(target, attacks.Parry, NPCState.Parry, 0);
        }

        public void DoDodgeBack(NPC target)
        {
            AnimationControl.Attacks attacks;
            if (!GetAttacks(out attacks))
            {
                return;
            }

            if (State == NPCState.DodgeBack)
            {
                return;
            }

            DoAttack(target, attacks.Dodge, NPCState.DodgeBack, 0);
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
            if (item == null || (item.Owner != this && item != Item.Fists))
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
