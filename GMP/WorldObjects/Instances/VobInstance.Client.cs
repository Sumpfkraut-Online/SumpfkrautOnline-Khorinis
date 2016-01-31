using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using Gothic.Objects;
using Gothic.Objects.Meshes;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance : WorldObject, IVobObj<ushort>
    {
        public virtual zCVob CreateVob(zCVob vob = null)
        {
            zCVob ret = vob == null ? zCVob.Create() : vob;
            zCVisual vis = zCVisual.LoadVisual(this.Visual);
            ret.SetVisual(vis);
            if (CDDyn) ret.BitField1 |= (int)zCVob.BitFlag0.collDetectionDynamic;
            else ret.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionDynamic;

            if (CDStatic) ret.BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
            else ret.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionStatic;
            return ret;
        }
    }
}
