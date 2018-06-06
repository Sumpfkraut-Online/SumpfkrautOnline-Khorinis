using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.ItemContainers;
using GUC.WorldObjects;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;
using GUC.WorldObjects.ItemContainers;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public enum NPCSlots
    {
        OneHanded1,
        OneHanded2,
        TwoHanded,
        Ranged,
        Ammo,
        Armor,

        LeftHand,
        RightHand
    }

    public enum JumpMoves
    {
        Fwd,
        Run,
        Up
    }

    public enum ClimbMoves
    {
        Low,
        Mid,
        High
    }

    public enum FightMoves
    {
        None,

        Fwd,
        Left,
        Right,
        Run,

        Dodge,
        Parry
    }

    public enum Unconsciousness
    {
        None,
        Front,
        Back
    }

    public partial class NPCInst : VobInst, NPC.IScriptNPC, ScriptInventory.IContainer
    {
        #region Constructors

        partial void pConstruct();
        public NPCInst()
        {
            pConstruct();
        }

        protected override BaseVob CreateVob()
        {
            return new NPC(new ScriptInventory(this), new ModelInst(this), this);
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new NPCInstEffectHandler(null, null, this);
        }

        #endregion

        #region Properties

        new public NPCInstEffectHandler EffectHandler { get { return (NPCInstEffectHandler)base.EffectHandler; } }

        new public NPC BaseInst { get { return (NPC)base.BaseInst; } }
        public ItemInventory BaseInventory { get { return BaseInst.Inventory; } }
        public ScriptInventory Inventory { get { return (ScriptInventory)BaseInventory.ScriptObject; } }

        public new NPCDef Definition { get { return (NPCDef)base.Definition; } set { base.Definition = value; } }

        public NPCMovement Movement { get { return this.BaseInst.Movement; } }
        public VobEnvironment Environment { get { return this.BaseInst.Environment; } }

        public bool IsDead { get { return this.BaseInst.IsDead; } }
        public bool IsInFightMode { get { return this.BaseInst.IsInFightMode; } }

        public bool IsWading
        {
            get
            {
                float waterLevel = Environment.WaterLevel;
                return waterLevel > 0 && waterLevel < 0.4f;
            }
        }

        public bool IsSwimming
        {
            get
            {
                float waterLevel = Environment.WaterLevel;
                return waterLevel > 0 && waterLevel >= 0.4f;
            }
        }

        public int HP { get { return this.BaseInst.HP; } }
        public int HPMax { get { return this.BaseInst.HPMax; } }

        public bool UseCustoms = false;
        public HumBodyTexs CustomBodyTex;
        public HumHeadMeshs CustomHeadMesh;
        public HumHeadTexs CustomHeadTex;
        public HumVoices CustomVoice;
        public float CustomFatness = 0;
        public Vec3f CustomScale = new Vec3f(1, 1, 1);
        public string CustomName;



        #endregion

        public void OnWriteTakeControl(PacketWriter stream)
        {
            // write everything the player needs to know about this npc
            // i.e. abilities, level, guild etc
        }

        public void OnReadTakeControl(PacketReader stream)
        {
            // read everything the player needs to know about this npc
            // i.e. abilities, level, guild etc
        }

        #region Equipment

        public ItemInst LastUsedWeapon;

        public ItemInst GetEquipmentBySlot(NPCSlots slotNum)
        {
            return this.BaseInst.TryGetEquippedItem((int)slotNum, out Item item) ? (ItemInst)item.ScriptObject : null;
        }

        public ItemInst GetArmor() { return GetEquipmentBySlot(NPCSlots.Armor); }
        public ItemInst GetAmmo() { return GetEquipmentBySlot(NPCSlots.Ammo); }
        public ItemInst GetRightHand() { return GetEquipmentBySlot(NPCSlots.RightHand); }
        public ItemInst GetLeftHand() { return GetEquipmentBySlot(NPCSlots.LeftHand); }
        public bool HasItemInHands() { return GetRightHand() != null || GetLeftHand() != null; }

        public ItemInst GetDrawnWeapon()
        {
            ItemInst item;
            if (((item = GetRightHand()) != null && item.IsWeapon)
             || ((item = GetLeftHand()) != null && item.IsWeapon))
                return item;
            return null;
        }

        public void EquipItem(int slot, Item item)
        {
            this.EquipItem((NPCSlots)slot, (ItemInst)item.ScriptObject);
        }

        public delegate void OnEquipHandler(ItemInst item);
        public event OnEquipHandler OnEquip;

        partial void pBeforeEquip(NPCSlots slot, ItemInst item);
        partial void pAfterEquip(NPCSlots slot, ItemInst item);
        public void EquipItem(NPCSlots slot, ItemInst item)
        {
            if (item.BaseInst.Slot == (int)slot)
                return;
            
            pBeforeEquip(slot, item);
            this.BaseInst.EquipItem((int)slot, item.BaseInst);
            pAfterEquip(slot, item);

            OnEquip?.Invoke(item);
        }

        public void UnequipItem(Item item)
        {
            this.UnequipItem((ItemInst)item.ScriptObject);
        }
        public event OnEquipHandler OnUnequip;

        partial void pBeforeUnequip(ItemInst item);
        partial void pAfterUnequip(ItemInst item);
        public void UnequipItem(ItemInst item)
        {
            pBeforeUnequip(item);
            this.BaseInst.UnequipItem(item.BaseInst);
            pAfterUnequip(item);

            OnUnequip?.Invoke(item);
        }

        public void UnequipSlot(NPCSlots slot)
        {
            ItemInst item = GetEquipmentBySlot(slot);
            if (item != null)
                UnequipItem(item);
        }

        #endregion

        #region Health

        public delegate void OnDeathHandler(NPCInst npc);
        public static event OnDeathHandler sOnDeath;
        public event OnDeathHandler OnDeath;

        public void SetHealth(int hp)
        {
            this.SetHealth(hp, BaseInst.HPMax);
        }

        public int GetHealth()
        {
            return this.BaseInst.HP;
        }

        partial void pSetHealth(int hp, int hpmax);
        public void SetHealth(int hp, int hpmax)
        {
            this.BaseInst.SetHealth(hp, hpmax);
            pSetHealth(hp, hpmax);

            if (hp <= 0)
            {
                this.uncon = Unconsciousness.None;
                OnDeath?.Invoke(this);
                sOnDeath?.Invoke(this);
            }
        }

        #endregion

        public override void OnReadProperties(PacketReader stream)
        {
            base.OnReadProperties(stream);
            UseCustoms = stream.ReadBit();
            if (UseCustoms)
            {
                CustomBodyTex = (HumBodyTexs)stream.ReadByte();
                CustomHeadMesh = (HumHeadMeshs)stream.ReadByte();
                CustomHeadTex = (HumHeadTexs)stream.ReadByte();
                CustomVoice = (HumVoices)stream.ReadByte();
                CustomFatness = stream.ReadFloat();
                CustomScale = stream.ReadVec3f();
                CustomName = stream.ReadString();
            }

            this.uncon = (Unconsciousness)stream.ReadByte();
            this.TeamID = stream.ReadSByte();
        }

        // ARENA
        public int TeamID = -1;

        public override void OnWriteProperties(PacketWriter stream)
        {
            base.OnWriteProperties(stream);
            if (UseCustoms)
            {
                stream.Write(true);
                stream.Write((byte)CustomBodyTex);
                stream.Write((byte)CustomHeadMesh);
                stream.Write((byte)CustomHeadTex);
                stream.Write((byte)CustomVoice);
                stream.Write(CustomFatness);
                stream.Write(CustomScale);
                stream.Write(CustomName == null ? "" : CustomName);
            }
            else
            {
                stream.Write(false);
            }

            stream.Write((byte)this.uncon);
            stream.Write((sbyte)TeamID);
        }

        #region FightMode

        partial void pSetFightMode(bool fightMode);
        public void SetFightMode(bool fightMode)
        {
            this.BaseInst.SetFightMode(fightMode);
            pSetFightMode(fightMode);
        }

        #endregion

        partial void pDespawn();
        public override void Despawn()
        {
            pDespawn();
            base.Despawn();
        }

        partial void pBeforeSpawn();
        partial void pAfterSpawn();
        public override void Spawn(WorldInst world, Vec3f pos, Angles ang)
        {
            pBeforeSpawn();
            base.Spawn(world, pos, ang);
            pAfterSpawn();
        }

        Unconsciousness uncon = Unconsciousness.None;
        public bool IsUnconscious { get { return uncon != Unconsciousness.None; } }
    }
}
