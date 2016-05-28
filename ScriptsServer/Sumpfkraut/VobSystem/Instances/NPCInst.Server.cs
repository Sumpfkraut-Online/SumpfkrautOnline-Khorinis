using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripting;
using GUC.Enumeration;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        // TFFA
        public override void OnPosChanged()
        {
            if (this.BaseInst.IsPlayer && !this.BaseInst.IsDead && this.BaseInst.GetPosition().Y < -400)
                Server.Scripts.TFFA.TFFAGame.Kill((TFFA.TFFAClient)this.BaseInst.Client.ScriptObject);
        }

        public NPCInst(NPCDef def) : base(def, new WorldObjects.NPC())
        {
            pConstruct();
        }

        public void OnCmdEquipItem(int slot, WorldObjects.Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdUnequipItem(WorldObjects.Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdUseMob(WorldObjects.Mobs.MobInter mob)
        {
            throw new NotImplementedException();
        }

        public void OnCmdUseItem(WorldObjects.Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdDrawItem(WorldObjects.Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdPickupItem(WorldObjects.Item item)
        {
            throw new NotImplementedException();
        }

        public void OnCmdDropItem(WorldObjects.Item item)
        {
            throw new NotImplementedException();
        }

        partial void pConstruct()
        {
            this.hitTimer = new GUCTimer(CalcHit);
            this.comboTimer = new GUCTimer(AbleCombo);
        }

        public void OnCmdMove(MoveState state)
        {
            //TFFA
            if (Server.Scripts.TFFA.TFFAGame.Status == TFFA.TFFAPhase.Waiting)
                return;

            if (state == this.Movement)
                return;

            if (canCombo)
            {
                var aa = this.GetFightAni();
                if (aa != null)
                    this.StopAnimation(aa);
            }

            this.SetState(state);
        }

        partial void pSetHealth(int hp, int hpmax)
        {
            if (hp <= 0)
            {
                if (hitTimer.NextCallTime - GameTime.Ticks < 100000)
                {
                    hitTimer.Stop(true);
                }
                else
                {
                    hitTimer.Stop(false);
                }
                comboTimer.Stop(false);
            }
        }

        GUCTimer hitTimer;
        GUCTimer comboTimer;
        bool canCombo = true;

        void AbleCombo()
        {
            comboTimer.Stop();
            if (this.Movement != MoveState.Stand)
            {
                this.StopAnimation(this.GetFightAni());
            }
            else
            {
                canCombo = true;
            }
        }

        public delegate void OnHitHandler(NPCInst attacker, NPCInst target, int damage);
        public static event OnHitHandler sOnHit;

        void CalcHit()
        {
            hitTimer.Stop();

            ScriptAniJob attackerAni = (ScriptAniJob)this.GetFightAni()?.Ani.AniJob.ScriptObject;

            Vec3f attPos = this.BaseInst.GetPosition();
            Vec3f attDir = this.BaseInst.GetDirection();
            float range = this.DrawnWeapon.Definition.Range + this.Model.Radius + ModelDef.LargestNPC.Radius;
            this.BaseInst.World.ForEachNPCRoughInRange(attPos, range, npc =>
            {
                NPCInst target = (NPCInst)npc.ScriptObject;
                if (target != this && !target.BaseInst.IsDead)
                {
                    Vec3f targetPos = npc.GetPosition();
                    Vec3f targetDir = npc.GetDirection();
                    float realRange = this.DrawnWeapon.Definition.Range + this.Model.Radius + target.Model.Radius;

                    ScriptAniJob targetAni = (ScriptAniJob)target.GetFightAni()?.Ani.AniJob.ScriptObject;

                    if (targetAni != null && targetAni.IsDodge)
                        realRange /= 2.0f;

                    if ((targetPos - attPos).GetLength() <= realRange) // target is in range
                    {
                        if (targetPos.Y + target.Model.Height / 2.0f >= attPos.Y && targetPos.Y - target.Model.Height / 2.0f <= attPos.Y) // same height
                        {
                            Vec3f dir = (attPos - targetPos).Normalise();
                            float dot = attDir.Z * dir.Z + dir.X * attDir.X;

                            if (dot < -0.2f) // target is in front of attacker
                            {
                                float dist = attDir.X * (targetPos.Z - attPos.Z) - attDir.Z * (targetPos.X - attPos.X);
                                dist = (float)Math.Sqrt(dist * dist / (attDir.X * attDir.X + attDir.Z * attDir.Z));

                                if (dist <= target.Model.Radius + 10.0f) // distance to attack direction is smaller than radius + 10
                                {
                                    dir = (targetPos - attPos).Normalise();
                                    dot = targetDir.Z * dir.Z + dir.X * targetDir.X;

                                    if (targetAni != null && targetAni.IsParade && dot <= -0.2f) // PARRY
                                    {
                                        var strm = this.BaseInst.GetScriptVobStream();
                                        strm.Write((byte)Networking.NetVobMsgIDs.ParryMessage);
                                        strm.Write((ushort)npc.ID);
                                        this.BaseInst.SendScriptVobStream(strm);
                                    }
                                    else // HIT
                                    {
                                        var strm = this.BaseInst.GetScriptVobStream();
                                        strm.Write((byte)Networking.NetVobMsgIDs.HitMessage);
                                        strm.Write((ushort)npc.ID);
                                        this.BaseInst.SendScriptVobStream(strm);

                                        int damage = (this.DrawnWeapon.Definition.Damage + attackerAni.AttackBonus) - target.Armor.Definition.Protection;
                                        if (this.GetJumpAni() != null || this.Environment == EnvironmentState.InAir) // Jump attaaaack!
                                            damage += 5;

                                        if (damage > 0)
                                        {
                                            if (sOnHit != null)
                                                sOnHit(this, target, damage);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        public void OnCmdAniStart(Animations.Animation ani)
        {
            //TFFA
            if (Server.Scripts.TFFA.TFFAGame.Status == TFFA.TFFAPhase.Waiting)
                return;

            ScriptAni anim = (ScriptAni)ani.ScriptObject;
            ScriptAniJob job = anim.AniJob;

            if ((this.Environment > EnvironmentState.Wading || this.GetJumpAni() != null) && !job.IsAttackRun)
            {
                return;
            }

            if (!this.canCombo) // can't combo yet
                return;

            if (job.IsFightMove) // FIGHT MOVE
            {
                if (job.IsAttack) // new move is an attack
                {
                    ScriptAniJob curAni = (ScriptAniJob)this.GetFightAni()?.Ani.AniJob.ScriptObject;
                    if (curAni != null && curAni.IsAttack) // currently in an attack
                    {
                        if (curAni == job) // same attack
                            return;

                        if (curAni.IsAttackCombo && job.ID <= curAni.ID)
                            return;
                    }

                    hitTimer.SetInterval(anim.HitTime);
                    hitTimer.Start();
                }

                if (job.IsAttack)
                {
                    comboTimer.SetInterval(anim.ComboTime);
                    comboTimer.Start();
                }

                this.StartAnimation(anim, () => this.canCombo = true);
                this.canCombo = false;
            }
            else if (job.IsJump)
            {
                if (GetActiveAniFromLayerID(anim.Layer) != null)
                    return;

                this.StartAniJump(anim, 50, 300);
            }
        }

        public void OnCmdAniStart(Animations.Animation ani, object[] netArgs)
        {
            if (GetActiveAniFromLayerID(ani.LayerID) != null)
                return;

            ScriptAni a = (ScriptAni)ani.ScriptObject;
            if (a.AniJob.IsClimbing)
            {
                this.StartAniClimb(a, (WorldObjects.NPC.ClimbingLedge)netArgs[0]);
            }
        }

        public void OnCmdAniStop(bool fadeOut)
        {
        }

        public override void Despawn()
        {
            hitTimer.Stop();
            comboTimer.Stop();
            base.Despawn();
        }

        public void StartAniJump(ScriptAni ani, int fwdVelocity, int upVelocity)
        {
            this.BaseInst.StartAnimation(ani.BaseAni, null, fwdVelocity, upVelocity);
        }

        public void StartAniClimb(ScriptAni ani, WorldObjects.NPC.ClimbingLedge ledge)
        {
            this.BaseInst.StartAnimation(ani.BaseAni, () => this.canCombo = true, ledge);
            this.canCombo = false;
        }
    }
}
