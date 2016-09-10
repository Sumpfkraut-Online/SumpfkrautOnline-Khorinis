using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef
    {
        #region Constructors

        public BaseVobDef(string codeName) : this()
        {
            this.CodeName = codeName;
        }

        #endregion

        static Dictionary<string, BaseVobDef> nameDict = new Dictionary<string, BaseVobDef>(StringComparer.OrdinalIgnoreCase);
        public static T Get<T>(string codeName) where T : BaseVobDef
        {
            BaseVobDef ret;
            if (nameDict.TryGetValue(codeName, out ret) && ret is T)
            {
                return (T)ret;
            }
            return null;
        }

        string codeName;
        public string CodeName
        {
            get { return codeName; }
            set
            {
                if (this.IsCreated)
                    throw new NotSupportedException("Can't change CodeName when the Definition is already added to the static collection!");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CodeName is null or white space!");
                this.codeName = value;
            }
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
