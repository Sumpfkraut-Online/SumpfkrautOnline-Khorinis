using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public partial class VobCollection : VobObjCollection<uint, Vob>
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
        }

        internal override void Remove(Vob vob)
        {
            base.Remove(vob);
        }
    }
}
