using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class NPCDef
    {
        public static NPCDef Get(string codeName)
        {
            return Get<NPCDef>(codeName);
        }

        public NPCDef(string codeName) : this()
        {
            this.CodeName = codeName;
        }
    }
}
