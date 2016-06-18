using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Scripting;
using GUC.Network;
using GUC.Animations;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst : VobInst, NPC.IScriptNPC
    {
        #region Ranged Weapons

        bool isAiming = false;
        public bool IsAiming { get { return this.isAiming; } }

        #endregion
        
        public enum AttackMove // sync with SetAnis
        {
            Fwd1,
            Fwd2,
            Fwd3,
            Fwd4,

            Left,
            Right,

            Run,

            Dodge,
            Parry1,
            Parry2,
            Parry3
        }

        public bool TryGetDrawFromType(ItemTypes type, out ScriptAniJob aniJob, bool running = false)
        {
            SetAnis aniID;
            switch (type)
            {
                case ItemTypes.Wep1H:
                    aniID = running ? SetAnis.Draw1HRun : SetAnis.Draw1H;
                    break;
                case ItemTypes.Wep2H:
                    aniID = running ? SetAnis.Draw2HRun : SetAnis.Draw2H;
                    break;
                case ItemTypes.WepBow:
                    aniID = running ? SetAnis.DrawBowRun : SetAnis.DrawBow;
                    break;
                case ItemTypes.WepXBow:
                    aniID = running ? SetAnis.DrawXBowRun : SetAnis.DrawXBow;
                    break;
                default:
                    aniJob = null;
                    return false;
            }

            return this.Model.TryGetAniJob((int)aniID, out aniJob);
        }

        public bool TryGetUndrawFromType(ItemTypes type, out ScriptAniJob aniJob, bool running = false)
        {
            SetAnis aniID;
            switch (type)
            {
                case ItemTypes.Wep1H:
                    aniID = running ? SetAnis.Undraw1HRun : SetAnis.Undraw1H;
                    break;
                case ItemTypes.Wep2H:
                    aniID = running ? SetAnis.Undraw2HRun : SetAnis.Undraw2H;
                    break;
                case ItemTypes.WepBow:
                    aniID = running ? SetAnis.UndrawBowRun : SetAnis.UndrawBow;
                    break;
                case ItemTypes.WepXBow:
                    aniID = running ? SetAnis.UndrawXBowRun : SetAnis.UndrawXBow;
                    break;
                default:
                    aniJob = null;
                    return false;
            }

            return this.Model.TryGetAniJob((int)aniID, out aniJob);
        }

        public bool TryGetAttackFromMove(AttackMove attMove, out ScriptAniJob aniJob)
        {
            if (this.drawnWeapon != null)
            {
                if (this.DrawnWeapon.ItemType == ItemTypes.Wep2H)
                {
                    return this.Model.TryGetAniJob((int)attMove + 11, out aniJob);
                }
                else if (this.DrawnWeapon.ItemType == ItemTypes.Wep1H)
                {
                    return this.Model.TryGetAniJob((int)attMove, out aniJob);
                }
            }
            aniJob = null;
            return false;
        }

        public NPC.ActiveAni GetFightAni() // alternative: add a var and change on Start-/Stopanimation ?
        {
            NPC.ActiveAni aa = null;
            this.BaseInst.ForEachActiveAni(a =>
            {
                if (((ScriptAniJob)a.Ani.AniJob.ScriptObject).IsFightMove)
                {
                    aa = a;
                    return false;
                }
                return true;
            });
            return aa;
        }

        public NPC.ActiveAni GetJumpAni() // alternative: add a var and change on Start-/Stopanimation ?
        {
            NPC.ActiveAni aa = null;
            this.BaseInst.ForEachActiveAni(a =>
            {
                if (((ScriptAniJob)a.Ani.AniJob.ScriptObject).IsJump)
                {
                    aa = a;
                    return false;
                }
                return true;
            });
            return aa;
        }

        public NPC.ActiveAni GetClimbAni() // alternative: add a var and change on Start-/Stopanimation ?
        {
            NPC.ActiveAni aa = null;
            this.BaseInst.ForEachActiveAni(a =>
            {
                if (((ScriptAniJob)a.Ani.AniJob.ScriptObject).IsClimb)
                {
                    aa = a;
                    return false;
                }
                return true;
            });
            return aa;
        }

        public NPC.ActiveAni GetDrawAni() // alternative: add a var and change on Start-/Stopanimation ?
        {
            NPC.ActiveAni aa = null;
            this.BaseInst.ForEachActiveAni(a =>
            {
                if (((ScriptAniJob)a.Ani.AniJob.ScriptObject).IsDraw)
                {
                    aa = a;
                    return false;
                }
                return true;
            });
            return aa;
        }

        public NPC.ActiveAni GetUndrawAni() // alternative: add a var and change on Start-/Stopanimation ?
        {
            NPC.ActiveAni aa = null;
            this.BaseInst.ForEachActiveAni(a =>
            {
                if (((ScriptAniJob)a.Ani.AniJob.ScriptObject).IsUndraw)
                {
                    aa = a;
                    return false;
                }
                return true;
            });
            return aa;
        }

        public bool IsInFightMove()
        {
            return GetFightAni() != null;
        }

        #region Properties

        public new NPC BaseInst { get { return (NPC)base.BaseInst; } }

        public new NPCDef Definition { get { return (NPCDef)base.Definition; } }

        public ModelDef Model { get { return this.Definition.Model; } }

        public MoveState Movement { get { return this.BaseInst.Movement; } }
        public EnvironmentState Environment { get { return this.BaseInst.EnvState; } }

        public bool UseCustoms = false;
        public HumBodyTexs CustomBodyTex;
        public HumHeadMeshs CustomHeadMesh;
        public HumHeadTexs CustomHeadTex;
        public HumVoices CustomVoice;

        public float Fatness = 0;
        public Vec3f ModelScale = new Vec3f(1, 1, 1);

        #endregion

        partial void pConstruct();
        public NPCInst() : base(new NPC())
        {
            pConstruct();
        }

        public void SetState(MoveState state)
        {
            this.BaseInst.SetMovement(state);
        }

        #region Animations

        #region Overlays

        public void ApplyOverlay(Overlay overlay)
        {
            this.ApplyOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        public void ApplyOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.ApplyOverlay(overlay.BaseOverlay);
        }

        public void RemoveOverlay(Overlay overlay)
        {
            this.RemoveOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        public void RemoveOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.RemoveOverlay(overlay.BaseOverlay);
        }

        #endregion

        public bool TryGetAniFromJob(ScriptAniJob job, out ScriptAni ani)
        {
            Animation baseAni;
            if (this.BaseInst.TryGetAniFromJob(job.BaseAniJob, out baseAni))
            {
                ani = (ScriptAni)baseAni.ScriptObject;
                return true;
            }
            ani = null;
            return false;
        }

        public void StartAnimation(Animation ani)
        {
            this.StartAnimation((ScriptAni)ani.ScriptObject);
        }
        
        public void StartAnimation(ScriptAni ani, Action onStop = null)
        {
            if (ani.AniJob.ID == (int)SetAnis.BowAim || ani.AniJob.ID == (int)SetAnis.XBowAim)
            {
                this.BaseInst.StartAnimation(ani.BaseAni, () =>
                {
                    if (this.BaseInst.IsDead)
                        return;
                    this.isAiming = true;
                    if (onStop != null)
                        onStop();
                });
                return;
            }
            else if (ani.AniJob.ID == (int)SetAnis.BowLower || ani.AniJob.ID == (int)SetAnis.XBowLower || ani.AniJob.IsUndraw)
            {
                this.isAiming = false;
            }
            else if (ani.AniJob.ID == (int)SetAnis.BowReload || ani.AniJob.ID == (int)SetAnis.XBowReload)
            {
                this.isAiming = false;
                this.BaseInst.StartAnimation(ani.BaseAni, () =>
                {
                    if (this.BaseInst.IsDead)
                        return;
                    this.isAiming = true;
                    if (onStop != null)
                        onStop();
                });
                return;
            }

            this.BaseInst.StartAnimation(ani.BaseAni, onStop);
        }

        public void StopAnimation(NPC.ActiveAni ani, bool fadeOut = false)
        {
            this.BaseInst.StopAnimation(ani, fadeOut);
        }

        public bool IsInAni() { return this.BaseInst.IsInAnimation(); }
        public NPC.ActiveAni GetActiveAniFromAniID(int aniID) { return this.BaseInst.GetActiveAniFromAniID(aniID); }
        public NPC.ActiveAni GetActiveAniFromLayerID(int layerID) { return this.BaseInst.GetActiveAniFromLayerID(layerID); }

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

        #region Inventory

        public void AddItem(Item item)
        {
            this.AddItem((ItemInst)item.ScriptObject);
        }

        public void AddItem(ItemInst item)
        {
            this.BaseInst.Inventory.Add(item.BaseInst);
        }

        public void RemoveItem(Item item)
        {
            this.RemoveItem((ItemInst)item.ScriptObject);
        }

        partial void pRemoveItem(ItemInst item);
        public void RemoveItem(ItemInst item)
        {
            if (this.armor == item)
                this.armor = null;
            else if (this.meleeWep == item)
                this.meleeWep = null;
            else if (this.rangedWep == item)
                this.rangedWep = null;
            else if (this.ammo == item)
                this.ammo = null;
            else if (this.drawnWeapon == item)
                this.drawnWeapon = null;

            pRemoveItem(item);
            this.BaseInst.Inventory.Remove(item.BaseInst);
        }

        #endregion

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
                Fatness = stream.ReadFloat();
                ModelScale = stream.ReadVec3f();
            }

            this.isAiming = stream.ReadBit();
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
                stream.Write(Fatness);
                stream.Write(ModelScale);
            }
            else
            {
                stream.Write(false);
            }

            stream.Write(this.isAiming);
        }

        public void OnWriteAniStartArgs(PacketWriter stream, AniJob job, object[] netArgs)
        {
            ScriptAniJob j = (ScriptAniJob)job.ScriptObject;
            if (j.IsJump)
            {
                stream.Write((ushort)(int)netArgs[0]);
                stream.Write((ushort)(int)netArgs[1]);
            }
            else if (j.IsClimb)
            {
                ((NPC.ClimbingLedge)netArgs[0]).WriteStream(stream);
            }
            else if (j.IsDraw || j.IsUndraw)
            {
                stream.Write((byte)(int)netArgs[0]);
            }
        }

        public void OnReadAniStartArgs(PacketReader stream, AniJob job, out object[] netArgs)
        {
            ScriptAniJob j = (ScriptAniJob)job.ScriptObject;
            if (j.IsJump)
            {
                netArgs = new object[2] { (int)stream.ReadUShort(), (int)stream.ReadUShort() };
            }
            else if (j.IsClimb)
            {
                netArgs = new object[1] { new NPC.ClimbingLedge(stream) };
            }
            else if (j.IsDraw || j.IsUndraw)
            {
                netArgs = new object[1] { (int)stream.ReadByte() };
            }
            else
            {
                netArgs = null;
            }
        }

        partial void pSetFightMode(bool fightMode);
        public void SetFightMode(bool fightMode)
        {
            this.BaseInst.SetFightMode(fightMode);
            pSetFightMode(fightMode);
        }

        partial void pDespawn();
        public override void Despawn()
        {
            pDespawn();
            base.Despawn();
        }
    }
}
