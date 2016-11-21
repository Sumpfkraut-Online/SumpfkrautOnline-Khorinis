using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Models;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ModelInst : ScriptObject, Model.IScriptModel
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
        public Model BaseInst { get { return this.vob.BaseInst.Model; } }

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
            
        }

        public void RemoveOverlay(ScriptOverlay overlay)
        {

        }

        #endregion

        #region Animations

        public void StartAnimation(AniJob ani, float fpsMult)
        {
            StartAnimation((ScriptAniJob)ani.ScriptObject, fpsMult);
        }

        public void StopAnimation(ActiveAni ani, bool fadeOut)
        {

        }
        
        public ActiveAni StartAnimation(ScriptAniJob aniJob, Action onStop)
        {
            return StartAnimation(aniJob, 1.0f, FrameActionPair.OnEnd(onStop));
        }

        public ActiveAni StartAnimation(ScriptAniJob aniJob, float fpsMult, Action onStop)
        {
            return this.StartAnimation(aniJob, fpsMult, FrameActionPair.OnEnd(onStop));
        }

        public ActiveAni StartAnimation(ScriptAniJob aniJob, params FrameActionPair[] pairs)
        {
            return StartAnimation(aniJob, 1.0f, pairs);
        }

        public ActiveAni StartAnimation(ScriptAniJob aniJob, float fpsMult, params FrameActionPair[] pairs)
        {
            return this.BaseInst.StartAnimation(aniJob.BaseAniJob, fpsMult, pairs);
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
