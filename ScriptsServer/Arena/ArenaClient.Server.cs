using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        partial void pOnConnect()
        {
            this.SetToSpectator(WorldInst.Current, new Vec3f(), new Vec3f());
        }
    }
}
