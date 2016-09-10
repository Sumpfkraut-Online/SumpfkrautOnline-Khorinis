using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Animations;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        #region Constructors

        public NPCInst(NPCDef def) : this()
        {
            this.Definition = def;
        }

        partial void pConstruct()
        {
            //this.hitTimer = new GUCTimer(CalcHit);
            //this.comboTimer = new GUCTimer(AbleCombo);
        }

        #endregion

        public NPCCatalog AniCatalog { get { return (NPCCatalog)this.ModelDef?.Catalog; } }
                
        #region Jumps

        public enum JumpTypes
        {
            Fwd,
            Run,
            Up
        }

        ActiveAni jumpAni;
        public void DoJump(JumpTypes type)
        {
            if (AniCatalog == null)
                return;

            if (this.jumpAni != null || this.IsDead /*|| this.Environment == WorldObjects.EnvironmentState.InAir*/)
                return;

            ScriptAniJob job;
            switch (type)
            {
                case JumpTypes.Fwd:
                    job = AniCatalog.Jumps.Fwd;
                    break;
                case JumpTypes.Run:
                    job = AniCatalog.Jumps.Run;
                    break;
                case JumpTypes.Up:
                    job = AniCatalog.Jumps.Up;
                    break;
                default:
                    return;
            }

            if (job == null)
                return;
            
            jumpAni = this.ModelInst.StartAnimation(job, () => jumpAni = null);
        }

        #endregion

        #region Fight Moves

        public enum FightMoves
        {
            Fwd,
            Left,
            Right,
            Run,

            Dodge,
            Parry
        }

        ActiveAni fightAni;
        public void DoFightMove(FightMoves move)
        {

        }

        #endregion

        #region Weapon Drawing



        #endregion

        public bool IsPlayer { get { return this.BaseInst.IsPlayer; } }

        


        partial void pSetHealth(int hp, int hpmax)
        {
            if (hp <= 0)
            {
               // hitTimer.Stop(false);
               // comboTimer.Stop(false);
            }
        }

        GUCTimer hitTimer;
        GUCTimer comboTimer;
        bool canCombo = true;

        /*void AbleCombo()
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

        public void Hit(NPCInst attacker, int damage)
        {
            var strm = this.BaseInst.GetScriptVobStream();
            strm.Write((byte)NetWorldMsgID.HitMessage);
            strm.Write((ushort)this.ID);
            this.BaseInst.SendScriptVobStream(strm);
            
            if (damage > 0)
            {
                if (sOnHit != null)
                    sOnHit(attacker, this, damage);
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
                this.BaseInst.World.ForEachNPCRough(attPos, range, npc =>
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
                                            strm.Write((byte)NetWorldMsgID.ParryMessage);
                                            strm.Write((ushort)npc.ID);
                                            this.BaseInst.SendScriptVobStream(strm);
                                        }
                                        else // HIT
                                        {
                                            int damage = (this.DrawnWeapon.Definition.Damage + attackerAni.AttackBonus) - (target.Armor == null ? 0 : target.Armor.Definition.Protection);
                                            if (this.GetJumpAni() != null || this.Environment == EnvironmentState.InAir) // Jump attaaaack!
                                                damage += 5;

                                            target.Hit(this, damage);
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
        }*/



    }
}
