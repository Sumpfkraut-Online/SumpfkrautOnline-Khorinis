using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;
using Gothic.Objects;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        public override void Spawn(WorldInst world)
        {
            base.Spawn(world);
            if (UseCustoms)
            {
                using (var vec = Gothic.Types.zVec3.Create(ModelScale.X, 1, ModelScale.Z))
                    this.BaseInst.gVob.SetModelScale(vec);

                this.BaseInst.gVob.SetAdditionalVisuals(HumBodyMeshs.HUM_BODY_NAKED0.ToString(), (int)CustomBodyTex, 0, CustomHeadMesh.ToString(), (int)CustomHeadTex, 0, -1);
                this.BaseInst.gVob.Voice = (int)CustomVoice;
                this.BaseInst.gVob.SetFatness(Fatness);
            }

            // TFFA
            foreach (TFFA.ClientInfo ci in TFFA.ClientInfo.ClientInfos.Values)
            {
                if (ci.CharID == this.ID)
                    this.BaseInst.gVob.Name.Set(Client.Scripts.TFFA.InputControl.ClientsShown ? string.Format("({0}){1}", ci.ID, ci.Name) : ci.Name);
            }

            this.BaseInst.ForEachEquippedItem(i => this.pEquipItem(i.Slot, (ItemInst)i.ScriptObject));
        }

        #region Equipment

        partial void pEquipItem(int slot, ItemInst item)
        {
            if (!this.IsSpawned)
                return;

            oCNpc gNpc = this.BaseInst.gVob;
            oCItem gItem = item.BaseInst.gVob;

            if (item.BaseInst.IsEquipped)
            {
                pUnequipItem(item);
            }

            switch (slot)
            {
                case SlotNums.Sword:
                    gNpc.PutInSlot(oCNpc.NPCNodes.Sword, gItem, true);
                    break;

                case SlotNums.Longsword:
                    gNpc.PutInSlot(oCNpc.NPCNodes.Longsword, gItem, true);
                    break;

                case SlotNums.Torso:
                    gItem.VisualChange.Set(item.Definition.VisualChange);
                    gNpc.PutInSlot(oCNpc.NPCNodes.Torso, gItem, true);
                    break;

                case SlotNums.Righthand:
                    gNpc.PutInSlot(oCNpc.NPCNodes.RightHand, gItem, true);
                    break;

                default:
                    break;
            }
        }

        partial void pUnequipItem(ItemInst item)
        {
            if (!this.IsSpawned)
                return;

            oCNpc gNpc = this.BaseInst.gVob;
            oCItem gItem = item.BaseInst.gVob;

            switch (item.BaseInst.Slot)
            {
                case SlotNums.Sword:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Sword, true, true);
                    break;

                case SlotNums.Longsword:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Longsword, true, true);
                    break;

                case SlotNums.Torso:
                    gItem.VisualChange.Set(item.Definition.VisualChange);
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Torso, true, true);
                    break;

                case SlotNums.Righthand:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.RightHand, true, true);
                    break;

                default:
                    break;
            }
        }

        #endregion

        public override void OnReadScriptVobMsg(PacketReader stream)
        {
            var msgID = (Networking.NetVobMsgIDs)stream.ReadByte();
            switch (msgID)
            {
                case Networking.NetVobMsgIDs.HitMessage:
                    var targetID = stream.ReadUShort();
                    WorldObjects.BaseVob target;
                    if (WorldInst.Current.BaseWorld.TryGetVob(targetID, out target))
                    {
                        this.BaseInst.gVob.AniCtrl.CreateHit(target.gVob);
                    }
                    break;
                case Networking.NetVobMsgIDs.ParryMessage:
                    targetID = stream.ReadUShort();
                    WorldObjects.NPC targetNPC;
                    if (WorldInst.Current.BaseWorld.TryGetVob(targetID, out targetNPC))
                    {
                        this.BaseInst.gVob.AniCtrl.StartParadeEffects(targetNPC.gVob);
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnTick(long now)
        {
            var fightAni = (ScriptAniJob)this.GetFightAni()?.Ani.AniJob.ScriptObject;
            if (fightAni != null && fightAni.IsAttack)
            {
                this.BaseInst.gVob.AniCtrl.ShowWeaponTrail();
            }

            var activeJumpAni = GetJumpAni();
            if (activeJumpAni != null && activeJumpAni.GetPercent() >= 0.2f)
            {
                var gVob = this.BaseInst.gVob;
                var ai = gVob.HumanAI;
                if (((gVob.BitField1 & Gothic.Objects.zCVob.BitFlag0.physicsEnabled) != 0) && ai.AboveFloor <= 0)
                {
                    // LAND
                    int id = this.Movement == MoveState.Forward ? ai._t_jump_2_runl : ai._t_jump_2_stand;
                    ai.LandAndStartAni(gVob.GetModel().GetAniFromAniID(id));
                }
            }
        }

        public void StartAnimation(Animations.Animation ani, object[] netArgs)
        {
            ScriptAni a = (ScriptAni)ani.ScriptObject;

            if (a.AniJob.IsJump)
            {
                this.StartAniJump(a, (int)netArgs[0], (int)netArgs[1]);
            }
            else if (a.AniJob.IsClimb)
            {
                this.StartAniClimb(a, (WorldObjects.NPC.ClimbingLedge)netArgs[0]);
            }
        }

        public void StartAniJump(ScriptAni ani, int fwdVelocity, int upVelocity)
        {
            this.StartAnimation(ani);

            var ai = this.BaseInst.gVob.HumanAI;
            ai.AniCtrlBitfield &= ~(1 << 3);
            //this.BaseInst.gVob.SetBodyState(8);

            var vel = new Gothic.Types.zVec3(ai.Address + 0x90);
            var dir = this.BaseInst.GetDirection();

            vel.X = dir.X * fwdVelocity;
            vel.Z = dir.Z * fwdVelocity;
            vel.Y = upVelocity;

            this.BaseInst.SetPhysics(true);

            this.BaseInst.SetVelocity((Vec3f)vel);
        }

        public void StartAniClimb(ScriptAni ani, WorldObjects.NPC.ClimbingLedge ledge)
        {
            this.BaseInst.SetGClimbingLedge(ledge);
            this.StartAnimation(ani);
        }
    }
}
