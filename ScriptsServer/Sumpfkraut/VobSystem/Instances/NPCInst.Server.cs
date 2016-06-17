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
        const long TimeBeforeRegen = 10 * TimeSpan.TicksPerSecond;
        const long RegenTime = 500 * TimeSpan.TicksPerMillisecond;
        const int RegenHP = 5;

        // TFFA
        Vec3f lastPos;
        public override void OnPosChanged()
        {
            Vec3f pos = this.BaseInst.GetPosition();
            if (this.BaseInst.IsPlayer && !this.BaseInst.IsDead)
            {
                if (pos.Y < -400)
                {
                    Server.Scripts.TFFA.TFFAGame.Kill((TFFA.TFFAClient)this.BaseInst.Client.ScriptObject, true);
                }
                else
                {
                    if (pos.GetDistance(lastPos) < 20 && !this.IsInAni() && this.BaseInst.HP < this.BaseInst.HPMax) // not moving & hurt
                    {
                        if (!regenTimer.Started)
                        {
                            regenTimer.SetInterval(TimeBeforeRegen);
                            regenTimer.Start();
                        }
                    }
                    else
                    {
                        regenTimer.Stop();
                    }
                }
            }
            lastPos = pos;
        }

        GUCTimer regenTimer;
        void Regenerate()
        {
            if (this.BaseInst.IsDead || this.IsInAni())
            {
                regenTimer.Stop();
                return;
            }

            int hp = this.BaseInst.HP + RegenHP;
            if (hp > this.BaseInst.HPMax)
            {
                hp = this.BaseInst.HPMax;
                regenTimer.Stop();
            }
            else
            {
                regenTimer.SetInterval(RegenTime);
            }
            this.SetHealth(hp);
        }

        public NPCInst(NPCDef def) : base(def, new WorldObjects.NPC())
        {
            pConstruct();
        }

        public void OnCmdEquipItem(int slot, WorldObjects.Item item)
        {
            ItemInst ii = (ItemInst)item.ScriptObject;

            this.EquipItem(slot, ii);
        }

        public void OnCmdUnequipItem(WorldObjects.Item item)
        {
            ItemInst ii = (ItemInst)item.ScriptObject;

            this.UnequipItem(ii);
        }

        public void OnCmdSetFightMode(bool fightMode)
        {
            if (!this.BaseInst.IsDead)
                this.SetFightMode(fightMode);
        }

        public void OnCmdUseMob(WorldObjects.Mobs.MobInter mob)
        {
            throw new NotImplementedException();
        }

        public void OnCmdUseItem(WorldObjects.Item item)
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
            this.regenTimer = new GUCTimer(Regenerate);
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
                hitTimer.Stop(false);
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
                var aa = this.GetFightAni();
                if (aa != null)
                    this.StopAnimation(aa);
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
            try
            {
                hitTimer.Stop();

                if (this.BaseInst.IsDead || this.drawnWeapon == null)
                    return;

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
                                                target.regenTimer.Stop();
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
            catch (Exception e)
            {
                Log.Logger.Log("CalcHit of npc " + this.ID + " " + this.BaseInst.HP + " " + this.IsInAni() + " " + e);
            }
        }

        public void OnCmdAniStart(Animations.Animation ani)
        {
            //TFFA
            if (Server.Scripts.TFFA.TFFAGame.Status == TFFA.TFFAPhase.Waiting)
                return;

            ScriptAni anim = (ScriptAni)ani.ScriptObject;
            ScriptAniJob job = anim.AniJob;

            if (job.IsFightMove) // FIGHT MOVE
            {
                if (!this.canCombo) // can't combo yet
                    return;

                if (this.GetDrawAni() != null || this.GetUndrawAni() != null || this.GetClimbAni() != null)
                    return;

                if (this.GetJumpAni() != null && !job.IsAttackRun)
                    return;

                if (job.IsAttack) // new move is an attack
                {
                    ScriptAniJob curFightAni = (ScriptAniJob)this.GetFightAni()?.Ani.AniJob.ScriptObject;
                    if (curFightAni != null && curFightAni.IsAttack) // currently in an attack
                    {
                        if (curFightAni == job) // same attack
                            return;

                        if (curFightAni.IsAttackCombo && job.ID <= curFightAni.ID)
                            return;
                    }

                    hitTimer.SetInterval(anim.HitTime);
                    hitTimer.Start();
                }

                comboTimer.SetInterval(anim.ComboTime);
                comboTimer.Start();

                this.StartAnimation(anim, () => this.canCombo = true);
                this.canCombo = false;
            }
            else if (job.IsJump)
            {
                if (this.Environment > EnvironmentState.Wading)
                    return;

                if (GetActiveAniFromLayerID(anim.Layer) != null)
                    return;

                this.StartAniJump(anim, 50, 300);
            }
        }

        public void OnCmdAniStart(Animations.Animation ani, object[] netArgs)
        {
            ScriptAni a = (ScriptAni)ani.ScriptObject;
            if (a.AniJob.IsClimb)
            {
                if (this.IsInAni())
                    return;

                this.StartAniClimb(a, (WorldObjects.NPC.ClimbingLedge)netArgs[0]);
            }
            else if (a.AniJob.IsDraw)
            {
                int itemID = (int)netArgs[0];
                WorldObjects.Item item;
                if (this.BaseInst.Inventory.TryGetItem(itemID, out item) && item.IsEquipped)
                {
                    var ii = (ItemInst)item.ScriptObject;
                    ScriptAniJob job;
                    ScriptAni drawAni;
                    if (this.TryGetDrawFromType(ii.ItemType, out job, this.Movement != MoveState.Stand || this.Environment != EnvironmentState.None)
                        && this.TryGetAniFromJob(job, out drawAni) && this.GetActiveAniFromLayerID(drawAni.Layer) == null)
                    {
                        this.StartAniDraw(drawAni, ii);
                    }
                }
            }
            else if (a.AniJob.IsUndraw)
            {
                int itemID = (int)netArgs[0];
                WorldObjects.Item item;
                if (this.BaseInst.Inventory.TryGetItem(itemID, out item) && item.IsEquipped)
                {
                    var ii = (ItemInst)item.ScriptObject;
                    ScriptAniJob job;
                    ScriptAni drawAni;
                    if (this.TryGetUndrawFromType(ii.ItemType, out job, this.Movement != MoveState.Stand || this.Environment != EnvironmentState.None)
                        && this.TryGetAniFromJob(job, out drawAni) && this.GetActiveAniFromLayerID(drawAni.Layer) == null)
                    {
                        this.StartAniUndraw(drawAni, ii);
                    }
                }
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

        GUCTimer drawTimer = new GUCTimer();

        public void StartAniDraw(ScriptAni ani, ItemInst item)
        {
            this.BaseInst.StartAnimation(ani.BaseAni, null, item.ID);

            drawTimer.SetInterval(ani.DrawTime);
            drawTimer.SetCallback(() =>
            {
                drawTimer.Stop();

                if (this.BaseInst.IsDead)
                    return;

                if (item.ItemType == ItemTypes.WepBow)
                {
                    this.EquipItem(SlotNums.Lefthand, item);
                }
                else
                {
                    this.EquipItem(SlotNums.Righthand, item);
                }

                if (item.IsWeapon)
                    this.SetFightMode(true);
            });

            drawTimer.Start();
        }

        public void StartAniUndraw(ScriptAni ani, ItemInst item)
        {
            this.BaseInst.StartAnimation(ani.BaseAni, null, item.ID);

            drawTimer.SetInterval(ani.DrawTime);
            drawTimer.SetCallback(() =>
            {
                drawTimer.Stop();

                if (this.BaseInst.IsDead)
                    return;

                this.EquipItem(item);
                if (item.IsWeapon)
                    this.SetFightMode(false);
            });

            drawTimer.Start();
        }
    }
}
