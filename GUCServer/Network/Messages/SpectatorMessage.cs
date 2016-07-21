using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Network.Messages
{
    static class SpectatorMessage
    {
        public static void ReadPos(PacketReader stream, GameClient client)
        {
            client.SetPosition(stream.ReadCompressedPosition(), false);
        }
    }
}
