using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using RakNet;
using GUC.WorldObjects.VobGuiding;

namespace GUC.Network.Messages
{
    static class GuideMessage
    {
        static PacketWriter guideWriter = new PacketWriter(100);
        public static void WriteAddGuidableMessage(GameClient client, GuidedVob vob, GuideCmd cmd)
        {
            guideWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);

            if (cmd == null)
            {
                guideWriter.Write((byte)NetworkIDs.GuideAddMessage);
            }
            else
            {
                guideWriter.Write((byte)NetworkIDs.GuideAddCmdMessage);
                guideWriter.Write(cmd.CmdType);
                cmd.WriteStream(guideWriter);
            }

            guideWriter.Write((ushort)vob.ID);
            client.Send(guideWriter, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            guideWriter.Reset();
        }

        public static void WriteGuidableCmdMessage(GameClient client, GuidedVob vob, GuideCmd cmd)
        {
            guideWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);

            if (cmd == null)
            {
                guideWriter.Write((byte)NetworkIDs.GuideRemoveCmdMessage);
            }
            else
            {
                guideWriter.Write((byte)NetworkIDs.GuideSetCmdMessage);
                guideWriter.Write(cmd.CmdType);
                cmd.WriteStream(guideWriter);
            }

            guideWriter.Write((ushort)vob.ID);
            client.Send(guideWriter, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            guideWriter.Reset();
        }

        public static void WriteRemoveGuidableMessage(GameClient client, GuidedVob vob)
        {
            guideWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            guideWriter.Write((byte)NetworkIDs.GuideRemoveMessage);
            guideWriter.Write((ushort)vob.ID);
            client.Send(guideWriter, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            guideWriter.Reset();
        }
    }
}
