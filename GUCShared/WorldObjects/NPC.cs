using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Mobs;
using GUC.Network;
using GUC.WorldObjects.ItemContainers;
using GUC.Scripting;
using GUC.Types;
using GUC.Models;

namespace GUC.WorldObjects
{
    public partial class NPC : Vob, ItemContainer
    {
        public partial class ClimbingLedge
        {
            Vec3f loc;
            Vec3f norm;
            Vec3f cont;
            float maxMoveFwd;

            public Vec3f Location { get { return this.loc; } }

            public ClimbingLedge(PacketReader stream)
            {
                this.loc = stream.ReadVec3f();
                this.norm = stream.ReadVec3f();
                this.cont = stream.ReadVec3f();
                this.maxMoveFwd = stream.ReadFloat();
            }

            public void WriteStream(PacketWriter stream)
            {
                stream.Write(loc);
                stream.Write(norm);
                stream.Write(cont);
                stream.Write(maxMoveFwd);
            }
        }

        public override VobTypes VobType { get { return VobTypes.NPC; } }

        #region ScriptObject

        public partial interface IScriptNPC : IScriptVob
        {
            void OnWriteTakeControl(PacketWriter stream);
            void OnReadTakeControl(PacketReader stream);

            void EquipItem(int slot, Item item);
            void UnequipItem(Item item);

            void SetState(NPCMovement state);

            void SetHealth(int hp, int hpmax);
            
            void SetFightMode(bool fightMode);
        }
        
        new public IScriptNPC ScriptObject { get { return (IScriptNPC)base.ScriptObject; } }

        #endregion

        #region Constructors

        public NPC(ItemInventory.IScriptItemInventory scriptItemInventory, Model.IScriptModel scriptModel, IScriptNPC scriptObject) : base(scriptModel, scriptObject)
        {
            this.inventory = new NPCInventory(this, scriptItemInventory);
        }

        #endregion

        #region Properties
        
        public override Type InstanceType { get { return typeof(NPCInstance); } }
        new public NPCInstance Instance
        {
            get { return (NPCInstance)base.Instance; }
            set { SetInstance(value); }
        }

        NPCInventory inventory;
        public ItemInventory Inventory { get { return inventory; } }
        
        /// <summary> The NPC's name set by its Instance. </summary>
        public string Name { get { return Instance.Name; } }
        /// <summary> The NPC's body mesh set by its Instance. </summary>
        public string BodyMesh { get { return Instance.BodyMesh; } }
        /// <summary> The NPC's body texture set by its Instance. </summary>
        public int BodyTex { get { return Instance.BodyTex; } }
        /// <summary> The NPC's head mesh set by its Instance. </summary>
        public string HeadMesh { get { return Instance.HeadMesh; } }
        /// <summary> The NPC's head texture set by its Instance. </summary>
        public int HeadTex { get { return Instance.HeadTex; } }

        #endregion

        #region Environment

        partial void pGetEnvironment();
        public override Environment GetEnvironment()
        {
            pGetEnvironment();
            return this.environment;
        }

        #endregion

        #region Health

        int hpmax = 100;
        public int HPMax { get { return hpmax; } }
        int hp = 100;
        public int HP { get { return hp; } }

        public bool IsDead { get { return this.hp <= 0; } }

        public void SetHealth(int hp)
        {
            SetHealth(hp, this.hpmax);
        }

        partial void pSetHealth();
        public void SetHealth(int hp, int hpmax)
        {
            if (hp > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("HP is out of range! (0..65535) val: " + hp);
            if (hpmax <= 0 || hpmax > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("HPMax is out of range! (1..65535) val: " + hpmax);

            if (hp <= 0)
            {
                this.movement = NPCMovement.Stand;
                this.Model.ForEachActiveAni(aa => aa.Stop());
                this.hp = 0;
            }
            else
            {
                this.hp = hp;
            }
            this.hpmax = hpmax;

            pSetHealth();
        }

        #endregion

        #region Movement

        NPCMovement movement = NPCMovement.Stand;
        public NPCMovement Movement { get { return this.movement; } }

        #endregion
        
        #region Equipment

        Dictionary<int, Item> equippedItems = new Dictionary<int, Item>();

        partial void pEquipSwitch(int slot, Item item); 
        partial void pEquipItem(int slot, Item item);
        public void EquipItem(int slot, Item item)
        {
            if (item == null)
                throw new ArgumentNullException("Item is null!");

            if (item.Container != this)
                throw new ArgumentException("Item is not in this container!");
            
            if (slot < 0 || slot >= Item.SlotNumUnused)
                throw new ArgumentOutOfRangeException("Slotnum must be greater or equal than zero and smaller than " + Item.SlotNumUnused);

            if (equippedItems.ContainsKey(slot))
                throw new ArgumentException("Slot is already in use!");

            if (item.IsEquipped) 
            {
                if (item.slot == slot)
                    return;

                // switching slots
                equippedItems.Remove(item.slot);
                item.slot = slot;
                equippedItems.Add(slot, item);

                pEquipSwitch(slot, item);
            }
            else
            {
                item.slot = slot;
                equippedItems.Add(slot, item);

                pEquipItem(slot, item);
            }
        }

        public Item UnequipSlot(int slot)
        {
            Item item;
            if (equippedItems.TryGetValue(slot, out item))
            {
                UnequipItem(item);
                return item;
            }
            else
            {
                return null;
            }
        }

        partial void pUnequipItem(Item item);
        public void UnequipItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("Item is null!");

            if (item.Container != this)
                throw new ArgumentException("Item is not in this container!");

            if (!item.IsEquipped)
                throw new ArgumentException("Item is not equipped!");

            pUnequipItem(item);

            equippedItems.Remove(item.slot);
            item.slot = Item.SlotNumUnused;
        }

        #region Access

        public bool TryGetEquippedItem(int slot, out Item item)
        {
            return equippedItems.TryGetValue(slot, out item);
        }

        public void ForEachEquippedItem(Action<Item> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach(Item item in equippedItems.Values)
            {
                action(item);
            }
        }

        /// <summary>
        /// Return FALSE to break the loop!
        /// </summary>
        public void ForEachEquippedItem(Predicate<Item> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            foreach (Item item in equippedItems.Values)
            {
                if (!predicate(item))
                    break;
            }
        }

        #endregion

        #endregion
        
        #region Fight Mode

        bool isInFightMode = false;
        public bool IsInFightMode { get { return this.isInFightMode; } }

        partial void pSetFightMode(bool fightMode);
        public void SetFightMode(bool fightMode)
        {
            if (this.IsDead)
                return;

            if (this.isInFightMode == fightMode)
                return;

            pSetFightMode(fightMode);
            this.isInFightMode = fightMode;
        }

        #endregion

        #region Read & Write

        #region Player Control

        internal void WriteTakeControl(PacketWriter stream)
        {
            stream.Write((byte)this.inventory.Count);
            this.inventory.ForEach(item =>
            {
                stream.Write((byte)item.ID);
                item.WriteInventoryProperties(stream);
            });

            this.ScriptObject.OnWriteTakeControl(stream);
        }

        internal void ReadTakeControl(PacketReader stream)
        {
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                int itemID = stream.ReadByte();
                Item item;
                if (!inventory.TryGetItem(itemID, out item))
                {
                    item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
                    item.ID = itemID;
                    item.ReadInventoryProperties(stream);
                    this.inventory.ScriptObject.AddItem(item);
                }
                else
                {
                    item.ReadInventoryProperties(stream);
                }
            }
            this.ScriptObject.OnReadTakeControl(stream);
        }

        #endregion

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write((byte)this.movement);

            stream.Write((ushort)hpmax);
            stream.Write((ushort)hp);
            
            stream.Write((byte)equippedItems.Count);
            ForEachEquippedItem(item =>
            {
                stream.Write((byte)item.slot);
                item.WriteEquipProperties(stream);
            });

            stream.Write(this.isInFightMode);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.movement = (NPCMovement)stream.ReadByte();

            this.hpmax = stream.ReadUShort();
            this.hp = stream.ReadUShort();

            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                int slot = stream.ReadByte();
                Item item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
                item.ReadEquipProperties(stream);
                this.inventory.ScriptObject.AddItem(item);
                this.ScriptObject.EquipItem(slot, item);
            }

            this.isInFightMode = stream.ReadBit();
        }

        #endregion

        #region Mob using

        MobInter usedMob = null;
        public MobInter UsedMob { get { return this.usedMob; } }

        public void UseMob(MobInter mob)
        {

        }

        #endregion

        #region Spawn

        partial void pBeforeDespawn();
        partial void pAfterDespawn();
        public override void Despawn()
        {
            pBeforeDespawn();

            base.Despawn();
            this.Model.ForEachActiveAni(aa => aa.Stop());

            pAfterDespawn();
        }

        #endregion

        partial void pOnTick(long now);
        internal override void OnTick(long now)
        {
            base.OnTick(now);
            pOnTick(now);
            this.Model.OnTick(now); // update animations
        }
    }
    
}
