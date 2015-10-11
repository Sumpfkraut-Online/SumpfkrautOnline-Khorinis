using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Enumeration;
using GUC.Client.WorldObjects;
using GUC.Network;
using GUC.Client.GUI;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

using GUC.Client.Menus;

namespace GUC.Client.Network.Messages
{
    public static class TradeMessage
    {

        public static void Read(BitStream stream)
        {
            // call accept - break - change item

            TradeStatus tradeState = (TradeStatus)stream.mReadByte();

            TradeMenu trade = TradeMenu.GetTrade();

            ItemInstance itemInst;

            if (tradeState >= TradeStatus.SelfOfferItem)
            {
                itemInst = null; // stream read
            }

            //switch (tradeState)
            //{
            //    case TradeStatus.Request:
            //        uint playerID = stream.mReadUInt();
            //        NPC npc;
            //        World.npcDict.TryGetValue(playerID, out npc);
            //        if (npc == null) return;
            //        trade.TradeRequested(npc);
            //        break;
            //    case TradeStatus.Accept:
            //        trade.Open();
            //        break;
            //    case TradeStatus.Break:
            //        trade.TradeCancelled();
            //        break;
            //    case TradeStatus.SelfOfferItem:
            //        trade.ChangeItem(itemInst, true, true);
            //        break;
            //    case TradeStatus.SelfRemoveItem:
            //        trade.ChangeItem(itemInst, true, false);
            //        break;
            //    case TradeStatus.OtherOfferItem:
            //        trade.ChangeItem(itemInst, false, true);
            //        break;
            //    case TradeStatus.OtherRemoveItem:
            //        trade.ChangeItem(itemInst, false, false);
            //        break;
            //}

            //GUC.Client.Menus.TradeMenu.GetTrade().ChangeItem();
        }
    /*
        public void RemoveItem(Item item)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.SelfRemoveItem);
            stream.Write(Player.Hero.ID);
            stream.Write(item.ID);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void OfferItem(Item item)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.SelfOfferItem);
            stream.Write(Player.Hero.ID);
            stream.Write(item.ID);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void SendRequest(int id)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Request);

            stream.Write(Player.Hero.ID);
            stream.Write(id);

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void SendBreak()
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Break);
            stream.Write(Player.Hero.ID);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
        */
    }
}
