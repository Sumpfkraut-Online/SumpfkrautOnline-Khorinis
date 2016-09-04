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

        public class ActiveAni
        {
            AnimatedVob vob;
            /// <summary> The animated vob which is playing this animation. </summary>
            public AnimatedVob Vob { get { return this.vob; } }

            GUCTimer timer;
            internal GUCTimer Timer { get { return this.timer; } }

            public Animation Ani { get; internal set; }
            internal Action OnStop;

            internal ActiveAni(AnimatedVob vob)
            {
                this.vob = vob;
                this.timer = new GUCTimer(this.EndAni);
            }

            void EndAni()
            {
                vob.pEndAni(this.Ani);
                this.timer.Stop();
                this.Ani = null;
                if (this.OnStop != null)
                    this.OnStop();
            }

            public float GetPercent()
            {
                return 1.0f - (float)(timer.NextCallTime - GameTime.Ticks) / (float)timer.Interval;
            }
        }

        List<ActiveAni> activeAnis = new List<ActiveAni>(1);

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
                if (activeAnis[i].Ani != null && activeAnis[i].Ani.LayerID == layerID)
                    return activeAnis[i];
            return null;
        }

        #endregion

        bool PlayAni(Animation ani, Action onStop)
        {
            if (ani == null)
                throw new ArgumentNullException("Ani is null!");

            if (ani.AniJob == null)
                throw new ArgumentException("Ani is from no AniJob!");

            if (ani.AniJob.Model != this.Model)
                throw new ArgumentException("Ani is not for this NPC's Model!");

            if (!this.IsSpawned)
                throw new Exception("NPC is not spawned!");
            
            ActiveAni aa = null;
            for (int i = 0; i < activeAnis.Count; i++)
            {
                if (activeAnis[i].Ani == null)
                {
                    aa = activeAnis[i];
                    break;
                }
                else if (activeAnis[i].Ani.LayerID == ani.LayerID)
                {
                    aa = activeAnis[i];
                    activeAnis[i].Timer.Stop(true);
                    break;
                }
            }

            if (aa == null)
            {
                aa = new ActiveAni(this);
                activeAnis.Add(aa);
            }

            aa.Ani = ani;
            aa.Timer.SetInterval(ani.Duration);
            aa.OnStop = onStop;
            aa.Timer.Start();
            return true;
        }

        partial void pStartAnimation(Animation ani);
        public void StartAnimation(Animation ani, Action onStop = null)
        {
            if (PlayAni(ani, onStop))
            {
                pStartAnimation(ani);
            }
        }

        partial void pEndAni(Animation ani);
        partial void pStopAnimation(Animation ani, bool fadeOut);
        public void StopAnimation(ActiveAni ani, bool fadeOut = false)
        {
            if (ani == null)
                return;

            if (ani.Vob != this)
                throw new ArgumentException("ActiveAni is not from this Vob!");

            pStopAnimation(ani.Ani, fadeOut);
            ani.Timer.Stop(true);
        }
    }
}
