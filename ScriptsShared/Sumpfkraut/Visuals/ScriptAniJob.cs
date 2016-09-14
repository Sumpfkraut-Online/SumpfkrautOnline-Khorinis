using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Visuals
{  
    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        #region Properties

        public bool IsCreated { get { return this.baseAniJob.IsCreated; } }

        AniJob baseAniJob;
        public AniJob BaseAniJob { get { return this.baseAniJob; } }

        public string AniName { get { return this.baseAniJob.Name; } set { this.baseAniJob.Name = value; } }

        public ScriptAni DefaultAni { get { return (ScriptAni)this.baseAniJob.DefaultAni?.ScriptObject; } }

        public ModelDef ModelDef { get { return (ModelDef)this.baseAniJob.ModelInstance?.ScriptObject; } }

        public int ID { get { return this.baseAniJob.ID; } set { this.baseAniJob.ID = value; } }

        public int Layer { get { return this.baseAniJob.Layer; } set { this.baseAniJob.Layer = value; } }

        #endregion

        public void SetDefaultAni(Animation ani)
        {
            this.SetDefaultAni((ScriptAni)ani.ScriptObject);
        }

        public virtual void SetDefaultAni(ScriptAni ani)
        {
            this.baseAniJob.SetDefaultAni(ani.BaseAni);
        }

        public void AddOverlayAni(Animation ani, Overlay overlay)
        {
            this.AddOverlayAni((ScriptAni)ani.ScriptObject, (ScriptOverlay)overlay.ScriptObject);
        }

        public virtual void AddOverlayAni(ScriptAni ani, ScriptOverlay ov)
        {
            this.baseAniJob.AddOverlayAni(ani.BaseAni, ov.BaseOverlay);
        }

        public void RemoveOverlayAni(Animation ani)
        {
            this.RemoveOverlayAni((ScriptAni)ani.ScriptObject);
        }

        public void RemoveOverlayAni(ScriptAni ani)
        {
            this.baseAniJob.RemoveOverlayAni(ani.BaseAni);
        }

        #region Constructors

        public ScriptAniJob()
        {
            this.baseAniJob = new AniJob(this);
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
