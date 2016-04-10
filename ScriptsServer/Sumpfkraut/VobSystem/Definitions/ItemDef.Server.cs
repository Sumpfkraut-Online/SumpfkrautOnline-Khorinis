using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class ItemDef
    {
        public float Range = 0;
        public int Damage = 0;
        public int Protection = 0;

        public ItemDef(string codeName) : base(new ItemInstance(), codeName)
        {
        }
    }
}
