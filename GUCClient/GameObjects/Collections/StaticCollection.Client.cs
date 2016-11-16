using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.GameObjects.Collections
{
    internal partial class StaticCollection<T> where T : IDObject
    {
        partial void pCheckID(T vob)
        {
            if (vob.ID < 0 || vob.ID >= capacity)
                throw new ArgumentOutOfRangeException("Vob ID is out of range! 0.." + (capacity-1));
        }
    }
}
