using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.VobCommands
{
    class CreateVobMessage
    {
        public static void Write(Vob vob)
        {
            Write(vob, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS);
        }
        public static void Write(Vob vob, AddressOrGUID addguid)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            //Console.WriteLine("CreateVob: "+vob.VobType);

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CreateVobMessage);
            stream.Write((int)vob.VobType);
            vob.Write(stream);

            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, addguid, true);

        }
    }
}
