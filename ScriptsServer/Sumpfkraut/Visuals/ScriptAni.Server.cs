using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAni : ScriptObject, Animation.IScriptAnimation
    {
        #region Constructors

        public ScriptAni(int startFrame, int endFrame) : this()
        {
            this.StartFrame = startFrame;
            this.EndFrame = endFrame;
        }

        #endregion
        
    }
}
