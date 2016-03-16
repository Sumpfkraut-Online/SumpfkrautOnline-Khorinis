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
            
            void OnCmdMove(NPCStates state, IScriptBaseVob target = null);
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

        partial void pSetState(NPCStates state, BaseVob target = null);
        public void SetState(NPCStates state, BaseVob target = null)
        {
            this.state = state;
            if (this.IsSpawned)
            {
                pSetState(state, target);
            }
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

        //protected Dictionary<byte, Item> equippedSlots = new Dictionary<byte, Item>();
        //public IEnumerable<Item> GetEquippedItems() { return equippedSlots.Values; }

        public string Name { get { return Instance.Name; } }
        public string BodyMesh { get { return Instance.BodyMesh; } }
        public int BodyTex { get { return Instance.BodyTex; } }
        public string HeadMesh { get { return Instance.HeadMesh; } }
        public int HeadTex { get { return Instance.HeadTex; } }

        #endregion

        #region Read & Write

        internal void WriteTakeControl(PacketWriter stream)
        {
            if (this.ScriptObject != null)
            {
                this.ScriptObject.OnWriteTakeControl(stream);
            }
        }

        internal void ReadTakeControl(PacketReader stream)
        {
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
