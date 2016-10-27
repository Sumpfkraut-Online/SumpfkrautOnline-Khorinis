using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.Models;
using GUC.GameObjects;

namespace GUC.Animations
{
    /// <summary>
    /// An Animation of an Overlay contains frame and layer information for the animation system.
    /// </summary>
    public class Animation : GameObject
    {
        #region ScriptObject

        public interface IScriptAnimation : IScriptGameObject
        {
        }

        new public IScriptAnimation ScriptObject { get { return (IScriptAnimation)base.ScriptObject; } }

        #endregion

        #region Constructors

        public Animation(IScriptAnimation scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        void CanChangeNow()
        {
            if (this.aniJob != null && this.aniJob.ModelInstance != null && this.aniJob.ModelInstance.IsCreated)
                throw new NotSupportedException("Can't change value when the Animation's ModelInstace is already created!");
        }

        float startFrame = 0;
        /// <summary>
        /// From which frame the gothic animation should start.
        /// </summary>
        public float StartFrame
        {
            get { return this.startFrame; }
            set
            {
                CanChangeNow();
                if (value < 0)
                    throw new ArgumentOutOfRangeException("StartFrame needs to be greater than or zero! Is " + value);

                this.startFrame = value;
            }
        }

        float endFrame = 0;
        /// <summary>
        /// At what frame the gothic animation should end. EndFrame = 0 plays the whole animation
        /// </summary>
        public float EndFrame
        {
            get { return this.endFrame; }
            set
            {
                CanChangeNow();
                if (value < 0)
                    throw new ArgumentOutOfRangeException("EndFrame needs to be greater than or zero! Is " + value);

                this.endFrame = value;
            }
        }

        float fps = 25;
        /// <summary>
        /// The the frame speed.
        /// </summary>
        public float FPS
        {
            get { return this.fps; }
            set
            {
                CanChangeNow();
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("FPS needs to be greater than zero! Is " + value);

                this.fps = value;
            }
        }

        /// <summary>
        /// The number of frames of this Animation.
        /// </summary>
        public float GetFrameNum() { return this.endFrame - this.startFrame; }

        /// <summary> Calculates the duration of this animation in ticks (1/10000ms). </summary>
        public long GetDuration()
        {
            return GetDuration(1.0f);
        }

        /// <summary> Calculates the duration of this animation in ticks (1/10000ms). </summary>
        /// <param name="fpsMult"> Frame speed multiplier. </param>
        public long GetDuration(float fpsMult)
        {
            return (long)((this.endFrame - this.startFrame) / (fps * fpsMult) * TimeSpan.TicksPerSecond);
        }

        Overlay overlay;
        /// <summary>
        /// The Overlay of this Animation.
        /// </summary>
        public Overlay Overlay { get { return this.overlay; } }

        AniJob aniJob;
        /// <summary>
        /// The associated AniJob of this Animation.
        /// </summary>
        public AniJob AniJob { get { return this.aniJob; } }

        /// <summary>
        /// Is true if this Animation is added to an AniJob.
        /// </summary>
        public bool IsCreated { get { return this.aniJob != null; } }

        internal void SetAniJob(AniJob job, Overlay overlay)
        {
            this.aniJob = job;
            this.overlay = overlay;
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            this.startFrame = stream.ReadFloat();
            this.endFrame = stream.ReadFloat();
            this.fps = stream.ReadFloat();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write(startFrame);
            stream.Write(endFrame);
            stream.Write(fps);
        }

        #endregion
    }
}
