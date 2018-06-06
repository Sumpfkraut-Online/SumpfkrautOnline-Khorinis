using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Arena.GameModes
{
    enum TeamIdent : sbyte
    {
        None = -4,

        FFAPlayer = -3,
        FFASpectator = -2,

        GMSpectator = -1,
        /// <summary> and above </summary>
        GMPlayer = 0,
    }
}
