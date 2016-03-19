using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Mobs;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class NPC : Vob, ItemContainer.IContainer
    {
        public override VobTypes VobType { get { return VobTypes.NPC; } }

        #region ScriptObject

        public partial interface IScriptNPC : IScriptVob
        {
            void OnWriteTakeControl(PacketWriter stream);
            void OnReadTakeControl(PacketReader stream);
            
            void OnCmdMove(NPCStates state);
            void OnCmdJump();
        }

        new public IScriptNPC ScriptObject
        {
            get { return (IScriptNPC)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        public NPC()
        {
            this.inventory = new ItemContainer(this);
        }

        #region Properties

        new public NPCInstance Instance
        {
            get { return (NPCInstance)base.Instance; }
            set { base.Instance = value; }
        }

        int hpmax = 100;
        public int HPMax { get { return hpmax; } }
        int hp = 100;
        public int HP { get { return hp; } }

        public void SetHealth(int hp)
        {
            SetHealth(hp, this.hpmax);
        }

        partial void pSetHealth(int hp, int hpmax);
        public void SetHealth(int hp, int hpmax)
        {
            if (hp < 0 || hp > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("HP is out of range! (0..65535) val: " + hp);
            if (hpmax < 0 || hpmax > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("HPMax is out of range! (0..65535) val: " + hpmax);

            this.hp = hp;
            this.hpmax = hpmax;

            pSetHealth(hp, hpmax);
        }

        partial void pFallDown();
        public void FallDown()
        {
            pFallDown();
        }

        internal NPCStates NextState = NPCStates.Stand;

        NPCStates state = NPCStates.Stand;
        public NPCStates State { get { return this.state; } }

        MobInter usedMob = null;
        public MobInter UsedMob { get { return this.usedMob; } }

        partial void pJump();
        public void Jump()
        {
            if (this.IsSpawned)
            {
                pJump();
            }
        }

        partial void pSetState(NPCStates state);
        public void SetState(NPCStates state)
        {
            this.state = state;
            pSetState(state);
        }

        public void UseMob(MobInter mob)
        {

        }

        Item drawnItem = null;
        public Item DrawnItem { get { return this.drawnItem; } }

        bool isInAttackMode = false;
        public bool IsInAttackMode { get { return this.isInAttackMode; } }
        
        /// <param name="item">null == fists</param>
        public void DrawItem(Item item)
        {

        }

        public void UndrawItem()
        {
        }

        ItemContainer inventory;
        public ItemContainer Inventory { get { return inventory; } }

        public string Name { get { return Instance.Name; } }
        public string BodyMesh { get { return Instance.BodyMesh; } }
        public int BodyTex { get { return Instance.BodyTex; } }
        public string HeadMesh { get { return Instance.HeadMesh; } }
        public int HeadTex { get { return Instance.HeadTex; } }

        #endregion

        #region Equipment

        public const int MAX_EQUIPPEDITEMS = 255;
        Dictionary<int, Item> equippedSlots = new Dictionary<int, Item>();

        partial void pEquipItem(Item item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot">0-254</param>
        public void EquipItem(int slot, Item item)
        {
            if (item == null)
                throw new ArgumentNullException("Item is null!");

            if (item.Container != this)
                throw new ArgumentException("Item is not in this container!");

            if (slot < 0 || slot >= MAX_EQUIPPEDITEMS)
                throw new ArgumentOutOfRangeException("Slotnum is out of range! 0.." + (MAX_EQUIPPEDITEMS-1));

            if (equippedSlots.ContainsKey(slot))
                throw new ArgumentException("Slot is already equipped!");

            item.slot = slot;
            equippedSlots.Add(slot, item);
            pEquipItem(item);
        }

        public void UnequipItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("Item is null!");

            if (item.Container != this)
                throw new ArgumentException("Item is not in this container!");

            UnequipSlot(item.slot);
        }

        partial void pUnequipItem(Item item);
        public void UnequipSlot(int slot)
        {
            if (slot < 0 || slot >= MAX_EQUIPPEDITEMS)
                throw new ArgumentOutOfRangeException("Slotnum is out of range! 0.." + (MAX_EQUIPPEDITEMS - 1));

            Item item;
            if (equippedSlots.TryGetValue(slot, out item))
            {
                pUnequipItem(item);
                item.slot = Item.SLOTNUM_UNEQUIPPED;
                equippedSlots.Remove(slot);
            }
        }

        #region Access

        public bool IsSlotEquipped(int slot)
        {
            return equippedSlots.ContainsKey(slot);
        }

        public bool TryGetEquippedItem(int slot, out Item item)
        {
            return equippedSlots.TryGetValue(slot, out item);
        }

        public void ForEachEquippedItem(Action<Item> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach (Item item in equippedSlots.Values)
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

            foreach(Item item in equippedSlots.Values)
            {
                if (!predicate(item))
                    break;
            }
        }

        #endregion

        #endregion

        #region Read & Write

        internal void WriteTakeControl(PacketWriter stream)
        {
            stream.Write((byte)this.inventory.GetCount());
            this.inventory.ForEachItem(item =>
            {
                item.WriteInventoryProperties(stream);
            });

            if (this.ScriptObject != null)
            {
                this.ScriptObject.OnWriteTakeControl(stream);
            }
        }

        internal void ReadTakeControl(PacketReader stream)
        {
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                this.inventory.Add(Scripting.ScriptManager.Interface.CreateInvItem(stream));
            }

            if (this.ScriptObject != null)
            {
                this.ScriptObject.OnReadTakeControl(stream);
            }
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write((ushort)hpmax);
            stream.Write((ushort)hp);

            /*stream.Write((byte)equippedSlots.Count);
            foreach (KeyValuePair<byte, Item> slot in equippedSlots)
            {
                stream.Write(slot.Key);
                slot.Value.WriteEquipProperties(stream);
            }

            if (drawnItem == null)
            {
                stream.Write(false);
            }
            else
            {
                stream.Write(true);
                drawnItem.WriteEquipProperties(stream);
            }

            //Overlays*/
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.hpmax = stream.ReadUShort();
            this.hp = stream.ReadUShort();
        }

        #endregion
    }
}
