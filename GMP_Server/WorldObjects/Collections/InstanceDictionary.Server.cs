using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects.Collections
{
    public partial class InstanceDictionary : VobObjDictionary<ushort, VobInstance>
    {
        Dictionary<string, VobInstance> names = new Dictionary<string, VobInstance>();

        internal override void Add(VobInstance vob)
        {
            base.Add(vob);
            names.Add(vob.InstanceName, vob);
        }

        internal override void Remove(VobInstance vob)
        {
            base.Remove(vob);
            names.Remove(vob.InstanceName);
        }

        public VobInstance Get(string name)
        {
            VobInstance inst;
            names.TryGetValue(name.ToUpper(), out inst);
            return inst;
        }
    }
}
