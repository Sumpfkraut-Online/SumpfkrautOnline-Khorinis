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

                if (OnRequestMessage != null && requester != null && target != null)
                {
                    OnRequestMessage(requester, target);
                }
            }
            else if (status == TradeStatus.Break)
            {
                int senderID = 0;
                stream.Read(out senderID);
                Player pl = (Player)sWorld.VobDict[senderID];
                if (OnBreakMessage != null && pl != null)
                {
                    OnBreakMessage(pl);
                }
            }
            else if (status >= TradeStatus.SelfOfferItem)
            {
                int traderID = 0;
                int itemID = 0;

                stream.Read(out traderID);
                stream.Read(out itemID);

                Player pl = (Player)sWorld.VobDict[traderID];
                Item item = (Item)sWorld.VobDict[itemID];

                if (OnOfferMessage != null && pl != null && item != null)
                {
                    OnOfferMessage(pl, item, status == TradeStatus.SelfOfferItem);
                }
            }
        }

        public void SendOffer(Player from, Player to, Item item, bool add)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            if (add) stream.Write((byte)TradeStatus.SelfOfferItem);
            else stream.Write((byte)TradeStatus.SelfRemoveItem);
            stream.Write(item.ID);
            using (RakNetGUID guid = from.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            if (add) stream.Write((byte)TradeStatus.OtherOfferItem);
            else stream.Write((byte)TradeStatus.OtherRemoveItem);
            stream.Write(item.ID);
            using (RakNetGUID guid = to.GUID)
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
