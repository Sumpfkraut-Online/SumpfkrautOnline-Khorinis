using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class SpellInvestMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int casterID = 0;

            stream.Read(out casterID);


            Vob casterVob = null;
            NPCProto caster = null;

            sWorld.VobDict.TryGetValue(casterID, out casterVob);
            

            if (casterVob == null)
                throw new Exception("Caster was not found!");
            if (!(casterVob is NPCProto))
                throw new Exception("Caster was not a npcproto " + casterVob);
            caster = (NPCProto)casterVob;
            


            Write(caster, packet.guid);
        }


        public static void Write(NPCProto proto)
        {
            Write(proto, null);
        }
        public static void Write(NPCProto proto, AddressOrGUID guidExclude)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SpellInvestMessage);

            stream.Write(proto.ID);

            if (guidExclude == null)
                guidExclude = RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS;
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guidExclude, true);
        }
    }
}
