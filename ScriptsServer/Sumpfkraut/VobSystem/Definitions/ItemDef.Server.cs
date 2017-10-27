using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class ItemDef
    {
        public ItemDef(string codeName) : this()
        {
            this.CodeName = codeName;
        }

        public static ItemDef Get(string codeName)
        {
            return Get<ItemDef>(codeName);
        }
    }
}
