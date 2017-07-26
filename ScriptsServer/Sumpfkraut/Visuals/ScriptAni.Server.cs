using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAni : ExtendedObject, Animation.IScriptAnimation
    {
        #region Constructors

        public ScriptAni(int startFrame, int endFrame) : this()
        {
            this.StartFrame = startFrame;
            this.EndFrame = endFrame;
        }

        #endregion

        #region Properties

        #region Special Frames

        Dictionary<SpecialFrame, float> specialFrames;

        public bool TryGetSpecialFrame(SpecialFrame index, out float value)
        {
            return specialFrames.TryGetValue(index, out value);
        }

        public void SetSpecialFrame(SpecialFrame index, float value)
        {
            if (specialFrames == null)
            {
                specialFrames = new Dictionary<SpecialFrame, float>(1);
            }
            else if (specialFrames.ContainsKey(index))
            {
                specialFrames[index] = value;
                return;
            }
            specialFrames.Add(index, value);
        }

        #endregion

        #endregion
    }
}
