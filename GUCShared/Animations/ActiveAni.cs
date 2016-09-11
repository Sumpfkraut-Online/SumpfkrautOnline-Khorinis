using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Scripting;
using GUC.Models;

namespace GUC.Animations
{
    public partial class ActiveAni
    {
        Model model;
        /// <summary> The animated model which is playing this animation. </summary>
        public Model Model { get { return this.model; } }

        float fpsMult;
        /// <summary> The value with which the frame speed is multiplied. </summary>
        public float FrameSpeedMultiplier { get { return this.fpsMult; } }
        /// <summary> The frame speed with which the active animation is being played. </summary>
        public float FPS { get { return this.ani.FPS * this.fpsMult; } }

        Animation ani;
        /// <summary> The Animation which is being active. </summary>
        public Animation Ani { get { return this.ani; } }

        /// <summary> The AniJob of the active animation. </summary>
        public AniJob AniJob { get { return this.ani.AniJob; } }

        Action onStop;
        /// <summary> The Action which will be started at the end of the (last played if NextAni exists) animation. </summary>
        public Action OnStop { get { return this.onStop; } }

        GUCTimer timer;

        internal ActiveAni(Model model)
        {
            if (model == null)
                throw new ArgumentNullException("Model is null!");

            this.model = model;
            this.timer = new GUCTimer(this.EndAni);
        }

        void EndAni()
        {
            model.EndAni(ani);

            var nextAni = this.ani.AniJob.NextAni; // TODO!

            this.timer.Stop();
            this.ani = null;
            if (this.OnStop != null)
                this.OnStop();
        }

        /// <summary> Gets the active frame of this animation. </summary>
        public float GetCurrentFrame()
        {
            float numFrames = ani.GetFrameNum();
            if (numFrames > 0)
            {
                
                return (float)timer.GetElapsedTicks() / (float)TimeSpan.TicksPerSecond * (ani.FPS * fpsMult) / numFrames;
            }
            return 0;
        }

        public float GetPercent()
        {
            return timer.Started ? (float)timer.GetElapsedTicks() / timer.Interval : 0;
        }


        internal void Start(Animation ani, float fpsMult, Action onStop)
        {
            this.ani = ani;
            this.fpsMult = fpsMult;
            this.onStop = onStop;

            float numFrames = ani.GetFrameNum();
            if (numFrames > 0)
            {
                timer.SetInterval((long)(numFrames / (ani.FPS * fpsMult) * TimeSpan.TicksPerSecond));
                timer.Start();
            }
        }

        internal void Stop()
        {
            timer.Stop(true);
        }
    }
}
