using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using RakNet;
using GUC.Server.Network;

namespace GUC.Server.Sumpfkraut
{
    class TradeMessage : IMessage
    {
        public TradeMessage()
        {
        }

        public delegate void RequestMessageHandler(Player requester, Player target);
        public event RequestMessageHandler OnRequestMessage;

        public delegate void BreakMessageHandler(Player sender);
        public event BreakMessageHandler OnBreakMessage;

        public delegate void OfferMessageHandler(Player trader, Item item, bool add);
        public event OfferMessageHandler OnOfferMessage;

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, GUC.Server.Network.Server server)
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

                if (OnRequestMessage != null && (requester.Position - target.Position).Length < 300)
                {
                    OnRequestMessage(requester, target);
                }
            }
            else if (status == TradeStatus.Break)
            {
                int senderID = 0;
                stream.Read(out senderID);

                if (OnBreakMessage != null)
                {
                    OnBreakMessage((Player)sWorld.VobDict[senderID]);
                }
            }
            else if (status == TradeStatus.OfferItem || status == TradeStatus.RemoveItem)
            {
                int traderID = 0;
                int itemID = 0;

                stream.Read(out traderID);
                stream.Read(out itemID);

                if (OnOfferMessage != null)
                {
                    OnOfferMessage((Player)sWorld.VobDict[traderID], (Item)sWorld.VobDict[itemID], status == TradeStatus.OfferItem);
                }
            }
        }

        public void SendOffer(Player trader, Item item, bool add)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            if (add) stream.Write((byte)TradeStatus.OfferItem);
            else stream.Write((byte)TradeStatus.RemoveItem);
            stream.Write(item.ID);
            using (RakNetGUID guid = trader.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void SendAccept(Player pl1, Player pl2)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Accept);
            stream.Write(pl1.ID);
            using (RakNetGUID guid = pl2.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Accept);
            stream.Write(pl2.ID);
            using (RakNetGUID guid = pl1.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void SendBreak(Player pl)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Break);
            using (RakNetGUID guid = pl.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }
    }
}
