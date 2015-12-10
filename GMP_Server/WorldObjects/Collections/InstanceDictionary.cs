using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects.Collections
{
    public class InstanceDictionary : VobObjDictionary<ushort, VobInstance>
    {
        Dictionary<string, VobInstance> names = new Dictionary<string, VobInstance>();

        internal InstanceDictionary()
        {
        }

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
