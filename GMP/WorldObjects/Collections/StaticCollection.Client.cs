using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    internal partial class StaticCollection<T> where T : GameObject
    {
        partial void CheckID(T vob)
        {
            if (vob.ID < 0 || vob.ID >= GameObject.MAX_ID)
                throw new ArgumentOutOfRangeException("Vob ID is out of range! 0.." + GameObject.MAX_ID);
        }
    }
}
