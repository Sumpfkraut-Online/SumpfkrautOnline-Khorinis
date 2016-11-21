using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Scripting;
using GUC.Models;

namespace GUC.Animations
{
    /// <summary>
    /// Handle for an active animation (for progress etc.).
    /// </summary>
    public partial class ActiveAni
    {
        struct TimeActionPair
        {
            public float Frame;
            public long Time;
            public Action Callback;
        }

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

        long startTime;
        long endTime;

        public bool IsIdleAni { get { return this.endTime < 0; } }

        internal ActiveAni(Model model)
        {
            if (model == null)
                throw new ArgumentNullException("Model is null!");

            this.model = model;
        }

        public float GetProgress()
        {
            return endTime < 0 ? 0 : (float)(GameTime.Ticks - this.startTime) / (this.endTime - this.startTime);
        }

        TimeActionPair[] actionPairs = new TimeActionPair[0];
        int pairCount = 0;

        internal void Start(Animation ani, float fpsMult, FrameActionPair[] pairs)
        {
            this.ani = ani;
            this.fpsMult = fpsMult;

            float numFrames = ani.GetFrameNum();
            if (numFrames > 0)
            {
                float coeff = TimeSpan.TicksPerSecond / (ani.FPS * fpsMult);

                startTime = GameTime.Ticks;
                endTime = startTime + (long)(numFrames * coeff);
                if (pairs != null)
                {
                    if (pairs.Length > actionPairs.Length)
                        actionPairs = new TimeActionPair[pairs.Length];

                    pairCount = 0;
                    for (int i = 0; i < pairs.Length; i++)
                        if (pairs[i].Callback != null)
                        {
                            actionPairs[pairCount].Frame = pairs[i].Frame;
                            actionPairs[pairCount].Callback = pairs[i].Callback;
                            actionPairs[pairCount].Time = startTime + (long)(pairs[i].Frame * coeff);
                            pairCount++;
                        }
                }
                else
                {
                    pairCount = 0;
                }
            }
            else // idle ani
            {
                endTime = -1;
            }
        }

        internal void Stop()
        {
        }

        internal void OnTick(long now)
        {
            if (!this.IsIdleAni)
            {
                if (now < endTime) // still playing
                {
                    for (int i = 0; i < pairCount; i++) // check for frame actions
                        if (actionPairs[i].Callback != null && now >= actionPairs[i].Time)
                        {
                            actionPairs[i].Callback();
                            actionPairs[i].Callback = null;
                        }
                }
                else
                {
                    AniJob nextAniJob = this.AniJob.NextAni;
                    if (nextAniJob != null)
                    {
                        Animation nextAni;
                        if (model.TryGetAniFromJob(nextAniJob, out nextAni))
                        {
                            Continue(nextAni); // there is a nextAni, continue playing it
                            return;
                        }
                    }

                    // fire all remaining actions
                    for (int i = 0; i < pairCount; i++)
                        if (actionPairs[i].Callback != null)
                        {
                            actionPairs[i].Callback();
                            actionPairs[i].Callback = null;
                        }

                    // end this animation
                    model.EndAni(this.ani);
                    this.ani = null;
                }
            }
        }

        void Continue(Animation nextAni)
        {
            float numFrames = nextAni.GetFrameNum();
            if (numFrames > 0)
            {
                float coeff = TimeSpan.TicksPerSecond / (nextAni.FPS * fpsMult);

                this.startTime = this.endTime;
                this.endTime = this.startTime + (long)(numFrames * coeff);

                float elapsedFrames = this.ani.GetFrameNum();

                int newPairCount = 0;
                for (int i = 0; i < pairCount; i++)
                    if (actionPairs[i].Callback != null)
                    {
                        float newFrame = actionPairs[i].Frame - elapsedFrames;
                        actionPairs[newPairCount].Frame = newFrame;
                        actionPairs[newPairCount].Time = endTime + (long)(newFrame * coeff);
                        if (i != newPairCount)
                        {
                            actionPairs[newPairCount].Callback = actionPairs[i].Callback;
                            actionPairs[i].Callback = null;
                        }
                        newPairCount++;
                    }
                pairCount = newPairCount;
            }
            else
            {
                endTime = -1;
            }

            this.ani = nextAni;
        }
    }
}
