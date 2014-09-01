using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.NpcCommands
{
    class AnimationUpdateMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int plID = 0;
            short anim = 0;

            stream.Read(out plID);
            stream.Read(out anim);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPC!");

            ((NPCProto)vob).Animation = anim;


            Write((NPCProto)vob, packet.guid);
        }

        public static void Write(NPCProto proto)
        {
            Write(proto, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS);
        }
        public static void Write(NPCProto proto, AddressOrGUID addGuid)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationUpdateMessage);
            stream.Write(proto.ID);
            stream.Write(proto.Animation);
            Program.server.ServerInterface.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, addGuid, true);
        }
    }
}
