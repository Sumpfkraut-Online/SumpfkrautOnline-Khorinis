using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Enumeration;
using RakNet;

namespace GUC.Network.Messages
{
    static class SpectatorMessage
    {
        public static void ReadPos(PacketReader stream, GameClient client)
        {            
            client.SetPosition(stream.ReadCompressedPosition(), false);
        }

        public static void WriteSpectatorMessage(GameClient client, Vec3f pos, Vec3f dir)
        {
            var stream = GameServer.SetupStream(NetworkIDs.SpectatorMessage);
            stream.Write(pos);
            stream.Write(dir);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
        }
    }
}
