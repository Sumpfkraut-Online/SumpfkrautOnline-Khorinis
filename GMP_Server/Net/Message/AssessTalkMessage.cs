using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class AssessTalkMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            byte type = 0; int senderid, receiverid;
            stream.Read(out type);
            stream.Read(out senderid);//NPC
            stream.Read(out receiverid);//Spieler

            Console.WriteLine("AssessTalkMessage: "+type+" | "+senderid +" | "+receiverid);

            Player talk_player = Player.getPlayerSort(receiverid, Program.playerList);
            //NPC suchen -> TODO: Verbessern
            NPC talk_npc = null;;
            foreach(NPC npc in Program.npcList)
            {
                if(npc.npcPlayer.id == senderid)
                {
                    talk_npc = npc;
                }
            }

            if (type == 0)//Weiterleiten
            {
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetWorkIDS.AssessTalkMessage);
                stream.Write(type);
                stream.Write(senderid);
                stream.Write(receiverid);
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, talk_npc.controller.guid, false);
            }
            else if (type == 3)//Weiterleiten
            {
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetWorkIDS.AssessTalkMessage);
                stream.Write(type);
                stream.Write(senderid);
                stream.Write(receiverid);
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, talk_player.guid, false);
            }
            else//Controller hat überprüft obs iO. ist und übergibt die Kontrolle
            {
                new NPCControllerMessage().Write(stream, server, talk_npc, false);
                talk_npc.controller = talk_player;
                new NPCControllerMessage().Write(stream, server, talk_npc, true);
            }

        }
    }
}
