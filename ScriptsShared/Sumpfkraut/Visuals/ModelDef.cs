using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Models;
using GUC.Animations;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ModelDef : ExtendedObject, ModelInstance.IScriptModelInstance
    {
        #region Properties

        ModelInstance baseDef;
        public ModelInstance BaseDef { get { return baseDef; } }

        public int ID { get { return baseDef.ID; } }
        public bool IsStatic { get { return baseDef.IsStatic; } }
        public bool IsCreated { get { return this.baseDef.IsCreated; } }

        public string Visual { get { return baseDef.Visual; } set { baseDef.Visual = value; } }

        #endregion

        #region Collections

        // By IDs

        public static bool Contains(int id)
        {
            return ModelInstance.Contains(id);
        }

        public static bool TryGetModel(int id, out ModelDef model)
        {
            ModelInstance m;
            if (ModelInstance.TryGet(id, out m))
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
            ModelInstance.ForEach(m => action((ModelDef)m.ScriptObject));
        }

        public static void ForEachModelPredicate(Predicate<ModelDef> predicate)
        {
            ModelInstance.ForEachPredicate(m => predicate((ModelDef)m.ScriptObject));
        }

        public int GetCount()
        {
            return ModelInstance.Count;
        }

        #endregion

        #region Constructors

        public ModelDef()
        {
            this.baseDef = new ModelInstance(this);
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

        #region Overlays

        public void AddOverlay(Overlay overlay)
        {
            AddOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        public void RemoveOverlay(Overlay overlay)
        {
            RemoveOverlay((ScriptOverlay)overlay.ScriptObject);
        }

        partial void pAddOverlay(ScriptOverlay overlay);
        public void AddOverlay(ScriptOverlay overlay)
        {
            this.baseDef.AddOverlay(overlay.BaseOverlay);
            pAddOverlay(overlay);
        }

        partial void pRemoveOverlay(ScriptOverlay overlay);
        public void RemoveOverlay(ScriptOverlay overlay)
        {
            this.baseDef.RemoveOverlay(overlay.BaseOverlay);
            pRemoveOverlay(overlay);
        }

        #endregion
        
        #region Animations

        public void AddAniJob(AniJob aniJob)
        {
            AddAniJob((ScriptAniJob)aniJob.ScriptObject);
        }

        public void RemoveAniJob(AniJob aniJob)
        {
            RemoveAniJob((ScriptAniJob)aniJob.ScriptObject);
        }

        partial void pAddAniJob(ScriptAniJob aniJob);
        public void AddAniJob(ScriptAniJob aniJob)
        {
            this.baseDef.AddAniJob(aniJob.BaseAniJob);
            pAddAniJob(aniJob);
        }

        partial void pRemoveAniJob(ScriptAniJob aniJob);
        public void RemoveAniJob(ScriptAniJob aniJob)
        {
            this.baseDef.RemoveAniJob(aniJob.BaseAniJob);
            pRemoveAniJob(aniJob);
        }

        #endregion
    }
}
