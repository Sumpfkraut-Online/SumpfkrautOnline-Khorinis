using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Models;

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

        public int Radius = 1;

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

        public ModelDef(PacketReader stream) : this()
        {
            this.baseDef.ReadStream(stream);
        }

        private ModelDef()
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

        partial void pAddAniJob(ScriptAniJob job);
        public void AddAniJob(ScriptAniJob job)
        {
            if (job == null)
                throw new ArgumentNullException("ScriptAniJob is null!");

            this.baseDef.AddAniJob(job.BaseAniJob);
            pAddAniJob(job);
        }

        partial void pRemoveAniJob(ScriptAniJob job);
        public void RemoveAniJob(ScriptAniJob job)
        {
            if (job == null)
                throw new ArgumentNullException("ScriptAniJob is null!");

            this.baseDef.RemoveAniJob(job.BaseAniJob);
            pRemoveAniJob(job);
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
    }
}
