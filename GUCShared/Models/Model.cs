using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Animations;
using GUC.GameObjects;
using System.Collections;

namespace GUC.Models
{
    public partial class Model : GameObject
    {
        #region ScriptObject

        public partial interface IScriptModel : IScriptGameObject
        {
            void ApplyOverlay(Overlay overlay);
            void RemoveOverlay(Overlay overlay);

            ActiveAni StartAniJob(AniJob aniJob, float fpsMult);
            void StopAnimation(ActiveAni ani, bool fadeOut);
            void StartAniJobUncontrolled(AniJob job);
        }

        public new IScriptModel ScriptObject { get { return (IScriptModel)base.ScriptObject; } }

        #endregion

        #region Constructors

        partial void pConstruct();
        internal Model(Vob vob, IScriptModel scriptObject) : base(scriptObject)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            this.vob = vob;
            pConstruct();
        }

        #endregion

        #region Properties

        Vob vob;
        /// <summary> The Vob of this Model. </summary>
        public Vob Vob { get { return this.vob; } }

        public ModelInstance Instance { get { return this.vob.ModelInstance; } }

        #endregion

        #region Overlays

        List<Overlay> overlays;

        #region IsOverlayApplied

        /// <summary> Checks whether the overlay with the given id is applied. </summary>
        /// <param name="id">[0..255]</param>
        public bool IsOverlayApplied(int id)
        {
            if (overlays != null)
                for (int i = 0; i < overlays.Count; i++)
                    if (overlays[i].ID == id)
                        return true;

            return false;
        }

        /// <summary> Checks whether the specified overlay is applied. </summary>
        public bool IsOverlayApplied(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlays != null)
                return overlays.Contains(overlay);
            return false;
        }

        #endregion

        #region Apply & Remove

        partial void pAddOverlay(Overlay overlay);
        /// <summary> Applies an overlay to this vob. </summary>
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
        /// <summary> Removes the specified overlay. </summary>
        public void RemoveOverlay(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlays == null || !overlays.Remove(overlay))
                return;

            pRemoveOverlay(overlay);
        }

        /// <summary> Removes an overlay with the given id. </summary>
        /// <param name="id">[0..255]</param>
        public void RemoveOverlay(int id)
        {
            if (overlays != null)
                for (int i = 0; i < overlays.Count; i++)
                    if (overlays[i].ID == id)
                    {
                        pRemoveOverlay(overlays[i]);
                        overlays.RemoveAt(i);
                        return;
                    }
        }

        #endregion

        /// <summary> Checks this Vob's applied Overlays and returns the right Animation or null for the specified AniJob. </summary>
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
        internal void EndAni(Animation ani)
        {
            pEndAni(ani);
        }

        List<ActiveAni> activeAnis = new List<ActiveAni>();

        #region Access

        /// <summary> Loops through all active animations of this vob. </summary>
        public void ForEachActiveAni(Action<ActiveAni> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null)
                    action(activeAnis[i]);
        }

        /// <summary>
        /// Loops through all active animations of this vob.
        /// Let the predicate return FALSE to BREAK the loop.
        /// </summary>
        public void ForEachActiveAniPredicate(Predicate<ActiveAni> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null)
                    if (!predicate(activeAnis[i]))
                        return;
        }

        /// <summary> Checks whether this vob is in any animation. </summary>
        public bool IsInAnimation()
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null)
                    return true;
            return false;
        }

        /// <summary> Returns an ActiveAni-Object or null of the given Animation-ID. </summary>
        public ActiveAni GetActiveAniFromAniID(int aniID)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null && activeAnis[i].Ani.AniJob.ID == aniID)
                    return activeAnis[i];
            return null;
        }

        /// <summary> Returns an ActiveAni-Object or null of the given Animation. </summary>
        public ActiveAni GetActiveAniFromAni(Animation ani)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null && activeAnis[i].Ani == ani)
                    return activeAnis[i];
            return null;
        }

        /// <summary> Returns an ActiveAni-Object or null of the given AniJob. </summary>
        public ActiveAni GetActiveAniFromAniJob(AniJob aniJob)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null && activeAnis[i].Ani.AniJob == aniJob)
                    return activeAnis[i];
            return null;
        }

        /// <summary> Returns an ActiveAni-Object or null of the given Animation-Layer. </summary>
        /// <param name="layer">[0..255]</param>
        public ActiveAni GetActiveAniFromLayerID(int layer)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null && activeAnis[i].AniJob.Layer == layer)
                    return activeAnis[i];
            return null;
        }

        #endregion

        #endregion

        #region Start & Stop Animation

        partial void pStartUncontrolledAni(AniJob aniJob);
        /// <summary>
        /// Starts the AniJob without controlling the process. I.e. it can be interrupted by Gothic (f.e. falling).
        /// </summary>
        public void StartUncontrolledAni(AniJob aniJob)
        {
            if (!this.vob.IsSpawned)
                throw new Exception("Vob is not spawned!");

            if (aniJob == null)
                throw new ArgumentNullException("AniJob is null!");

            if (aniJob.ModelInstance != this.Instance)
                throw new ArgumentException("AniJob is not for this Model!");

            pStartUncontrolledAni(aniJob);            
        }


        /// <summary> 
        /// Starts the given AniJob and calls onStop at the end of the animation. 
        /// Returns a handle to the active animation or null if the animation can't be played (f.e. by not having the right overlays applied).
        /// </summary>
        public ActiveAni StartAniJob(AniJob aniJob)
        {
            return StartAniJob(aniJob, 1.0f, null);
        }

        partial void pStartAnimation(ActiveAni aa, float fpsMult);
        /// <summary> 
        /// Starts the given Animation with the given frame speed multiplier value and calls onStop at the end of the animation. 
        /// Returns false if the Animation can't be played (f.e not the right overlays applied). 
        /// </summary>
        public ActiveAni StartAniJob(AniJob aniJob, float fpsMult, FrameActionPair[] pairs)
        {
            ActiveAni aa = PlayAni(aniJob, fpsMult, pairs);
            pStartAnimation(aa, fpsMult);
            return aa;
        }
        
        ActiveAni PlayAni(AniJob aniJob, float fpsMult, FrameActionPair[] pairs)
        {
            if (!this.vob.IsSpawned)
                throw new Exception("Vob is not spawned!");

            if (aniJob == null)
                throw new ArgumentNullException("AniJob is null!");

            if (aniJob.ModelInstance != this.Instance)
                throw new ArgumentException("AniJob is not for this Model!");

            if (fpsMult <= 0)
                throw new ArgumentException("Frame speed multiplier has to be greater than zero!");

            Animation ani;
            if (!this.TryGetAniFromJob(aniJob, out ani))
                return null;

            // search a free ActiveAni
            ActiveAni aa = null;
            for (int i = 0; i < activeAnis.Count; i++)
            {
                if (activeAnis[i].Ani == null) // this ActiveAni is unused
                {
                    aa = activeAnis[i];
                    // continue to search, maybe there's an active ani with the same layer
                }
                else if (activeAnis[i].AniJob.Layer == aniJob.Layer) // same layer, stop this animation
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

            aa.Start(ani, fpsMult, pairs);
            return aa;
        }

        partial void pStopAnimation(ActiveAni aa, bool fadeOut);
        public void StopAnimation(ActiveAni ani, bool fadeOut = false)
        {
            if (ani == null)
                throw new ArgumentNullException("ActiveAni is null!");

            if (ani.Ani == null)
                return;

            if (ani.Model != this)
                throw new ArgumentException("ActiveAni is not from this Model!");

            pStopAnimation(ani, fadeOut);
            ani.Stop();
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            // FIXME: Read active overlays & animations

            // read applied overlays
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                Overlay ov;
                int id = stream.ReadByte();
                if (this.Instance.TryGetOverlay(id, out ov))
                {
                    this.ScriptObject.ApplyOverlay(ov);
                }
                else
                {
                    throw new Exception("Overlay not found: " + id);
                }
            }
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            // FIXME: Write active overlays & animations

            // applied overlays
            if (this.overlays == null)
            {
                stream.Write((byte)0);
            }
            else
            {
                stream.Write((byte)overlays.Count);
                for (int i = 0; i < overlays.Count; i++)
                {
                    stream.Write((byte)overlays[i].ID);
                }
            }
        }

        #endregion

        partial void pOnTick(long now);
        internal void OnTick(long now)
        {
            for (int i = 0; i < activeAnis.Count; i++)
                if (activeAnis[i].Ani != null)
                    activeAnis[i].OnTick(now);

            pOnTick(now);
        }
    }
}
