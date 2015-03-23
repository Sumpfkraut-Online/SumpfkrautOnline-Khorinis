using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using RakNet;

namespace GUC.Server.Network.Messages
{
    class TradeMessage : IMessage
    {
        public TradeMessage()
        {

        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            byte b;
            stream.Read(out b);
            TradeStatus status = (TradeStatus)b;

            if (status == TradeStatus.Request)
            {
                int requesterID = 0;
                int targetID = 0;

                stream.Read(out requesterID);
                stream.Read(out targetID);

                Player requester = (Player)sWorld.VobDict[requesterID];
                Player target = (Player)sWorld.VobDict[targetID];

                if (requester.WeaponMode == 0 && target.WeaponMode == 0 && (requester.Position - target.Position).Length < 100)
                {
                    StartTrade(requester, target);
                }
            }
        }

        void StartTrade(Player pl1, Player pl2)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Start);
            stream.Write(pl1.ID);
            using (RakNetGUID guid = pl2.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Start);
            stream.Write(pl2.ID);
            using (RakNetGUID guid = pl1.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }
    }
}
