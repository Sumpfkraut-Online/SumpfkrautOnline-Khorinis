﻿using System;
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

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
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

        #endregion

        #region Properties

        new public NPC BaseInst { get { return (NPC)base.BaseInst; } }
        public ItemInventory BaseInventory { get { return BaseInst.Inventory; } }

        public new NPCDef Definition { get { return (NPCDef)base.Definition; } set { base.Definition = value; } }
        
        public NPCMovement Movement { get { return this.BaseInst.Movement; } }
        public BaseVob.Environment Environment { get { return this.BaseInst.GetEnvironment(); } }

        public bool IsDead { get { return this.BaseInst.IsDead; } }

        public int HP { get { return this.BaseInst.HP; } }

        public bool UseCustoms = false;
        public HumBodyTexs CustomBodyTex;
        public HumHeadMeshs CustomHeadMesh;
        public HumHeadTexs CustomHeadTex;
        public HumVoices CustomVoice;
        public float CustomFatness = 0;
        public Vec3f CustomScale = new Vec3f(1, 1, 1);
        public string CustomName;

        #endregion
        
        public void SetState(NPCMovement state)
        {
            //this.BaseInst.SetMovement(state);
        }
        
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

        ItemInst armor;
        public ItemInst Armor { get { return this.armor; } }
        ItemInst meleeWep;
        public ItemInst MeleeWeapon { get { return this.meleeWep; } }
        ItemInst rangedWep;
        public ItemInst RangedWeapon { get { return this.rangedWep; } }
        ItemInst ammo;
        public ItemInst Ammo { get { return this.ammo; } }

        ItemInst drawnWeapon;
        public ItemInst DrawnWeapon { get { return this.drawnWeapon; } }
        
        public enum SlotNums
        {
            Torso,
            Sword,
            Longsword,
            Righthand,
            Lefthand,
            Bow,
            XBow,
            AmmoBow,
            AmmoXBow
        }

        public void EquipItem(int slot, Item item)
        {
            this.EquipItem(slot, (ItemInst)item.ScriptObject);
        }

        partial void pEquipItem(int slot, ItemInst item);
        public void EquipItem(int slot, ItemInst item)
        {
            pEquipItem(slot, item);

            if (item.BaseInst.Slot == slot)
                return;

            if (item.BaseInst.IsEquipped)
            {
                switch ((SlotNums)item.BaseInst.Slot)
                {
                    case SlotNums.Torso:
                        this.armor = null;
                        break;
                    case SlotNums.Sword:
                    case SlotNums.Longsword:
                        this.meleeWep = null;
                        break;
                    case SlotNums.Bow:
                    case SlotNums.XBow:
                        this.rangedWep = null;
                        break;
                    case SlotNums.AmmoBow:
                    case SlotNums.AmmoXBow:
                        this.ammo = null;
                        break;
                    case SlotNums.Righthand:
                    case SlotNums.Lefthand:
                        this.drawnWeapon = null;
                        break;
                }
            }

            switch ((SlotNums)slot)
            {
                case SlotNums.Torso:
                    this.armor = item;
                    break;
                case SlotNums.Sword:
                case SlotNums.Longsword:
                    this.meleeWep = item;
                    break;
                case SlotNums.Bow:
                case SlotNums.XBow:
                    this.rangedWep = item;
                    break;
                case SlotNums.AmmoBow:
                case SlotNums.AmmoXBow:
                    this.ammo = item;
                    break;
                case SlotNums.Righthand:
                case SlotNums.Lefthand:
                    this.drawnWeapon = item;
                    break;
            }

            this.BaseInst.EquipItem(slot, item.BaseInst);
        }

        public void UnequipItem(Item item)
        {
            this.UnequipItem((ItemInst)item.ScriptObject);
        }

        partial void pUnequipItem(ItemInst item);
        public void UnequipItem(ItemInst item)
        {
            pUnequipItem(item);

            switch ((SlotNums)item.BaseInst.Slot)
            {
                case SlotNums.Torso:
                    this.armor = null;
                    break;
                case SlotNums.Sword:
                case SlotNums.Longsword:
                    this.meleeWep = null;
                    break;
                case SlotNums.Bow:
                case SlotNums.XBow:
                    this.rangedWep = null;
                    break;
                case SlotNums.AmmoBow:
                case SlotNums.AmmoXBow:
                    this.ammo = null;
                    break;
                case SlotNums.Righthand:
                case SlotNums.Lefthand:
                    this.drawnWeapon = null;
                    break;
            }

            this.BaseInst.UnequipItem(item.BaseInst);
        }

        public void EquipItem(ItemInst item)
        {
            SlotNums slot;

            switch (item.ItemType)
            {
                case ItemTypes.Armor:
                    slot = SlotNums.Torso;
                    break;
                case ItemTypes.Wep1H:
                    slot = SlotNums.Sword;
                    break;
                case ItemTypes.Wep2H:
                    slot = SlotNums.Longsword;
                    break;
                case ItemTypes.WepBow:
                    slot = SlotNums.Bow;
                    break;
                case ItemTypes.WepXBow:
                    slot = SlotNums.XBow;
                    break;
                case ItemTypes.AmmoBow:
                    slot = SlotNums.AmmoBow;
                    break;
                case ItemTypes.AmmoXBow:
                    slot = SlotNums.AmmoXBow;
                    break;
                default:
                    return;

            }
            EquipItem((int)slot, item);
        }

        #endregion

        #region Health

        public void SetHealth(int hp)
        {
            this.SetHealth(hp, BaseInst.HPMax);
        }

        partial void pSetHealth(int hp, int hpmax);
        public void SetHealth(int hp, int hpmax)
        {
            this.BaseInst.SetHealth(hp, hpmax);
            pSetHealth(hp, hpmax);
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
        }

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
    }
}
