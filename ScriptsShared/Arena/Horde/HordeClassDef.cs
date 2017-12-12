using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Arena
{
    partial class HordeClassDef
    {
        public string Name;

        public List<string> Equipment;
        public bool NeedsArrows, NeedsBolts;

        public int ExtraProtection;
        public int ExtraDamage;
    }
}
