using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects.VobGuiding;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToVobCommand : TargetCmd
    {
        public override byte CmdType { get { return (byte)CommandType.GoToVob; } }

        new public BaseVobInst Target { get { return (BaseVobInst)base.Target?.ScriptObject; } }

        public GoToVobCommand() : base()
        {
        }

        public GoToVobCommand(BaseVobInst target, float distance = 500f) : base(target.BaseInst, distance)
        {
        }
    }
}
