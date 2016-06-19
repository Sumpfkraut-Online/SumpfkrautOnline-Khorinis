using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using Gothic.Objects.Meshes;

namespace GUC.WorldObjects.Instances
{
    public partial class ProjectileInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            zCVob ret = vob == null ? zCVob.Create() : vob;
            zCVisual vis = zCVisual.LoadVisual(this.Model.Visual);
            ret.SetVisual(vis);
            return ret;
        }
    }
}
