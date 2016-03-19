using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseVobInstance : GameObject
    {
        public abstract zCVob CreateVob(zCVob vob = null);
    }
}
