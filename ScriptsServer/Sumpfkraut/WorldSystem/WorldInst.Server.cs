using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldInst
    {
        public static WorldInst NewWorld;

        public WorldInst(WorldDef def) : this()
        {
            this.definition = def;
        }
    }
}
