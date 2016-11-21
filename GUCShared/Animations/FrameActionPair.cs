using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Animations
{
    public struct FrameActionPair
    {
        public float Frame;
        public Action Callback;

        public FrameActionPair(float frameNum, Action callback)
        {
            this.Frame = frameNum;
            this.Callback = callback;
        }

        public static FrameActionPair OnEnd(Action callback)
        {
            return new FrameActionPair(float.MaxValue, callback);
        }
    }
}
