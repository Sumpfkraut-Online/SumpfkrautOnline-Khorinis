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

        public static void ReadGuideAddCmdMessage(PacketReader stream)
        {
        }

        public static void ReadGuideRemoveMessage(PacketReader stream)
        {
            GameClient.Client.guidedIDs.Remove(stream.ReadUShort());
        }

        public static void ReadGuideSetCmdMessage(PacketReader stream)
        {
        }

        public static void ReadGuideRemoveCmdMessage(PacketReader stream)
        {
        }
    }
}
