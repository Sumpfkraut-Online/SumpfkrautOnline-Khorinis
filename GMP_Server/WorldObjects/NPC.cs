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
    public class NPC : AbstractCtrlVob
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

        public ushort Mana;
        public ushort ManaMax;

        #endregion
        
        public NPCState State { get; protected set; }
        public MobInter UsedMob { get; protected set; }

        public Client client { get; internal set; }
        public bool isPlayer { get { return client != null; } }

        public NPC(NPCInstance instance, object scriptObject) : base(scriptObject)
        {
            this.BodyHeight = instance.BodyHeight;
            this.BodyHeight = instance.BodyHeight;
            this.BodyWidth = instance.BodyWidth;
            this.BodyFatness = instance.Fatness;

            this.HealthMax = instance.HealthMax;
            this.Health = instance.HealthMax;

            this.ManaMax = instance.ManaMax;
            this.Mana = instance.ManaMax;

            this.State = NPCState.Stand;

            Network.Server.sAllNpcsDict.Add(this.ID, this);
            Network.Server.sNpcDict.Add(this.ID, this);
        }

        public override void Dispose()
        {
            base.Dispose();

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
        public delegate void CmdUseMobHandler(MobInter mob, NPC user);
        public static event CmdUseMobHandler CmdOnUseMob;
        public static event CmdUseMobHandler CmdOnUnUseMob;

        internal static void ReadCmdUseMob(BitStream stream, Client client)
        {
            if (CmdOnUseMob == null)
                return;

            uint ID = stream.mReadUInt();

            VobMob mob;
            if (client.Character.World.vobDict.TryGetValue(ID, out mob) && mob is MobInter)
            {
                CmdOnUseMob((MobInter)mob, client.Character);
            }
        }

        internal static void ReadCmdUnuseMob(BitStream stream, Client client)
        {
            if (CmdOnUnUseMob != null && client.Character.UsedMob != null)
            {
                CmdOnUnUseMob(client.Character.UsedMob, client.Character);
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
            if (Network.Server.sItemDict.TryGetValue(ID, out item) && item.Owner == client.Character)
            {
                CmdOnUseItem(item, client.Character);
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
            if (id == client.Character.ID)
            {
                CmdOnMove(client.Character, state);
            }
            else //is it a controlled NPC?
            {
                NPC npc;
                if (Network.Server.sNpcDict.TryGetValue(id, out npc) && client.VobControlledList.Find(v => v == npc) != null)
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
            if (id == client.Character.ID)
            {
                CmdOnJump(client.Character);
            }
            else //is it a controlled NPC?
            {
                NPC npc;
                if (Network.Server.sNpcDict.TryGetValue(id, out npc) && client.VobControlledList.Find(v => v == npc) != null)
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
            Item item = client.Character.GetEquipment(slot);

            CmdOnDrawEquipment(client.Character, item == null ? Item.Fists : item);
        }

        internal static void ReadCmdUndrawItem(BitStream stream, Client client)
        {
            if (CmdOnUndrawItem == null || client.Character.DrawnItem == null)
                return;

            CmdOnUndrawItem(client.Character, client.Character.DrawnItem);
        }

        public delegate void CmdTargetMoveHandler(NPC npc, NPC target, NPCState state);
        public static CmdTargetMoveHandler CmdOnTargetMove;

        internal static void ReadCmdTargetState(BitStream stream, Client client)
        {
            if (CmdOnTargetMove == null)
                return;

            NPCState state = (NPCState)stream.mReadByte();
            NPC target = client.Character.World.GetNpcOrPlayer(stream.mReadUInt());

            CmdOnTargetMove(client.Character, target, state);
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
                stream.Write(slot.Value.ID);
                stream.Write(slot.Value.Instance.ID);
            }

            if (DrawnItem == null)
            {
                stream.Write(false);
            }
            else
            {
                stream.Write(true);
                NPCMessage.WriteStrmDrawItem(stream, this, DrawnItem);
            }

            //Overlays

            if (NPC.OnWriteSpawn != null)
                NPC.OnWriteSpawn(this, stream);
        }

        public delegate void OnEnterWorldHandler(NPC player);
        public static event OnEnterWorldHandler OnEnterWorld;

        internal static void WriteControl(Client client, NPC npc)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.PlayerControlMessage);
            stream.Write(npc.ID);
            stream.Write(npc.World.FileName);
            //write stats & inventory
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'G');
        }

        internal static void ReadControl(PacketReader stream, Client client, NPC character)
        {
            if (client.MainChar == null) // coming from the log-in menus, first spawn
            {
                client.MainChar = client.Character;
                ConnectionMessage.WriteInstanceTables(client);

                if (OnEnterWorld != null)
                {
                    OnEnterWorld(client.MainChar);
                }
            }

            if (!client.Character.IsSpawned)
            {
                client.Character.WriteSpawn(new Client[1] { client }); // to self
                client.Character.Spawn(client.Character.World);
            }
        }

        internal static void ReadPickUpItem(BitStream stream, Client client)
        {
            Item item;
            client.Character.World.itemDict.TryGetValue(stream.mReadUInt(), out item);
            if (item == null) return;

            client.Character.AddItem(item);
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
            if (Network.Server.sItemDict.TryGetValue(itemID, out item))
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
            if (item.IsSpawned)
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
                        item.Dispose();
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
                        item.Dispose();
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

                PacketWriter stream = Program.server.SetupStream(NetworkID.MobUseMessage);
                stream.Write(this.ID);
                stream.Write(mob.ID);

                foreach (Client cl in this.cell.SurroundingClients())
                {
                    cl.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
                }
            }
        }

        public void DoUnUseMob()
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
        Timer<NPC> AttackEndTimer; // fixme? only one timer?
        Timer<NPC> AttackHitTimer;
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

        static void OnAttackEnd(NPC attacker)
        {
            if (attacker != null)
            {
                attacker.AttackEndTimer.Stop();

                attacker.State = NPCState.Stand;
                attacker.ComboNum = 0;
                attacker.TriedAttack = false;
                Log.Logger.log("Stand");
            }
        }

        static void OnAttackHit(NPC attacker)
        {
            if (attacker != null)
            {
                attacker.AttackHitTimer.Stop();

                if (sOnHit == null || attacker.DrawnItem == null)
                    return;

                float range = attacker.DrawnItem == Item.Fists ? 100 : attacker.DrawnItem.Instance.Range;
                foreach (NPC target in attacker.World.GetNPCs(attacker.Position, range))
                {
                    if (attacker == target) continue;

                    Vec3f dir = (attacker.Position - target.Position).Normalise();
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
                if (DateTime.UtcNow.Ticks >= comboTime)
                {
                    AttackEndTimer.Interval = attackTimes.Attack;
                    AttackEndTimer.Start();

                    if (attackTimes.Combo > 0)
                    {
                        comboTime = DateTime.UtcNow.Ticks + attackTimes.Combo;
                    }
                    else //no combo time defined
                    {
                        comboTime = AttackEndTimer.NextCallTime;
                    }

                    if (AttackHitTimer.Started) //just in case, that the last attack's timer has not fired yet
                    {
                        OnAttackHit(this);
                    }

                    if (attackTimes.Hit > 0)
                    {
                        AttackHitTimer.Interval = attackTimes.Hit;
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

        ControlCmd ControlState = ControlCmd.Stop;
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
        }
    }
}
