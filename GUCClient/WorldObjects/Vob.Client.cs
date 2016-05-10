using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using Gothic.Objects;
using GUC.Types;
using Gothic.Types;

namespace GUC.WorldObjects
{
    public partial class Vob
    {
        public void SetPhysics(bool enabled)
        {
            this.gVob.SetPhysicsEnabled(enabled);
        }

        public void SetVelocity(Vec3f vel)
        {
            var rb = WinApi.Process.ReadInt(gvob.Address + 224);
            using (zVec3 vec = zVec3.Create(vel.X, vel.Y, vel.Z))
                WinApi.Process.THISCALL<WinApi.NullReturnCall>(rb, 0x5B66D0, vec);
        }
    }
}
