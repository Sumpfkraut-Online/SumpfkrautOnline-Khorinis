using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages
{
    class TradeMessage : IMessage
    {
        TradeGUI gui;

        public TradeMessage()
        {
            gui = new TradeGUI(SendTradeRequest, SendOffer, TakeBack);
        }

        void SendTradeRequest(int ID)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Request);

            stream.Write(Player.Hero.ID);
            stream.Write(ID);

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        void SendOffer(int itemNum)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.OfferItem);

            stream.Write(Player.Hero.ID);
            stream.Write(itemNum);

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        void TakeBack(int itemNum)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.OfferItem);

            stream.Write(Player.Hero.ID);
            stream.Write(itemNum);

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte b;
            stream.Read(out b);
            TradeStatus status = (TradeStatus)b;

            if (status == TradeStatus.Start)
            {
                int traderID;
                stream.Read(out traderID);

                gui.StartTrading(traderID);
            } 
            else if (status == TradeStatus.OfferItem)
            {
                int traderID;
                int itemNum;
                stream.Read(out traderID);//dumm, lieber übern server machen lassen
                stream.Read(out itemNum);

            }
        }
    }
}
