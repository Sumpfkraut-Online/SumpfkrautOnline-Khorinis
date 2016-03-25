using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef
    {
        static Dictionary<string, BaseVobDef> nameDict = new Dictionary<string, BaseVobDef>();
        public static T Get<T>(string codeName) where T : BaseVobDef
        {
            BaseVobDef ret;
            nameDict.TryGetValue(codeName.ToUpper(), out ret);
            return (T)ret;
        }

        string codeName;
        public string CodeName { get { return codeName; } }

        protected BaseVobDef(BaseVobInstance baseDef, string codeName) : this(baseDef)
        {
            if (string.IsNullOrWhiteSpace(codeName))
                throw new ArgumentException("CodeName is null or white space!");

            this.codeName = codeName.Trim().ToUpper();
        }

        partial void pCreate()
        {
            nameDict.Add(this.CodeName, this);
        }

        partial void pDelete()
        {
            nameDict.Remove(this.CodeName);
        }
    }
}
