using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Mobs;
using GUC.Network;
using GUC.Animations;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst : VobInst, NPC.IScriptNPC
    {
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

        public bool TryGetAttackFromMove(AttackMove attMove, out ScriptAniJob aniJob)
        {
            if (this.DrawnWeapon.ItemType == ItemTypes.Wep2H)
            {
                return this.Model.TryGetAniJob((int)attMove + 11, out aniJob);
            }
            else // 1h
            {
                return this.Model.TryGetAniJob((int)attMove, out aniJob);
            }
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
                if (((ScriptAniJob)a.Ani.AniJob.ScriptObject).IsClimbing)
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

        public ItemInst DrawnWeapon;
        public ItemInst Armor;

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

        public void RemoveItem(ItemInst item)
        {
            this.BaseInst.Inventory.Remove(item.BaseInst);
        }

        public void EquipItem(int slot, Item item)
        {
            this.EquipItem(slot, (ItemInst)item.ScriptObject);
        }

        partial void pEquipItem(ItemInst item);
        public void EquipItem(int slot, ItemInst item)
        {
            this.BaseInst.EquipItem(slot, item.BaseInst);
            pEquipItem(item);

            //TFFA
            if (slot == 1)
                DrawnWeapon = item;
            else if (slot == 0)
                Armor = item;
        }

        public void UnequipItem(Item item)
        {
            this.UnequipItem((ItemInst)item.ScriptObject);
        }

        partial void pUnequipItem(ItemInst item);
        public void UnequipItem(ItemInst item)
        {
            this.BaseInst.UnequipItem(item.BaseInst);
            pUnequipItem(item);
            if (item == DrawnWeapon)
                DrawnWeapon = null;
        }

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
        }

        public int visibleClientID = -1;

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
        }

        public void OnWriteAniStartArgs(PacketWriter stream, AniJob job, object[] netArgs)
        {
            ScriptAniJob j = (ScriptAniJob)job.ScriptObject;
            if (j.IsJump)
            {
                stream.Write((ushort)(int)netArgs[0]);
                stream.Write((ushort)(int)netArgs[1]);
            }
            else if (j.IsClimbing)
            {
                ((NPC.ClimbingLedge)netArgs[0]).WriteStream(stream);
            }
        }

        public void OnReadAniStartArgs(PacketReader stream, AniJob job, out object[] netArgs)
        {
            ScriptAniJob j = (ScriptAniJob)job.ScriptObject;
            if (j.IsJump)
            {
                netArgs = new object[2] { (int)stream.ReadUShort(), (int)stream.ReadUShort() };
            }
            else if (j.IsClimbing)
            {
                netArgs = new object[1] { new NPC.ClimbingLedge(stream) };
            }
            else
            {
                netArgs = null;
            }
        }
    }
}
