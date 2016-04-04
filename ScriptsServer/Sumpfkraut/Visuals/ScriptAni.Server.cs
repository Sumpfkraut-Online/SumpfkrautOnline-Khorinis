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
        /// <param name="startPercent">255 = 100%</param>
        public ScriptAni(int duration, int startPercent = 0) : this()
        {
            this.Duration = duration;
            this.StartFrame = startPercent;
        }
    }
}
