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
                    throw new Exception("CodeName can't be changed while the object is created!");

                this.codeName = value == null ? "" : value.ToUpper();
            }
        }

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
        }

        partial void pDelete()
        {
            names.Remove(this.CodeName);
        }

        #endregion

        #region Animations

        Dictionary<string, ScriptAniJob> aniNames = new Dictionary<string, ScriptAniJob>();

        // By Names

        public bool ContainsAniJob(string codeName)
        {
            if (codeName == null)
                throw new ArgumentNullException("CodeName is null!");

            return aniNames.ContainsKey(codeName);
        }

        public bool TryGetAniJob(string codeName, out ScriptAniJob job)
        {
            if (codeName == null)
                throw new ArgumentNullException("CodeName is null!");

            return aniNames.TryGetValue(codeName, out job);
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
    }
}
