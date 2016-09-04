using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripting
{
    public partial interface ScriptInterface
    {
        WorldObjects.VobGuiding.TargetCmd GetTestCmd(WorldObjects.BaseVob target);
    }
}
