using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.VobGuiding
{
    public abstract class GuideCmd
    {
        public abstract byte CmdType { get; }

        public virtual void WriteStream(PacketWriter stream)
        {
        }

        public virtual void ReadStream(PacketReader stream)
        {
        }
    }

    public abstract class TargetCmd : GuideCmd
    {
        BaseVob target;
        public BaseVob Target { get { return this.target; } }

        public TargetCmd(BaseVob target)
        {
            this.target = target;
        }
    }
}
