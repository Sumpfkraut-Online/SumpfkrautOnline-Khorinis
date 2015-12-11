using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Client.WorldObjects.Collections
{
    public class VobCollection : VobObjCollection<uint, Vob>
    {
        Dictionary<int, Vob> vobAddr = new Dictionary<int, Vob>();
        public Vob GetByAddress(int address)
        {
            Vob vob;
            vobAddr.TryGetValue(address, out vob);
            return vob;
        }

        internal override void Add(Vob vob)
        {
            base.Add(vob);
            vobAddr.Add(vob.gVob.Address, vob);
        }

        internal override void Remove(Vob vob)
        {
            base.Remove(vob);
            vobAddr.Remove(vob.gVob.Address);
        }

        internal VobCollection()
        {
            for (int i = 0; i < (int)VobType.Maximum; i++)
            {
                vobDicts[i] = new VobDictionary();
            }
        }

        new public VobDictionary GetDict(VobType type)
        {
            return (VobDictionary)base.GetDict(type);
        }
    }
}
