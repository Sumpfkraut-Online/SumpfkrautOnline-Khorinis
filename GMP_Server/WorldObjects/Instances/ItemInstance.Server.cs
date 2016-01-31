using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class ItemInstance : VobInstance
    {
        public ItemInstance(string instanceName, IScriptItemInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public ItemInstance(ushort ID, string instanceName, IScriptItemInstance scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }
    }
}