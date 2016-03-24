using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef
    {
        static Dictionary<string, BaseVobDef> codeNameDict = new Dictionary<string, BaseVobDef>();
        public static T Get<T> (string codeName) where T : BaseVobDef
        {
            BaseVobDef baseVobDef;
            codeNameDict.TryGetValue(codeName.ToUpper(), out baseVobDef);
            return (T) baseVobDef;
        }

        private String codeName;
        public String CodeName { get { return codeName; } }

        protected BaseVobDef (BaseVobInstance baseDef, string codeName) : this(baseDef)
        {
            if (string.IsNullOrWhiteSpace(codeName))
            {
                throw new ArgumentException(this.getObjName()  + ": Invalid null or white space" 
                    + "provided in constructor for parameter codeName!");
            }

            this.codeName = codeName.Trim().ToUpper();
        }

        partial void pCreate ()
        {
            codeNameDict.Add(CodeName, this);
        }

        partial void pDelete ()
        {
            codeNameDict.Remove(CodeName);
        }
    }
}
