using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration
{

    public enum ChangeType : int
    {
        Undefined,                          // do not use it normally (might be useful in checking, though)
        
        // changes influencing their own effect-container
        //Effect_Child_Add,                   // add child effect to pass changes to (can only add global effects)
        Effect_GlobalID_Set,                // name / id of an effect for global access
        Effect_Name_Set,                    // set the name of an effect
        Effect_Parent_Add,                  // add parent effect to inherit Changes from  (can only add global effects)
        Effect_PermanentFlag_Set,           // flagged Effects are added to the permanent Effect at index 0 in EffectHandler-objects

        // often synchronized attributes
        Vob_CodeName_Set,                   // codename of the VobDef with no whitespace and underscore as seperator
        Vob_Name_Set,                       // displayed name of a vob
        Vob_VobDefType_Set,                 // type of vob-definition (see VobSystem.Enumeration.VobDefType)
        Vob_VobInstType_Set,                // type of vob-instance (see VobSystem.Enumeration.VobInstType)

        // event driven
        World_Clock_Time_Set,               // set the clock time to specific value
        World_Clock_Rate_Set,               // set the rate at which the clock time changes
        World_Clock_IsRunning_Set,          // start, resume or stop the clock / time of a world

        // crafting
    }

}
