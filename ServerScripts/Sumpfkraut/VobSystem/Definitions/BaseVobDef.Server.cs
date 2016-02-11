using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef
    {
        public string CodeName { get; private set; }

        protected BaseVobDef(string codeName)
        {
            this.CodeName = codeName.Trim().ToUpper();
        }
    }
}
