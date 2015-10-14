using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.NpcCommands
{
    class NPCControllerMessage
    {

        public static void Write(NPC npc, Client controller, bool r)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCControllerMessage);
            stream.Write(npc.ID);
            stream.Write(r);
            
            using (RakNet.RakNetGUID guid = new RakNetGUID(controller.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

        }
    }
}
