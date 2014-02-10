using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class DisconnectedMessage : Message
    {
        ulong guid;
        int id;
        public DisconnectedMessage(int id, ulong guid)
        {
            this.guid = guid;
            this.id = id;


        }

        public class DisconnectedEventArgs : EventArgs
        {
            public int id;
            public ulong guid;
        }
        public static event EventHandler<DisconnectedEventArgs> Disconnected;

        public override void Write(RakNet.BitStream stream, Server server)
        {
            //Player pl = Player.getPlayerByGuid(guid, Program.playerList);

            if (Disconnected != null)
            {
                Disconnected(this, new DisconnectedEventArgs() { id = id, guid = guid });
            }

            if (id == -1)
                return;
            Player pl = Player.getPlayer(id, Program.playList);

            foreach (NPC npc in Program.npcList)
            {
                if (npc.controller == pl)
                {
                    npc.controller = null;
                }
            }
            if (Program.playerDict.ContainsKey(pl.id))
                Program.playerDict.Remove(pl.id);
            Program.playerList.Remove(pl);
            Program.playList.Remove(pl);
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.DisconnectedMessage);
            stream.Write(id);
            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
