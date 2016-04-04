using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripting;

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
        }

        GUCTimer hitTimer;
        long comboTime = 0;

        void CalcHit()
        {
            hitTimer.Stop();

            float range = 100;
            this.BaseInst.World.ForEachNPCInRange(this.BaseInst.GetPosition(), range, npc =>
            {
                if (npc.ScriptObject != this)
                {
                    var strm = this.BaseInst.GetScriptVobStream();
                    strm.Write((byte)Networking.NetVobMsgIDs.HitMessage);
                    strm.Write((ushort)npc.ID);
                    this.BaseInst.SendScriptVobStream(strm);
                }
            });
        }

        public void OnCmdAniStart(Animations.Animation ani)
        {
            long now = GameTime.Ticks;

            if (now < this.comboTime) // can't combo yet
                return;

            hitTimer.Stop(true); // just to be sure

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

                this.comboTime = now + anim.ComboTime; // can combo from this time on
            }

            this.StartAnimation(anim);
        }

        public void OnCmdAniStop(bool fadeOut)
        {
        }
    }
}
