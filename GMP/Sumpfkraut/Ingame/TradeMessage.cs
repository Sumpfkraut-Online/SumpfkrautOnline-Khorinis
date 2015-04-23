using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut.Ingame.GUI;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame
{
    class TradeMessage : IMessage
    {
        public delegate void AcceptionHandler(Player trader);
        public event AcceptionHandler OnAcceptMessage;

        public delegate void BreakHandler();
        public event BreakHandler OnBreakMessage;

        public delegate void ChangeItemsHandler(Item item, bool self, bool add);
        public event ChangeItemsHandler OnChangeItemMessage;

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

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte b;
            stream.Read(out b);
            TradeStatus status = (TradeStatus)b;

            if (status == TradeStatus.Accept)
            {
                int traderID;
                stream.Read(out traderID);

                Player pl = (Player)sWorld.VobDict[traderID];

                if (pl != null && OnAcceptMessage != null)
                {
                    OnAcceptMessage(pl);
                }
            }
            else if (status == TradeStatus.Break)
            {
                if (OnBreakMessage != null) OnBreakMessage();
            }
            else if (status >= TradeStatus.SelfOfferItem)
            {
                int itemID;
                stream.Read(out itemID);
                Item item = (Item)sWorld.VobDict[itemID];
                
                if (item != null && OnChangeItemMessage != null)
                {
                    OnChangeItemMessage(item, status == TradeStatus.SelfOfferItem || status == TradeStatus.SelfRemoveItem, status == TradeStatus.SelfOfferItem || status == TradeStatus.OtherOfferItem);
                }
            }
        }
    }
}
