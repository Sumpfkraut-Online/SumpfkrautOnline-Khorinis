using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using Network.Types;

namespace GMP_Server.Net.Message
{
    public class PlayerStatusMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            RakNet.BitStream stream_2 = new BitStream();
            stream_2.Reset();
            stream_2.Write(packet.data, packet.length);
            stream_2.IgnoreBytes(2);

            
            byte type = 0;
            stream_2.Read(out type);

            
            int id = 0;
            stream_2.Read(out id);
            Player player = Player.getPlayerSort(id, Program.playerList);
            if (player == null)
                return;

            if (player.newPlayer && !player.isNPC)
            {
                Scripting.View.SendToPlayer(new Scripting.Player(player));
                Program.scriptManager.OnPlayerConnected(new Scripting.Player(player));
                player.newPlayer = false;
            }


            //Position auslesen:
            if (type == 1)
            {
                float[] pos = new float[3];
                for (int p = 0; p < 3; p++)
                    stream_2.Read(out pos[p]);


                player.pos = new float[] { pos[0], pos[1], pos[2] };
            }
            

            try
            {
                stream_2.Reset();
                stream_2.Dispose();
                server.server.Send(stream, RakNet.PacketPriority.IMMEDIATE_PRIORITY, RakNet.PacketReliability.UNRELIABLE_SEQUENCED, (char)0, packet.guid, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler bei PlayerStatusMessage!");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
