using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Animations;
using GUC.Scripting;

namespace GUC.WorldObjects.AnimatedVob
{
    public abstract partial class AnimatedVob : Vob
    {
        #region Overlays

        List<Overlay> overlays = null;

        /// <summary>
        /// Checks whether the overlay with the given number is applied.
        /// </summary>
        /// <param name="num">0-255</param>
        public bool IsOverlayApplied(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlays != null)
                return overlays.Contains(overlay);
            return false;
        }

        partial void pAddOverlay(Overlay overlay);
        /// <summary>
        /// Applies an overlay.
        /// </summary>
        /// <param name="num">0-255</param>
        public void ApplyOverlay(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlays != null)
            {
                if (overlays.Contains(overlay))
                    return;
                //overlays.Remove(overlay); // so it's on top
                overlays.Add(overlay);
            }
            else
            {
                overlays = new List<Overlay>() { overlay };
            }
            pAddOverlay(overlay);
        }

        partial void pRemoveOverlay(Overlay overlay);
        /// <summary>
        /// Removes an overlay.
        /// </summary>
        /// <param name="num">0-255</param>
        public void RemoveOverlay(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlays == null || !overlays.Remove(overlay))
                return;

            pRemoveOverlay(overlay);
        }

        #endregion

        #region Get Animation

        /// <summary>
        /// Checks this Vob's applied Overlays and returns the right Animation or null.
        /// </summary>
        public bool TryGetAniFromJob(AniJob job, out Animation ani)
        {
            if (overlays != null)
                for (int i = 0; i < overlays.Count; i++)
                {
                    if (job.TryGetOverlayAni(overlays[i], out ani))
                        return true;
                }
            ani = job.DefaultAni;
            return ani != null;
        }

        #endregion

        #region Active Animation

        partial void pEndAni(Animation ani);

        public class ActiveAni
        {
            AnimatedVob vob;
            /// <summary> The animated vob which is playing this animation. </summary>
            public AnimatedVob Vob { get { return this.vob; } }

            GUCTimer timer;

            float fps;
            Animation ani;
            public Animation Ani { get { return this.ani; } }
            
            Action onStop;
            public Action OnStop { get { return this.onStop; } }

            internal ActiveAni(AnimatedVob vob)
            {
                this.vob = vob;
                this.timer = new GUCTimer(this.EndAni);
            }

            void EndAni()
            {
                vob.pEndAni(this.Ani);
                this.timer.Stop();
                this.ani = null;
                if (this.OnStop != null)
                    this.OnStop();
            }

            public float GetCurrentFrame()
            {
                float numFrames = ani.GetFrameNum();
                if (numFrames > 0)
                {
                    return (float)timer.GetElapsedTicks() / (float)TimeSpan.TicksPerSecond * fps / numFrames;
                }
                return 0;
            }

            internal void Start(Animation ani, float fps, Action onStop)
            {
                this.ani = ani;
                this.fps = fps;
                this.onStop = onStop;

                float numFrames = ani.GetFrameNum();
                if (numFrames > 0)
                {
                    timer.SetInterval((long)(numFrames / fps * TimeSpan.TicksPerSecond));
                    timer.Start();
                }
            }

            internal void Stop()
            {
                timer.Stop(true);
            }
        }

        List<ActiveAni> activeAnis = new List<ActiveAni>();

        public void ForEachActiveAni(Action<ActiveAni> action)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null)
                    action(activeAnis[i]);
        }

        public void ForEachActiveAni(Predicate<ActiveAni> action)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null)
                    if (!action(activeAnis[i]))
                        return;
        }

        public bool IsInAnimation()
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null)
                    return true;
            return false;
        }

        public ActiveAni GetActiveAniFromAniID(int aniID)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null && activeAnis[i].Ani.AniJob.ID == aniID)
                    return activeAnis[i];
            return null;
        }

        public ActiveAni GetActiveAniFromLayerID(int layerID)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null && activeAnis[i].Ani.Layer == layerID)
                    return activeAnis[i];
            return null;
        }

        #endregion
        
        public void StartAnimation(Animation ani, Action onStop = null)
        {
            if (ani == null)
                throw new ArgumentNullException("Ani is null!");

            StartAnimation(ani, ani.FPS, onStop);
        }

        partial void pStartAnimation(Animation ani, float fps);
        public void StartAnimation(Animation ani, float fps, Action onStop = null)
        {
            PlayAni(ani, fps, onStop);
            pStartAnimation(ani, fps);
        }

        void PlayAni(Animation ani, float fps, Action onStop)
        {
            if (!this.IsSpawned)
                throw new Exception("AnimatedVob is not spawned!");

            if (ani == null)
                throw new ArgumentNullException("Ani is null!");

            if (ani.AniJob == null)
                throw new ArgumentException("Ani is from no AniJob!");

            if (ani.AniJob.Model != this.Model)
                throw new ArgumentException("AniJob is not for this NPC's Model!");

            if (fps <= 0)
                throw new ArgumentException("FPS has to be greater than zero!");

            // search a free ActiveAni
            ActiveAni aa = null;
            for (int i = 0; i < activeAnis.Count; i++)
            {
                if (activeAnis[i].Ani == null) // this ActiveAni is unused
                {
                    aa = activeAnis[i];
                    // continue to search, maybe there's an active ani with the same layer
                }
                else if (activeAnis[i].Ani.Layer == ani.Layer) // same layer, stop this animation
                {
                    aa = activeAnis[i];
                    aa.Stop(); // stop this animation
                    break;
                }
            }

            if (aa == null) // no free ActiveAni, create a new one
            {
                aa = new ActiveAni(this);
                activeAnis.Add(aa);
            }

            aa.Start(ani, fps, onStop);
        }

        partial void pStopAnimation(Animation ani, bool fadeOut);
        public void StopAnimation(ActiveAni ani, bool fadeOut = false)
        {
            if (ani == null)
                return;

            if (ani.Vob != this)
                throw new ArgumentException("ActiveAni is not from this Vob!");

            pStopAnimation(ani.Ani, fadeOut);
            ani.Stop();
        }
    }
}
