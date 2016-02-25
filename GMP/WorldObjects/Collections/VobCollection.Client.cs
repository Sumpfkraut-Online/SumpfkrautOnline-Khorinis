using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public partial class VobCollection
    {
        partial void CheckID(BaseVob vob)
        {
            if (vob.ID < 0 ||vob.ID >= MAX_VOBS)
                throw new ArgumentOutOfRangeException("Vob ID is out of range! 0.." + MAX_VOBS);
        }
    }
}
