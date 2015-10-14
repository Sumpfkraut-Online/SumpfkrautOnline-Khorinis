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
        public void Read(BitStream stream, Client client)
        {
            int casterID = 0;

            stream.Read(out casterID);


            Vob casterVob = null;
            NPC caster = null;

            sWorld.VobDict.TryGetValue(casterID, out casterVob);
            

            if (casterVob == null)
                throw new Exception("Caster was not found!");
            if (!(casterVob is NPC))
                throw new Exception("Caster was not a npcproto " + casterVob);
            caster = (NPC)casterVob;


            using (RakNetGUID guid = new RakNetGUID(client.guid))
            Write(caster, guid);
        }


        public static void Write(NPC proto)
        {
            Write(proto, null);
        }
        public static void Write(NPC proto, AddressOrGUID guidExclude)
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
