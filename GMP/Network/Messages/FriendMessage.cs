using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using Injection;
using RakNet;
using GMP.Modules;
using GMP.Helper;

namespace GMP.Network.Messages
{
    public class FriendMessage : Message
    {
        public void Write(RakNet.BitStream stream, Client client, Player player, byte type)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.FriendMessage);
            stream.Write(Program.Player.id);
            stream.Write(player.id);
            stream.Write(type);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.FriendMessage))
                StaticVars.sStats[(int)NetWorkIDS.FriendMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.FriendMessage] += 1;
        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            int senderID;
            byte type;

            stream.Read(out senderID);
            stream.Read(out type);


            if (type == 2)
                type = 3;
            Player pl = Player.getPlayer(senderID, StaticVars.playerlist);
            if (pl == null)
                return;
            pl.isFriend = type;
            
            NPCHelper.setFriend(pl);
        }
    }
}
