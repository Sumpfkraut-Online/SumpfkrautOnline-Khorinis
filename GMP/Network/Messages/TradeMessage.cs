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

        }

        public void Init()
        {
            gui = new TradeGUI(SendTradeRequest);
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
        }
    }
}
