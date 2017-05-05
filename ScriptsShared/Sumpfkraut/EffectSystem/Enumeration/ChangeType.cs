using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration
{

    public enum ChangeType : int
    {
        Undefined,                          // do not use it normally (might be useful in checking, though)
        
        // changes influencing their own Effect-container
        Effect_GlobalID_Set,                // name / id of an effect for global access
        Effect_Name_Set,                    // set the name of an effect
        Effect_Parent_Add,                  // add parent effect to inherit Changes from  (can only add global effects)

        // often synchronized attributes
        Vob_CDDyn_Set,                      // cdDyn of Gothic-engine (collision detection with dynamical environment)
        Vob_CDStatic_Set,                   // cdStatic of Gothic-engine (collision detection with static world mesh)
        Vob_CodeName_Set,                   // codename of the VobDef with no whitespace and underscore as seperator
        Vob_VobType_Set,                    // type of vob (replacing VobDefType and VobInstType?)

        NamedVob_Name_Set,                  //named of a vob (typically is the display name when used in child classes)

        NPC_BodyMesh_Set,                   // body mesh variation (only for NPCs)
        NPC_BodyTex_Set,                    // body texture variation (only for NPCs)
        NPC_HeadMesh_Set,                   // head mesh variation (only for NPCs)
        NPC_HeadTex_Set,                    // head texture varation (only for NPCs)

        // event driven
        World_Clock_Time_Set,               // set the clock time to specific value
        World_Clock_Rate_Set,               // set the rate at which the clock time changes
        World_Clock_IsRunning_Set,          // start, resume or stop the clock / time of a world

        // crafting
    }

}
