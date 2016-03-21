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

            void OnCmdApplyOverlay(Overlay overlay);
            void OnCmdRemoveOverlay(Overlay overlay);

            void OnCmdStartAni(AniJob job);
            void OnCmdStopAni(bool fadeOut);
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

        #endregion

        #region Movement / NPCStates

        NPCStates state = NPCStates.Stand;
        public NPCStates State { get { return this.state; } }

        partial void pSetState(NPCStates state);
        public void SetState(NPCStates state)
        {
            pSetState(state);
            this.state = state;
        }

        partial void pFallDown();
        public void FallDown()
        {
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

            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                Overlay ov;
                int id = stream.ReadByte();
                if (this.Model.TryGetOverlay(id, out ov))
                {
                    this.ScriptObject.OnCmdApplyOverlay(ov);
                }
                else
                {
                    throw new Exception("Overlay not found: " + id);
                }
            }
        }

        #endregion

        #region Animations

        partial void pJump();
        public void Jump()
        {
            if (this.IsSpawned)
            {
                pJump();
            }
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

        Animation GetAniFromJob(AniJob job)
        {
            if (overlays != null)
                for (int i = overlays.Count - 1; i >= 0; i--)
                {
                    Animation ani;
                    if (job.TryGetOverlayAni(overlays[i], out ani))
                        return ani;

                }
            return job.DefaultAni;
        }

        partial void pStartAnimation(Animation ani);
        public void StartAnimation(AniJob job, Action OnStop = null)
        {
            if (job == null)
                throw new ArgumentNullException("AniJob is null!");

            if (job.Model != this.Model)
                throw new ArgumentException("AniJob is not for this NPC's Model!");

            if (!this.IsSpawned)
                throw new Exception("NPC is not spawned!");

            Animation ani = GetAniFromJob(job);
            this.currentAni = ani;
            this.onStop = OnStop;
            aniTimer.SetInterval(ani.Duration * TimeSpan.TicksPerMillisecond);
            aniTimer.Start();

            pStartAnimation(ani);
        }

        partial void pStopAnimation(bool fadeOut);
        public void StopAnimation(bool fadeOut = false)
        {
            if (!this.IsInAnimation)
                return;

            pStopAnimation(fadeOut);
            aniTimer.Stop();
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
    }
}
