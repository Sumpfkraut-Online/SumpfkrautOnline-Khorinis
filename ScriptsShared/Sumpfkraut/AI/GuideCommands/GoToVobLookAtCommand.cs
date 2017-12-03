using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToVobLookAtCommand : GoToVobCommand
    {
        public override byte CmdType { get { return (byte)CommandType.GoToVobLookAt; } }

        public GoToVobLookAtCommand()
        {
        }

        public GoToVobLookAtCommand(BaseVobInst target, float distance = 500) : base(target, distance)
        {
        }
    }
}
