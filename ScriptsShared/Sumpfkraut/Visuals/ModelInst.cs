using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Models;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ModelInst : ExtendedObject, GUCModelInst.IScriptModelInst
    {
        #region Constructors

        public ModelInst(VobInst vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            this.vob = vob;
        }

        #endregion

        #region Properties

        VobInst vob;
        public VobInst Vob { get { return this.vob; } }
        public GUCModelInst BaseInst { get { return this.vob.BaseInst.Model; } }

        #endregion

        #region Overlays

        public void ApplyOverlay(Overlay overlay)
        {
            ApplyOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        public void RemoveOverlay(Overlay overlay)
        {
            RemoveOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        public void ApplyOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.ApplyOverlay(overlay.BaseOverlay);
        }

        public void RemoveOverlay(ScriptOverlay overlay)
        {
            this.BaseInst.RemoveOverlay(overlay.BaseOverlay);
        }

        #endregion

        #region Animations

        public bool IsAniActive(ScriptAniJob job)
        {
            return this.BaseInst.GetActiveAniFromAniJob(job.BaseAniJob) != null;
        }

        public void ForEachActiveAni(Action<ActiveAni> action)
        {
            this.BaseInst.ForEachActiveAni(action);
        }

        public ActiveAni GetActiveAniFromLayer(int layer)
        {
            return this.BaseInst.GetActiveAniFromLayerID(layer);
        }

        public bool TryGetAniFromJob(ScriptAniJob aniJob, out ScriptAni ani)
        {
            if (this.BaseInst.TryGetAniFromJob(aniJob.BaseAniJob, out Animation baseAni))
            {
                ani = (ScriptAni)baseAni.ScriptObject;
                return true;
            }
            ani = null;
            return false;
        }

        public void StartAniJobUncontrolled(AniJob aniJob)
        {
            StartAniJobUncontrolled((ScriptAniJob)aniJob.ScriptObject);
        }
        
        public void StartAniJobUncontrolled(ScriptAniJob aniJob)
        {
            this.BaseInst.StartUncontrolledAni(aniJob.BaseAniJob);
        }

        public ActiveAni StartAniJob(AniJob aniJob, float fpsMult, float progress)
        {
            return StartAniJob((ScriptAniJob)aniJob.ScriptObject, fpsMult, progress);
        }

        public void StopAnimation(ActiveAni ani, bool fadeOut)
        {
            this.BaseInst.StopAnimation(ani, fadeOut);
        }

        public bool IsInAnimation()
        {
            return this.BaseInst.IsInAnimation();
        }


        public ActiveAni StartAniJob(ScriptAniJob aniJob, Action onEnd) { return StartAniJob(aniJob, 1, 0, FrameActionPair.OnEnd(onEnd)); }
        public ActiveAni StartAniJob(ScriptAniJob aniJob, params FrameActionPair[] pairs) { return StartAniJob(aniJob, 1, 0, pairs); }
        public ActiveAni StartAniJob(ScriptAniJob aniJob, float fpsMult = 1, float progress = 0, params FrameActionPair[] pairs)
        {
            return this.BaseInst.StartAniJob(aniJob.BaseAniJob, fpsMult, progress, pairs);
        }
        

        #endregion

        #region Read & Write

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        #endregion

    }
}
