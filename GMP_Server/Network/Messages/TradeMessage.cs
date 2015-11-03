using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Enumeration;
using GUC.Server.Interface;

namespace GUC.Server.Network.Messages
{
    static class TradeMessage
    {
        public static void Read(BitStream stream, Client client)
        {
            byte b = stream.mReadByte();
            TradeStatus status = (TradeStatus)b;
            Trade trade = Trade.GetTrade();

            if (status == TradeStatus.Request)
            {
                uint targetID = stream.mReadUInt();
                NPC requester = client.character;
                NPC target;
                Log.Logger.log("request by "+targetID.ToString());
                client.character.World.PlayerDict.TryGetValue(targetID, out target);

                if (requester != null && target != null)
                {
                    trade.OnRequestMessage(requester, target);
                }
            }
            else if (status == TradeStatus.Break)
            {
                NPC sender = client.character;
                if (sender != null)
                {
                    trade.OnBreakMessage(sender);
                }
            }
            else if(status == TradeStatus.ConfirmOffer)
            {
                trade.OfferConfirmed(client.character);
            }
            else if(status == TradeStatus.DeclineOffer)
            {
                trade.OfferDeclined(client.character, true);
            }
            else if (status >= TradeStatus.SelfOfferItem)
            {
                trade.OfferDeclined(client.character, false);

                ushort id = stream.mReadUShort();
                Item item = sWorld.ItemDict[id];
                NPC sender = client.character;

                if (sender != null && item != null)
                {
                    trade.OnOfferMessage(sender, item, status == TradeStatus.SelfOfferItem);
                }
            }
        }

        public static void SendOffer(NPC from, NPC to, Item item, bool add)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.TradeMessage);
            if (add) stream.mWrite((byte)TradeStatus.SelfOfferItem);
            else stream.mWrite((byte)TradeStatus.SelfRemoveItem);
            stream.mWrite(item.ID); // back to from ->self offer/remove
            stream.mWrite(item.Instance.ID);
            stream.mWrite(item.Amount);
            stream.mWrite(item.Condition);
            stream.mWrite(item.SpecialLine);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', from.client.guid, false);

            stream = Program.server.SetupStream(NetworkID.TradeMessage);
            if (add) stream.mWrite((byte)TradeStatus.OtherOfferItem);
            else stream.mWrite((byte)TradeStatus.OtherRemoveItem);
            stream.mWrite(item.ID); //other offer/remove
            stream.mWrite(item.Instance.ID);
            stream.mWrite(item.Amount);
            stream.mWrite(item.Condition);
            stream.mWrite(item.SpecialLine);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', to.client.guid, false);
        }

        public static void SendAccept(NPC pl1, NPC pl2)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.Accept);
            stream.mWrite(pl1.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', pl1.client.guid, false);

            stream = Program.server.SetupStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.Accept);
            stream.mWrite(pl2.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', pl2.client.guid, false);
        }

        public static void SendBreak(NPC pl)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.Break);
            stream.mWrite(pl.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', pl.client.guid, false);
        }

        public static void SendOfferConfirmed(NPC pl)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.ConfirmOffer);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', pl.client.guid, false);
        }

        public static void SendOfferDeclined(NPC pl)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.DeclineOffer);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', pl.client.guid, false);
        }

    }
}