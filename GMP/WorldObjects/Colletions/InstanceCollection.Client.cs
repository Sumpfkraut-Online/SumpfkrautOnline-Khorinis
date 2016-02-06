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
        static partial void CheckID(BaseInstance inst)
        {
            if (inst.ID < 0 || inst.ID >= WorldObject.MAX_ID)
                throw new ArgumentOutOfRangeException("Instance ID is out of range!");
        }
    }
}
