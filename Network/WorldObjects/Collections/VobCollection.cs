using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.WorldObjects.Collections
{
    public partial class VobCollection : VobObjCollection<uint, Vob>
    {
        internal VobCollection()
        {
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
            {
                vobDicts[i] = new VobDictionary();
            }
        }

        new public VobDictionary GetDict(VobTypes type)
        {
            return (VobDictionary)base.GetDict(type);
        }
    }
}
