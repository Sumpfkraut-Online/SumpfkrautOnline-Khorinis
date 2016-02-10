using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects
{
    public abstract partial class BaseVob
    {
        zCVob gvob;
        public zCVob gVob { get {return gvob; } }
    }
}
