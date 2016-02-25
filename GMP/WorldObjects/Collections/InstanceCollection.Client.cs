using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects.Collections
{
    public static partial class InstanceCollection
    {
        static partial void CheckID(BaseVobInstance inst)
        {
            if (inst.ID < 0 || inst.ID >= MAX_INSTANCES)
                throw new ArgumentOutOfRangeException("Instance ID is out of range! 0.." + MAX_INSTANCES);
        }
    }
}
