using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Arena.GameModes
{
    enum TeamIdent
    {
        None = -4,
        FFAPlayer = -3,
        FFASpectator = -2,
        GMSpectator = -1,
        GMPlayer = 0, // and above
    }
}
