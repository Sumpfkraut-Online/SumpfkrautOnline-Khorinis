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
        Effect_Name,

        // often synchronized attributes
        Vob_CodeName,
        Vob_Name,

        // event driven

        // crafting
    }

}
