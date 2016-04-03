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
        long comboTime = 0;

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

        void CalcHit()
        {
            Log.Logger.Log("Calc Hit!");
            hitTimer.Stop();
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
                comboTime = now + anim.ComboTime; // can combo from this time on
                
                if (anim.AniJob.IsAttack)
                {
                    hitTimer.SetInterval(anim.HitTime * TimeSpan.TicksPerMillisecond);
                    hitTimer.Start();
                }
            }

            this.StartAnimation(anim);
        }

        public void OnCmdAniStop(bool fadeOut)
        {
        }
    }
}
