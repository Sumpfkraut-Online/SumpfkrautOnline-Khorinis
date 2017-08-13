using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ModelDef
    {
        #region Collection

        static Dictionary<string, ModelDef> names = new Dictionary<string, ModelDef>(StringComparer.OrdinalIgnoreCase);

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

            return names.TryGetValue(codeName, out model);
        }

        #endregion

        #region Properties

        AniCatalog catalog;
        public AniCatalog Catalog { get { return this.catalog; } }

        public void SetAniCatalog(AniCatalog catalog)
        {
            if (this.catalog != null)
                this.ForEachAniJob(job => catalog.RemoveJob(job));

            this.catalog = catalog;

            if (this.catalog != null)
                this.ForEachAniJob(job => catalog.AddJob(job));
        }

        string codeName;
        public string CodeName
        {
            get { return this.codeName; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("CodeName can't be changed when the object is created!");

                this.codeName = value;
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

        float fistRange = 1;
        public float FistRange
        {
            get { return this.fistRange; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("FistRange can't be changed when the object is created!");
                this.fistRange = value;
            }
        }

        public bool IsNPCModel() { return this.Visual.EndsWith(".MDS"); }

        #endregion

        #region Constructors

        public ModelDef(string codeName) : this(codeName, null)
        {
        }

        public ModelDef(string codeName, string visual) : this()
        {
            this.Visual = visual;
            this.CodeName = codeName;
        }

        #endregion

        #region Create & Delete

        partial void pCreate()
        {
            if (string.IsNullOrWhiteSpace(this.CodeName))
                throw new Exception("CodeName is null or white space!");

            names.Add(this.CodeName, this);
        }

        partial void pDelete()
        {
            names.Remove(this.CodeName);
        }

        #endregion

        #region Animations

        Dictionary<string, ScriptAniJob> aniNames = new Dictionary<string, ScriptAniJob>(StringComparer.OrdinalIgnoreCase);

        // By Names

        public bool ContainsAniJob(string codeName)
        {
            if (codeName == null)
                return false;

            return aniNames.ContainsKey(codeName);
        }

        public bool TryGetAniJob(string codeName, out ScriptAniJob job)
        {
            if (codeName == null)
            {
                job = null;
                return false;
            }

            return aniNames.TryGetValue(codeName, out job);
        }

        #region Add & Remove

        partial void pAddAniJob(ScriptAniJob aniJob)
        {
            if (string.IsNullOrWhiteSpace(aniJob.CodeName))
                throw new ArgumentException("CodeName of ScriptAniJob is null or white space!");

            aniNames.Add(aniJob.CodeName, aniJob);

            if (catalog != null)
                catalog.AddJob(aniJob);
        }

        partial void pRemoveAniJob(ScriptAniJob aniJob)
        {
            if (aniNames.Remove(aniJob.CodeName))
            {
                if (catalog != null)
                    catalog.RemoveJob(aniJob);
            }
        }

        #endregion

        public void ForEachAniJob(Action<ScriptAniJob> action)
        {
            this.baseDef.ForEachAniJob(job => action((ScriptAniJob)job.ScriptObject));
        }

        #endregion

        #region Overlays

        Dictionary<string, ScriptOverlay> ovNames = new Dictionary<string, ScriptOverlay>(StringComparer.OrdinalIgnoreCase);

        public bool ContainsOverlay(string codeName)
        {
            if (codeName == null)
                return false;

            return ovNames.ContainsKey(codeName);
        }

        public bool TryGetOverlay(string codeName, out ScriptOverlay ov)
        {
            if (codeName == null)
            {
                ov = null;
                return false;
            }

            return ovNames.TryGetValue(codeName, out ov);
        }

        public void ForEachOverlay(Action<ScriptOverlay> action)
        {
            this.baseDef.ForEachOverlay(o => action((ScriptOverlay)o.ScriptObject));
        }

        public void ForEachOverlayPredicate(Predicate<ScriptOverlay> predicate)
        {
            this.baseDef.ForEachOverlayPredicate(o => { return predicate((ScriptOverlay)o.ScriptObject); });
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
