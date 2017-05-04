using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration
{

    public enum ChangeDestination : int
    {
        Undefined,
        
        // changes influencing their own effect-container
        //Effect_Child,
        Effect_GlobalID,
        Effect_Name,
        Effect_Parent,
        Effect_PermanentFlag,

        // often synchronized attributes
        Vob_CDDyn,
        Vob_CDStatic,
        Vob_CodeName,
        Vob_Name,
        Vob_VobType,

        // event driven

        // crafting
    }

}
