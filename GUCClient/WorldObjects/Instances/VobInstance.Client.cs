using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using Gothic.Objects.Meshes;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            zCVob ret = vob == null ? zCVob.Create() : vob;
            zCVisual vis = zCVisual.LoadVisual(this.ModelInstance.Visual);
            ret.SetVisual(vis);

            ret.SetCollDetDyn(CDDyn);
            ret.SetCollDetStat(CDStatic);
            
            return ret;
        }
    }
}
