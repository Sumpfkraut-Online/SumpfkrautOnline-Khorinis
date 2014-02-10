using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class StartLevelChangeMessage : Message
    {
        /// <summary>
        /// Nur einsetzen, wenn noch nicht im Spiel, also nur bei Start-Modulen um das Startlevel zu ändern!
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="server"></param>
        /// <param name="world"></param>
        /// <param name="reveicer"></param>
        public void Write(RakNet.BitStream stream, Server server, string world, AddressOrGUID reveicer)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.StartLevelChangeMessage);
            stream.Write(world);

            server.server.Send(stream, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, reveicer, false);
        }
    }
}
