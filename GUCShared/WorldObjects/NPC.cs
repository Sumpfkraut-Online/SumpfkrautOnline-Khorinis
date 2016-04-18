using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Mobs;
using GUC.Network;
using GUC.WorldObjects.Collections;
using GUC.Animations;
using GUC.Scripting;
using GUC.Types;

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

            void AddItem(Item item);
            void RemoveItem(Item item);

            void EquipItem(int slot, Item item);
            void UnequipItem(Item item);

            void SetState(NPCStates state);
            void Jump();

            void ApplyOverlay(Overlay overlay);
            void RemoveOverlay(Overlay overlay);

            void StartAnimation(Animation ani);
            void StopAnimation(bool fadeOut);

            void SetHealth(int hp, int hpmax);
        }

        new public IScriptNPC ScriptObject
        {
            get { return (IScriptNPC)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public NPCInstance Instance
        {
            get { return (NPCInstance)base.Instance; }
            set { base.Instance = value; }
        }

        public string Name { get { return Instance.Name; } }
        public string BodyMesh { get { return Instance.BodyMesh; } }
        public int BodyTex { get { return Instance.BodyTex; } }
        public string HeadMesh { get { return Instance.HeadMesh; } }
        public int HeadTex { get { return Instance.HeadTex; } }

        #endregion

        #region Constructors

        public NPC()
        {
            this.inventory = new ItemContainer(this);
            this.aniTimer = new GUCTimer(this.EndAni);
        }

        #endregion

        #region Health

        int hpmax = 100;
        public int HPMax { get { return hpmax; } }
        int hp = 100;
        public int HP { get { return hp; } }

        public bool IsDead { get { return this.hp == 0; } }

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
                this.state = NPCStates.Stand;
                this.currentAni = null;
                this.aniTimer.Stop(true);
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

        #region Movement / NPCStates

        NPCStates state = NPCStates.Stand;
        public NPCStates State { get { return this.state; } }

        partial void pSetState(NPCStates state);
        public void SetState(NPCStates state)
        {
            if (this.IsDead)
                return;

            pSetState(state);
            this.state = state;
        }

        partial void pFallDown();
        public void FallDown()
        {
            if (this.IsDead)
                return;

            pFallDown();
        }

        #endregion

        #region Inventory

        ItemContainer inventory;
        public ItemContainer Inventory { get { return inventory; } }

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
                throw new ArgumentOutOfRangeException("Slotnum is out of range! 0.." + (MAX_EQUIPPEDITEMS - 1));

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

            foreach (Item item in equippedSlots.Values)
            {
                if (!predicate(item))
                    break;
            }
        }

        #endregion

        #endregion

        #region Item drawing

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

        #endregion

        #region Read & Write

        #region Player Control

        internal void WriteTakeControl(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write((byte)this.state);

            stream.Write((ushort)hpmax);
            stream.Write((ushort)hp);

            // applied overlays
            if (this.overlays == null)
            {
                stream.Write((byte)0);
            }
            else
            {
                stream.Write((byte)overlays.Count);
                for (int i = 0; i < overlays.Count; i++)
                {
                    stream.Write((byte)overlays[i].ID);
                }
            }

            stream.Write((byte)this.inventory.GetCount());
            this.inventory.ForEachItem(item =>
            {
                stream.Write((byte)item.ID);
                item.WriteInventoryProperties(stream);
            });

            this.ScriptObject.OnWriteTakeControl(stream);
        }

        internal void ReadTakeControl(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.state = (NPCStates)stream.ReadByte();

            this.hpmax = stream.ReadUShort();
            this.hp = stream.ReadUShort();

            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                Overlay ov;
                int id = stream.ReadByte();
                if (this.Model.TryGetOverlay(id, out ov))
                {
                    this.ScriptObject.ApplyOverlay(ov);
                }
                else
                {
                    throw new Exception("Overlay not found: " + id);
                }
            }

            count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                int itemID = stream.ReadByte();
                Item item;
                if (!inventory.TryGetItem(itemID, out item))
                {
                    item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
                    item.ID = itemID;
                    item.ReadInventoryProperties(stream);
                    this.ScriptObject.AddItem(item);
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

            stream.Write((byte)this.state);

            stream.Write((ushort)hpmax);
            stream.Write((ushort)hp);

            // applied overlays

            if (this.overlays == null)
            {
                stream.Write((byte)0);
            }
            else
            {
                stream.Write((byte)overlays.Count);
                for (int i = 0; i < overlays.Count; i++)
                {
                    stream.Write((byte)overlays[i].ID);
                }
            }

            stream.Write((byte)equippedSlots.Count);
            ForEachEquippedItem(item =>
            {
                stream.Write((byte)item.slot);
                item.WriteEquipProperties(stream);
            });
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.state = (NPCStates)stream.ReadByte();

            this.hpmax = stream.ReadUShort();
            this.hp = stream.ReadUShort();

            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                Overlay ov;
                int id = stream.ReadByte();
                if (this.Model.TryGetOverlay(id, out ov))
                {
                    this.ScriptObject.ApplyOverlay(ov);
                }
                else
                {
                    throw new Exception("Overlay not found: " + id);
                }
            }

            count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                Item item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
                int slot = stream.ReadByte();
                item.ReadEquipProperties(stream);
                this.ScriptObject.AddItem(item);
                this.ScriptObject.EquipItem(slot, item);
            }
        }

        #endregion

        #region Animations

        partial void pJump();
        public void Jump()
        {
            if (!this.IsSpawned)
                return;

            if (this.IsDead)
                return;

            pJump();
            this.state = NPCStates.MoveForward;
        }

        #region Overlays
        List<Overlay> overlays = null;

        /// <summary>
        /// Checks whether the overlay with the given number is applied.
        /// </summary>
        /// <param name="num">0-255</param>
        public bool IsOverlayApplied(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlays != null)
                return overlays.Contains(overlay);
            return false;
        }

        partial void pAddOverlay(Overlay overlay);
        /// <summary>
        /// Applies an overlay.
        /// </summary>
        /// <param name="num">0-255</param>
        public void ApplyOverlay(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (this.IsDead)
                return;

            if (overlays != null)
            {
                if (overlays.Contains(overlay))
                    return;
                //overlays.Remove(overlay); // so it's on top
                overlays.Add(overlay);
            }
            else
            {
                overlays = new List<Overlay>(1);
                overlays.Add(overlay);
            }
            pAddOverlay(overlay);
        }

        partial void pRemoveOverlay(Overlay overlay);
        /// <summary>
        /// Removes an overlay.
        /// </summary>
        /// <param name="num">0-255</param>
        public void RemoveOverlay(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (this.IsDead)
                return;

            if (overlays == null || !overlays.Remove(overlay))
                return;

            pRemoveOverlay(overlay);
        }

        #endregion

        GUCTimer aniTimer;
        Action onStop;

        Animation currentAni = null;
        public Animation CurrentAni { get { return this.currentAni; } }
        public bool IsInAnimation { get { return this.currentAni != null; } }

        public bool TryGetAniFromJob(AniJob job, out Animation ani)
        {
            if (overlays != null)
                for (int i = overlays.Count - 1; i >= 0; i--)
                {
                    if (job.TryGetOverlayAni(overlays[i], out ani))
                        return true;

                }
            ani = job.DefaultAni;
            return ani != null;
        }

        partial void pStartAnimation(Animation ani);
        public void StartAnimation(Animation ani, Action OnStop = null)
        {
            if (ani == null)
                throw new ArgumentNullException("Ani is null!");

            if (ani.AniJob == null)
                throw new ArgumentException("Ani is from no AniJob!");

            if (ani.AniJob.Model != this.Model)
                throw new ArgumentException("Ani is not for this NPC's Model!");

            if (!this.IsSpawned)
                throw new Exception("NPC is not spawned!");

            if (this.IsDead)
                return;

            this.currentAni = ani;
            this.onStop = OnStop;
            aniTimer.SetInterval(ani.Duration);
            aniTimer.Restart();

            pStartAnimation(ani);
        }

        partial void pStopAnimation(bool fadeOut);
        public void StopAnimation(bool fadeOut = false)
        {
            if (!this.IsInAnimation)
                return;

            pStopAnimation(fadeOut);
            aniTimer.Stop(true);
            this.currentAni = null;
        }

        partial void pEndAni();
        void EndAni()
        {
            pEndAni();
            aniTimer.Stop();
            this.currentAni = null;
            if (this.onStop != null)
                this.onStop();
        }

        #endregion

        #region Mob using

        MobInter usedMob = null;
        public MobInter UsedMob { get { return this.usedMob; } }


        public void UseMob(MobInter mob)
        {

        }

        #endregion

        partial void pDespawn();
        public override void Despawn()
        {
            pDespawn();
            this.currentAni = null;
            this.aniTimer.Stop();
            base.Despawn();
        }
    }
}
