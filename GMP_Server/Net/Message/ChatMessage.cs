using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class ChatMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            byte type;
            int id;
            String name;
            String content;
            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out name);
            BitStreamFunc.ReadNames(stream, out content);


            Player sendPL = Player.getPlayer(id, Program.playerList);
            if (sendPL.isMuted)
                return;


            bool sendChat = true;
            sendChat = Program.scriptManager.OnChatMessageReceived(new Scripting.Player(sendPL), ref name, ref content, ref type);

            if (!sendChat)
                return;


            Log.Logger.log(Log.Logger.LOG_INFO, packet.guid+" " +packet.systemAddress+" Chat Type:"+ type +" from: "+name+": "+content);
            


            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ChatMessage);
            stream.Write(id);
            stream.Write((byte)type);
            stream.Write(name);
            


            if (type == 1)
            {
                Player pl = null;
                foreach (Player pln in Program.playList)
                {
                    if (content.StartsWith("/" + pln.name + " "))
                    {
                        pl = pln;
                        content = content.Remove(0, ("/" + pln.name + " ").Length);
                        break;
                    }
                }

                BitStreamFunc.WriteNames(stream, content);
                if(pl != null)
                    server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, pl.systemAddress, false);
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, false);
            }
            else
            {
                BitStreamFunc.WriteNames(stream, content);
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            }
        }
    }
}
