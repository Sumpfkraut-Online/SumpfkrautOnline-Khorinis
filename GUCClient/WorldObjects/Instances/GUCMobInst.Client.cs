using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects
{
    public partial class Mob : Vob
    {
        new public oCMob gVob { get { return (oCMob)base.gVob; } }
    }
}
