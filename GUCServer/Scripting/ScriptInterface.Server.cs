using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripting
{
    public partial interface ScriptInterface
    {
        WorldObjects.VobGuiding.TargetCmd GetTestCmd(WorldObjects.Instances.GUCBaseVobInst target);
    }
}
