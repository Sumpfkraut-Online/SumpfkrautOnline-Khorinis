using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class VobDef
    {
        public VobDef(string codeName, int id = -1) : base(codeName)
        {
            SetBaseDef(new VobInstance(this, id));
        }

        protected VobDef(string codeName) : base(codeName)
        {
        }
    }
}
