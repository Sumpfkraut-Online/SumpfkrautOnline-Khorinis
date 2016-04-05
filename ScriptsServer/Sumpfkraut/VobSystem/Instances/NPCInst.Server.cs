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

        NPCStates nextState = NPCStates.Stand;
        public void OnCmdMove(NPCStates state)
        {
            if (state == this.State)
                return;

            Log.Logger.Log("Move: " + state + " Current: " + this.State);
            this.nextState = state;
            if (!this.IsInAni)
            {
                this.SetState(state);
            }
        }

        GUCTimer hitTimer;
        GUCTimer comboTimer;
        bool canCombo = true;

        void AbleCombo()
        {
            comboTimer.Stop();
            canCombo = true;
            Log.Logger.Log("Combo " + this.nextState + " " + this.State);
            if (this.nextState != this.State)
            {
                this.StopAnimation();
            }
        }

        void CalcHit()
        {
            hitTimer.Stop();

            const float wepRange = 100;
            Vec3f attPos = this.BaseInst.GetPosition();
            Vec3f attDir = this.BaseInst.GetDirection();
            float range = wepRange + this.Model.Radius + ModelDef.LargestNPC.Radius;
            this.BaseInst.World.ForEachNPCRoughInRange(attPos, range, npc =>
            {
                NPCInst target = (NPCInst)npc.ScriptObject;
                if (target != this)
                {
                    Vec3f targetPos = npc.GetPosition();
                    float distance = (targetPos - attPos).GetLength();
                    if (distance <= wepRange + this.Model.Radius + target.Model.Radius) // target is in range
                    {
                        if (targetPos.Y + target.Model.Height / 2.0f >= attPos.Y && targetPos.Y - target.Model.Height / 2.0f <= attPos.Y) // same height
                        {
                            Vec3f dir = (attPos - targetPos).Normalise();
                            float dot = attDir.Z * dir.Z + dir.X * attDir.X;

                            if (dot <= -0.3f) // target is in front of attacker
                            {
                                float dist = attDir.X * (targetPos.Z - attPos.Z) - attDir.Z * (targetPos.X - attPos.X);
                                dist = (float)Math.Sqrt(dist * dist / (attDir.X * attDir.X + attDir.Z * attDir.Z));

                                if (dist <= target.Model.Radius) // distance to attack direction is smaller than radius
                                {
                                    var strm = this.BaseInst.GetScriptVobStream();
                                    strm.Write((byte)Networking.NetVobMsgIDs.HitMessage);
                                    strm.Write((ushort)npc.ID);
                                    this.BaseInst.SendScriptVobStream(strm);
                                }
                            }
                        }
                    }
                }
            });
        }

        public void OnCmdAniStart(Animations.Animation ani)
        {
            long now = GameTime.Ticks;

            if (!this.canCombo) // can't combo yet
                return;
            this.canCombo = false;

            ScriptAni anim = (ScriptAni)ani.ScriptObject;
            if (anim.AniJob.IsFightMove) // FIGHT MOVE
            {
                if (anim.AniJob.IsAttack) // new move is an attack
                {
                    ScriptAni curAni = this.CurrentAni;
                    if (curAni != null && curAni.AniJob.IsAttack) // currently in an attack
                    {
                        if (curAni.AniJob == anim.AniJob) // same attack
                            return;

                        if (curAni.AniJob.ID >= (int)SetAnis.Attack2HFwd1 && curAni.AniJob.ID <= (int)SetAnis.Attack2HFwd4 && anim.AniJob.ID <= curAni.AniJob.ID)
                            return;
                    }

                    hitTimer.SetInterval(anim.HitTime);
                    hitTimer.Start();
                }

                comboTimer.SetInterval(anim.ComboTime);
                comboTimer.Start();
            }

            this.StartAnimation(anim, () => { this.canCombo = true; if (this.nextState != this.State) this.SetState(this.nextState); });
        }

        public void OnCmdAniStop(bool fadeOut)
        {
        }
    }
}
