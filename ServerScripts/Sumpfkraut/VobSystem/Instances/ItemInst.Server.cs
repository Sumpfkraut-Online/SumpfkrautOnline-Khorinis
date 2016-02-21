using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ItemInst
    {
        public ItemInst(ItemDef def, int id = -1) : base(def, true)
        {
            SetBaseInst(new WorldObjects.Item(this, def.baseDef, id));
        }
    }
}
