using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class LevelChangeMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id;
            String name;

            stream.Read(out id);
            stream.Read(out name);

            
            Player pl = Player.getPlayerSort(id, Program.playerList);

            String oldWorld = pl.actualMap;

            if (pl != null)
            {
                pl.actualMap = Player.getMap(name);

                foreach (NPC npc in Program.npcList)
                {
                    if (npc.controller == pl)
                    {
                        new NPCControllerMessage().Write(stream, server, npc, false);
                        npc.controller = null;
                    }
                }
            }

            Program.scriptManager.OnLevelChange(new Scripting.Player(pl), pl.actualMap, oldWorld);



            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.LevelChangeMessage);
            stream.Write(id);
            stream.Write(name);
            server.server.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);

        }
    }
}
