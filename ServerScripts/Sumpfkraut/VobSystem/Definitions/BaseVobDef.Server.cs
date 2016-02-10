using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef
    {
        public string CodeName { get; private set; }

        protected BaseVobDef(string codeName)
        {
            this.CodeName = codeName.Trim().ToUpper();
        }

        public void Create()
        {
            InstanceCollection.Add(this.baseDef);
        }
    }
}
