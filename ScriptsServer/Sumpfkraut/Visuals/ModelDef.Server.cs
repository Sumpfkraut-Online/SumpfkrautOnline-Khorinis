using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ModelDef
    {
        #region Collection

        static Dictionary<string, ModelDef> names = new Dictionary<string, ModelDef>();

        // By Names

        public static bool Contains(string codeName)
        {
            if (codeName == null)
                throw new ArgumentNullException("CodeName is null!");

            return names.ContainsKey(codeName);
        }

        public static bool TryGetModel(string codeName, out ModelDef model)
        {
            if (codeName == null)
                throw new ArgumentNullException("CodeName is null!");

            return names.TryGetValue(codeName.ToUpper(), out model);
        }

        #endregion

        #region Properties

        string codeName;
        public string CodeName
        {
            get { return this.codeName; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("CodeName can't be changed when the object is created!");

                this.codeName = value == null ? "" : value.ToUpper();
            }
        }

        float radius = 1;
        public float Radius
        {
            get { return this.radius; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("Radius can't be changed when the object is created!");
                this.radius = value;
            }
        }

        float height = 1;
        public float Height
        {
            get { return this.height; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("Height can't be changed when the object is created!");
                this.height = value;
            }
        }

        public static ModelDef LargestNPC = null; // for fight system
        public bool IsNPCModel() { return this.Visual.EndsWith(".MDS"); }

        #endregion

        #region Constructors

        public ModelDef(string codeName) : this()
        {
            this.CodeName = codeName;
        }

        public ModelDef(string codeName, string visual) : this(codeName)
        {
            this.Visual = visual;
        }

        #endregion

        #region Create & Delete

        partial void pCreate()
        {
            if (string.IsNullOrWhiteSpace(this.CodeName))
                throw new Exception("CodeName is null or white space!");

            names.Add(this.CodeName, this);

            if (this.IsNPCModel() && (LargestNPC == null || this.radius > LargestNPC.radius))
                LargestNPC = this;
        }

        partial void pDelete()
        {
            names.Remove(this.CodeName);

            //improve ?
            if (this == LargestNPC)
            {
                ModelDef newLargestNPC = null;
                Models.Model.ForEach(m =>
                {
                    ModelDef model = (ModelDef)m.ScriptObject;
                    if (model != this && model.IsNPCModel() && model.radius > this.radius)
                        newLargestNPC = model;
                });
                LargestNPC = newLargestNPC;
            }
        }

        #endregion

        #region Animations

        Dictionary<string, ScriptAniJob> aniNames = new Dictionary<string, ScriptAniJob>();

        // By Names

        public bool ContainsAniJob(string codeName)
        {
            if (codeName == null)
                return false;

            return aniNames.ContainsKey(codeName.ToUpper());
        }

        public bool TryGetAniJob(string codeName, out ScriptAniJob job)
        {
            if (codeName == null)
            {
                job = null;
                return false;
            }

            return aniNames.TryGetValue(codeName.ToUpper(), out job);
        }

        partial void pAddAniJob(ScriptAniJob aniJob)
        {
            if (string.IsNullOrWhiteSpace(aniJob.CodeName))
                throw new ArgumentException("CodeName of ScriptAniJob is null or white space!");

            aniNames.Add(aniJob.CodeName, aniJob);
        }

        partial void pRemoveAniJob(ScriptAniJob aniJob)
        {
            aniNames.Remove(aniJob.CodeName);
        }

        #endregion

        #region Overlays

        Dictionary<string, ScriptOverlay> ovNames = new Dictionary<string, ScriptOverlay>();

        public bool ContainsOverlay(string codeName)
        {
            if (codeName == null)
                return false;

            return ovNames.ContainsKey(codeName.ToUpper());
        }

        public bool TryGetOverlay(string codeName, out ScriptOverlay ov)
        {
            if (codeName == null)
            {
                ov = null;
                return false;
            }

            return ovNames.TryGetValue(codeName.ToUpper(), out ov);
        }

        partial void pAddOverlay(ScriptOverlay overlay)
        {
            if (string.IsNullOrWhiteSpace(overlay.CodeName))
                throw new ArgumentException("CodeName of ScriptOverlay is null or white space!");

            ovNames.Add(overlay.CodeName, overlay);
        }

        partial void pRemoveOverlay(ScriptOverlay overlay)
        {
            ovNames.Remove(overlay.CodeName);
        }

        #endregion
    }
}
