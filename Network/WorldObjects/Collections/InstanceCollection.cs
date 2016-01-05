using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects.Collections
{
    public partial class InstanceCollection : VobObjCollection<ushort, VobInstance>
    {
        internal InstanceCollection()
        {
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
            {
                vobDicts[i] = new InstanceDictionary();
            }
        }

        new public InstanceDictionary GetDict(VobTypes type)
        {
            return (InstanceDictionary)base.GetDict(type);
        }
    }
}
