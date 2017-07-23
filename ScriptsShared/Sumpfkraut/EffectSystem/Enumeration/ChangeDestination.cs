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
        Effect_GlobalID,
        Effect_Name,
        Effect_Parent,

        // often synchronized attributes
        Vob_CDDyn,
        Vob_CDStatic,
        Vob_CodeName,
        Vob_VobType,

        NamedVob_Name,

        Item_Material,

        NPC_BodyMesh,
        NPC_BodyText,
        NPC_HeadMesh,
        NPC_HeadText,

        // over-time effects
        NPC_TestPoison,

        // event driven
        World_Clock_IsRunning,
        World_Clock_Rate,
        World_Clock_Time,

        // crafting
    }

}
