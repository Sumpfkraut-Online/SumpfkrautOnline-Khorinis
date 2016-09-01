using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects.VobGuiding;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToPosCommand : GuideCmd
    {
        public override byte CmdType { get { return (byte)CommandType.GoToPos; } }

        public GoToPosCommand()
        {
        }

        public GoToPosCommand(Vec3f destination)
        {
            this.destination = destination;
        }

        Vec3f destination;
        public Vec3f Destination { get { return this.destination; } }

        public override void ReadStream(PacketReader stream)
        {
            this.destination = stream.ReadVec3f();
        }

        public override void WriteStream(PacketWriter stream)
        {
            stream.Write(this.destination);
        }
    }
}
