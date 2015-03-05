using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using RakNet;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Server.Network.Messages.VobCommands
{
    class SpawnVobMessage
    {
        public static void Write(Vob vob)
        {
            Write(vob, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS);
        }
        public static void Write(Vob vob, AddressOrGUID addguid)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SpawnVobMessage);
            stream.Write(vob.ID);
            stream.Write(true);
            stream.Write(vob.Map);
            stream.Write(vob.Position);
            stream.Write(vob.Direction);

            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, addguid, true);

        }
    }
}
