using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAni : ScriptObject, Animation.IScriptAnimation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">Duration in ms.</param>
        public ScriptAni(int duration, int startFrame = 0) : this()
        {
            this.Duration = duration;
            this.StartFrame = startFrame;
        }

        #region FightAni

        int comboTime = 0;
        public int ComboTime
        {
            get { return this.comboTime; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("ComboTime can't be changed when the object is created.");
                this.comboTime = value;
            }
        }
        
        public static ScriptAni NewFightAni(int duration, int comboTime = 0, int startFrame = 0)
        {
            var ani = new ScriptAni(duration, startFrame);
            ani.comboTime = comboTime == 0 ? duration : comboTime;
            return ani;
        }

        #region AttackAni

        int hitTime = 0;
        public int HitTime
        {
            get { return this.hitTime; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("HitTime can't be changed when the object is created.");
                this.hitTime = value;
            }
        }

        public static ScriptAni NewAttackAni(int duration, int hitTime = 0, int comboTime = 0, int startFrame = 0)
        {
            var ani = NewFightAni(duration, comboTime, startFrame);
            ani.hitTime = hitTime == 0 ? duration : hitTime;
            return ani;
        }

        #endregion

        #endregion

        #region DrawAni / UndrawAni

        public static ScriptAni NewDrawAni(int duration, int drawTime = 0, int startFrame = 0)
        {
            var ani = new ScriptAni(duration, startFrame);
            ani.drawTime = drawTime == 0 ? duration : drawTime;
            return ani;
        }

        #endregion
    }
}
