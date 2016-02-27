using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Server.Network;

namespace GUC.WorldObjects
{
    public partial class BaseVob
    {
        internal NetCell Cell = null;

        public void SetPosition(Vec3f pos)
        {
            throw new NotImplementedException();
        }

        public Vec3f GetPosition()
        {
            return this.pos;
        }

        public void SetDirection(Vec3f dir)
        {
            Vec3f d = dir.IsNull() ? new Vec3f(0, 0, 1) : dir;
            throw new NotImplementedException();
        }

        public Vec3f GetDirection()
        {
            return this.dir;
        }
    }
}
