using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Types;
using GUC.Network;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.VobCommands
{
    class SetVobPosDirMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int vobID = 0;
            Vec3f pos, dir;

            stream.Read(out vobID);
            stream.Read(out pos);
            stream.Read(out dir);

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            vob.Position = pos;
            vob.Direction = dir;



            Write(vob, packet.systemAddress);
        }


        public static void Write(Vob proto, AddressOrGUID addGuild)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVobPosDirMessage);
            stream.Write(proto.ID);

            stream.Write(proto.Position);
            stream.Write(proto.Direction);

            Program.server.server.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED, (char)0, addGuild, true);
        }
    }
}
