using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Models;
using GUC.Animations;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ModelDef : ScriptObject, Model.IScriptModel
    {
        #region Properties

        Model baseDef;
        public Model BaseDef { get { return baseDef; } }

        public int ID { get { return baseDef.ID; } }
        public bool IsStatic { get { return baseDef.IsStatic; } }
        public bool IsCreated { get { return this.baseDef.IsCreated; } }

        public string Visual { get { return baseDef.Visual; } set { baseDef.Visual = value; } }

        #endregion

        #region Collections

        // By IDs

        public static bool Contains(int id)
        {
            return Models.Model.Contains(id);
        }

        public static bool TryGetModel(int id, out ModelDef model)
        {
            Models.Model m;
            if (Models.Model.TryGet(id, out m))
            {
                model = (ModelDef)m.ScriptObject;
                return true;
            }
            model = null;
            return false;
        }

        // Loops

        public static void ForEachModel(Action<ModelDef> action)
        {
            Models.Model.ForEach(m => action((ModelDef)m.ScriptObject));
        }

        public static void ForEachModel(Predicate<ModelDef> predicate)
        {
            Models.Model.ForEach(m => predicate((ModelDef)m.ScriptObject));
        }

        public int GetCount()
        {
            return Models.Model.GetCount();
        }

        #endregion

        #region Constructors

        public ModelDef()
        {
            this.baseDef = new Model();
            this.baseDef.ScriptObject = this;
        }

        #endregion

        #region Create & Delete

        partial void pCreate();
        public void Create()
        {
            this.baseDef.Create();
            pCreate();
        }

        partial void pDelete();
        public void Delete()
        {
            this.baseDef.Delete();
            pDelete();
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

        #region Animations

        public void AddAniJob(AniJob aniJob)
        {
            AddAniJob((ScriptAniJob)aniJob.ScriptObject);
        }

        partial void pAddAniJob(ScriptAniJob aniJob);
        public void AddAniJob(ScriptAniJob aniJob)
        {
            if (aniJob == null)
                throw new ArgumentNullException("ScriptAniJob is null!");

            this.baseDef.AddAniJob(aniJob.BaseAniJob);
            pAddAniJob(aniJob);
        }

        public void RemoveAniJob(AniJob aniJob)
        {
            RemoveAniJob((ScriptAniJob)aniJob.ScriptObject);
        }

        partial void pRemoveAniJob(ScriptAniJob aniJob);
        public void RemoveAniJob(ScriptAniJob aniJob)
        {
            if (aniJob == null)
                throw new ArgumentNullException("ScriptAniJob is null!");

            this.baseDef.RemoveAniJob(aniJob.BaseAniJob);
            pRemoveAniJob(aniJob);
        }

        public bool ContainsAniJob(int id)
        {
            return this.baseDef.ContainsAni(id);
        }

        public bool TryGetAniJob(int id, out ScriptAniJob job)
        {
            Animations.AniJob a;
            if (this.baseDef.TryGetAni(id, out a))
            {
                job = (ScriptAniJob)a.ScriptObject;
                return true;
            }
            job = null;
            return false;
        }

        public void ForEachAniJob(Action<ScriptAniJob> action)
        {
            this.baseDef.ForEachAni(j => action((ScriptAniJob)j.ScriptObject));
        }

        public void ForEachAniJob(Predicate<ScriptAniJob> predicate)
        {
            this.baseDef.ForEachAni(j => predicate((ScriptAniJob)j.ScriptObject));
        }

        public int GetAniJobCount()
        {
            return this.baseDef.GetAniCount();
        }

        #endregion

        #region Overlays
        
        public void AddOverlay(Overlay overlay)
        {
            AddOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        partial void pAddOverlay(ScriptOverlay overlay);
        public void AddOverlay(ScriptOverlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("ScriptOverlay is null!");

            this.baseDef.AddOverlay(overlay.BaseOverlay);
            pAddOverlay(overlay);
        }

        public void RemoveOverlay(Overlay overlay)
        {
            RemoveOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        partial void pRemoveOverlay(ScriptOverlay overlay);
        public void RemoveOverlay(ScriptOverlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("ScriptOverlay is null!");

            this.baseDef.RemoveOverlay(overlay.BaseOverlay);
            pRemoveOverlay(overlay);
        }

        public bool ContainsOverlay(int id)
        {
            return this.baseDef.ContainsAni(id);
        }

        public bool TryGetOverlay(int id, out ScriptOverlay overlay)
        {
            Animations.Overlay a;
            if (this.baseDef.TryGetOverlay(id, out a))
            {
                overlay = (ScriptOverlay)a.ScriptObject;
                return true;
            }
            overlay = null;
            return false;
        }

        public void ForEachOverlay(Action<ScriptOverlay> action)
        {
            this.baseDef.ForEachOverlay(o => action((ScriptOverlay)o.ScriptObject));
        }

        public void ForEachOverlay(Predicate<ScriptOverlay> predicate)
        {
            this.baseDef.ForEachOverlay(o => predicate((ScriptOverlay)o.ScriptObject));
        }

        public int GetOverlayCount()
        {
            return this.baseDef.GetOverlayCount();
        }

        #endregion
    }
}
