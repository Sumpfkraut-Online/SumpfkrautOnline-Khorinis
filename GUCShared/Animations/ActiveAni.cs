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

        List<TimeActionPair> actionPairs = new List<TimeActionPair>(1);

        void AddPair(float frame, Action callback, long time)
        {
            TimeActionPair pair = new TimeActionPair();
            pair.Frame = frame;
            pair.Callback = callback;
            pair.Time = time < 0 ? long.MaxValue : time;
            for (int i = 0; i < actionPairs.Count; i++)
                if (time > actionPairs[i].Time)
                {
                    actionPairs.Insert(i, pair);
                    return;
                }
            actionPairs.Add(pair);
        }

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

                if (pairs != null && pairs.Length > 0)
                {
                    for (int i = pairs.Length - 1; i >= 0; i--)
                    {
                        FrameActionPair pair = pairs[i];
                        if (pair.Callback != null)
                        {
                            AddPair(pair.Frame, pair.Callback, startTime + (long)(pair.Frame * coeff));
                        }
                    }
                }
            }
            else // idle ani
            {
                endTime = -1;
            }
        }

        internal void Stop()
        {
            this.ani = null;
            this.actionPairs.Clear();
        }

        internal void OnTick(long now)
        {
            if (!this.IsIdleAni)
            {
                if (now < endTime) // still playing
                {
                    for (int i = actionPairs.Count - 1; i >= 0; i--)
                    {
                        if (now < actionPairs[i].Time)
                            break;

                        actionPairs[i].Callback();
                        if (this.ani == null) // ani is stopped
                            break;

                        actionPairs.RemoveAt(i);
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
                    actionPairs.ForEach(p => p.Callback());
                    actionPairs.Clear();

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
                for (int i = 0; i < actionPairs.Count; i++)
                {
                    var pair = actionPairs[i];

                    pair.Frame = pair.Frame - elapsedFrames; // new frame
                    pair.Time = startTime + (long)(pair.Frame * coeff);
                }
            }
            else
            {
                endTime = -1;
            }

            this.ani = nextAni;
        }
    }
}
