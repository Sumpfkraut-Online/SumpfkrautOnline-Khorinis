using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Enumeration;
//using GUC.Server.Interface;

namespace GUC.Server.Network.Messages
{
    static class TradeMessage
    {
        /*public static void Read(BitStream stream, Client client)
        {
            byte b = stream.mReadByte();
            TradeStatus status = (TradeStatus)b;
            Trade trade = Trade.GetTrade();

            if (status == TradeStatus.Request)
            {
                uint targetID = stream.mReadUInt();
                NPC requester = client.Character;
                NPC target;
                Log.Logger.Log("request by " + targetID.ToString());
                client.Character.World.playerDict.TryGetValue(targetID, out target);

                if (requester != null && target != null)
                {
                    trade.OnRequestMessage(requester, target);
                }
            }
            else if (status == TradeStatus.Break)
            {
                NPC sender = client.Character;
                if (sender != null)
                {
                    trade.OnBreakMessage(sender);
                }
            }
            else if (status == TradeStatus.ConfirmOffer)
            {
                trade.OfferConfirmed(client.Character);
            }
            else if (status == TradeStatus.DeclineOffer)
            {
                trade.OfferDeclined(client.Character, true);
            }
            else if (status >= TradeStatus.SelfOfferItem)
            {
                trade.OfferDeclined(client.Character, false);

                ushort id = stream.mReadUShort();
                Item item = Server.sItemDict[id];
                NPC sender = client.Character;

                if (sender != null && item != null)
                {
                    trade.OnOfferMessage(sender, item, status == TradeStatus.SelfOfferItem);
                }
            }
        }

        public static void SendOffer(NPC from, NPC to, Item item, bool add)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            if (add) stream.Write((byte)TradeStatus.SelfOfferItem);
            else stream.Write((byte)TradeStatus.SelfRemoveItem);
            stream.Write(item.ID); // back to from ->self offer/remove
            stream.Write(item.Instance.ID);
            stream.Write(item.Amount);
            stream.Write(item.Condition);
            stream.Write(item.SpecialLine);
            from.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');

            stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            if (add) stream.Write((byte)TradeStatus.OtherOfferItem);
            else stream.Write((byte)TradeStatus.OtherRemoveItem);
            stream.Write(item.ID); //other offer/remove
            stream.Write(item.Instance.ID);
            stream.Write(item.Amount);
            stream.Write(item.Condition);
            stream.Write(item.SpecialLine);
            to.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void SendAccept(NPC pl1, NPC pl2)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Accept);
            stream.Write(pl2.ID);
            pl1.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');

            stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Accept);
            stream.Write(pl1.ID);
            pl2.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void SendRequest(NPC from, NPC to)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Request);
            stream.Write(from.ID);
            to.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void SendTradeDone(NPC pl1, NPC pl2)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.TradeDone);
            pl1.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
            pl2.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void SendBreak(NPC pl)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Break);
            stream.Write(pl.ID);
            pl.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void SendOfferConfirmed(NPC pl)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.ConfirmOffer);
            pl.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }

        public static void SendOfferDeclined(NPC pl)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.DeclineOffer);
            pl.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I');
        }
        */
    }
}