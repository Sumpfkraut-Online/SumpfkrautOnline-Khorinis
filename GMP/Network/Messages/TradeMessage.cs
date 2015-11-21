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
    static class TradeMessage
    {

        public static void Read(BitStream stream)
        {
            // call accept - break - change item

            TradeStatus tradeState = (TradeStatus)stream.mReadByte();
            TradeMenu trade = TradeMenu.GetTrade();

            Item item = null;
            NPC npc = null;

            if (tradeState >= TradeStatus.SelfOfferItem)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', "received offer" , 0, "GameState.cs", 0);
                uint   id          = stream.mReadUInt();
                ushort instanceId  = stream.mReadUShort();
                ushort amount      = stream.mReadUShort();
                ushort condition   = stream.mReadUShort();
                string specialLine = stream.mReadString();

                if (!World.itemDict.TryGetValue(id, out item))
                {
                    // item not existing yet -> creating item
                    Item newItem = new Item(id, instanceId);
                    newItem.Amount = amount;
                    newItem.Condition = condition;
                    newItem.specialLine = specialLine;
                    item = newItem;
                    World.itemDict[id] = newItem;
                }
                else
                {
                    item = World.itemDict[id];
                }
            }
            else
            {
                uint playerID = stream.mReadUInt();
                World.npcDict.TryGetValue(playerID, out npc);
            }

            switch (tradeState)
            {
                case TradeStatus.Request:
                    if (npc == null) return;
                    trade.TradeRequested(npc);
                    break;
                case TradeStatus.Accept:
                    if (npc == null) return;
                    trade.TradeAccepted(npc);
                    break;
                case TradeStatus.Break:
                    trade.TradeCancelled();
                    break;
                case TradeStatus.ConfirmOffer:
                    trade.OtherConfirmedOffer();
                    break;
                case TradeStatus.DeclineOffer:
                    trade.OtherDeclinedOffer();
                    break;
                case TradeStatus.TradeDone:
                    trade.TradeDone();
                    break;
                case TradeStatus.SelfOfferItem:
                    trade.ChangeItem(item, true, true);
                    break;
                case TradeStatus.SelfRemoveItem:
                    trade.ChangeItem(item, true, false);
                    break;
                case TradeStatus.OtherOfferItem:
                    trade.ChangeItem(item, false, true);
                    break;
                case TradeStatus.OtherRemoveItem:
                    trade.ChangeItem(item, false, false);
                    break;
            }
        }

        public static void ConfirmOffer()
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.ConfirmOffer);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void DeclineOffer()
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.DeclineOffer);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void RemoveItem(Item item)
        {
            if (item == null)
                return;
            BitStream stream = Program.client.SetupSendStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.SelfRemoveItem);
            stream.mWrite(item.ID);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
            TradeMenu.GetTrade().DeclineOffer();
        }

        public static void OfferItem(Item item)
        {
            if (item == null)
                return;
            BitStream stream = Program.client.SetupSendStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.SelfOfferItem);
            stream.mWrite(item.ID);
            stream.mWrite(item.instance.ID);
            stream.mWrite(item.Amount);
            stream.mWrite(item.Condition);
            stream.mWrite(item.specialLine);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
            TradeMenu.GetTrade().DeclineOffer();
        }

        public static void SendRequest(uint npcId)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.Request);
            stream.mWrite(npcId);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void SendBreak()
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.TradeMessage);
            stream.mWrite((byte)TradeStatus.Break);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }

    }
}