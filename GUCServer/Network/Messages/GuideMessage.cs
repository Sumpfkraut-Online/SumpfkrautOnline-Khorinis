using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Enumeration;
using RakNet;

namespace GUC.Network.Messages
{
    static class GuideMessage
    {
        static PacketWriter guideWriter = new PacketWriter(4);
        public static void WriteAddGuidableMessage(GameClient client, GuidedVob vob)
        {
            guideWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            guideWriter.Write((byte)NetworkIDs.GuideAddMessage);
            guideWriter.Write((ushort)vob.ID);
            client.Send(guideWriter, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            guideWriter.Reset();
        }
    }
}
