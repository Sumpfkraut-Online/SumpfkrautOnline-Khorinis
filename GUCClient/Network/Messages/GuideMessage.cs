using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Network.Messages
{
    static class GuideMessage
    {
        public static void ReadGuideAddMessage(PacketReader stream)
        {
            GameClient.Client.guidedIDs.Add(stream.ReadUShort());
        }
    }
}
