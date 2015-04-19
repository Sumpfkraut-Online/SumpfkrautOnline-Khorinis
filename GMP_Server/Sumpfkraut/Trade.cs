using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Server.Network.Messages;
using RakNet;


namespace GUC.Server.Sumpfkraut
{
    public class Trade
    {
        //Adds the NetworkID to the listener
        private TradeMessage messenger;
        public Trade()
        {
            if (!Program.server.MessageListener.ContainsKey((byte)NetworkID.TradeMessage))
            {
                messenger = new TradeMessage();
                Program.server.MessageListener.Add((byte)NetworkID.TradeMessage, messenger);
            }
            else
            {
                messenger = (TradeMessage)Program.server.MessageListener[(byte)NetworkID.TradeMessage];
            }

            requests = new List<Request>();
            tradings = new List<TradeCouple>();
            messenger.OnRequestMessage += HandleRequest;
            messenger.OnBreakMessage += BreakTrade;
            messenger.OnOfferMessage += HandleOffer;
        }

        private class Request
        {
            public Player requester;
            public Player target;
            public long time;
        }

        private class TradeCouple
        {
            public Player pl1;
            public List<Item> list1;
            public Player pl2;
            public List<Item> list2;

            public TradeCouple(Player p1, Player p2)
            {
                pl1 = p1; pl2 = p2;
                list1 = new List<Item>();
                list2 = new List<Item>();
            }
        }

        private List<Request> requests;
        private List<TradeCouple> tradings;

        private void HandleOffer(Player trader, Item item, bool add)
        {
            foreach(TradeCouple tc in tradings)
            {
                if (trader == tc.pl1 && trader.ItemList.Contains(item))
                {
                    if (add)
                    {
                        tc.list1.Add(item);
                    }
                    else if (tc.list1.Contains(item))
                    {
                        tc.list1.Remove(item);
                    }
                    messenger.SendOffer(tc.pl2, item, add);
                    return;
                }
                else if (trader == tc.pl2 && trader.ItemList.Contains(item))
                {
                    if (add)
                    {
                        tc.list2.Add(item);
                    }
                    else if (tc.list2.Contains(item))
                    {
                        tc.list2.Remove(item);
                    }
                    messenger.SendOffer(tc.pl1, item, add);
                    return;
                }
            }
        }

        private void HandleRequest(Player requester, Player target)
        {
            BitStream stream = Program.server.SendBitStream;
            foreach (TradeCouple tc in tradings)
            {
                if (tc.pl1 == target || tc.pl2 == target)
                {
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkID.ChatMessage);
                    stream.Write((byte)ChatTextType.Global);
                    stream.Write("Diese Person handelt bereits.");

                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                    return;
                }
            }

            CleanRequestList();

            foreach(Request req in requests)
            {
                if (req.requester == target && req.target == requester)
                {
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkID.ChatMessage);
                    stream.Write((byte)ChatTextType.Global);
                    stream.Write(requester.Name + " accepted " + target.Name);

                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                    AddCouple(requester, target);

                    messenger.SendAccept(requester, target);
                    return;
                }
                else if (req.requester == requester && req.target == target)
                {
                    req.time = DateTime.Now.Ticks + 300000000; //30sec long

                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkID.ChatMessage);
                    stream.Write((byte)ChatTextType.Global);
                    stream.Write(requester.Name + " requested " + target.Name + " again.");

                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                    return;
                }
            }

            Request newReq = new Request();
            newReq.requester = requester;
            newReq.target = target;
            newReq.time = DateTime.Now.Ticks + 300000000; //30sec long
            requests.Add(newReq);

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);
            stream.Write((byte)ChatTextType.Global);
            stream.Write(requester.Name + " requested " + target.Name);

            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        private void BreakTrade(Player sender)
        {
            foreach(TradeCouple tc in tradings)
            {
                if (tc.pl1 == sender)
                {
                    messenger.SendBreak(tc.pl2);
                    tradings.Remove(tc);
                }
                else if (tc.pl2 == sender)
                {
                    messenger.SendBreak(tc.pl1);
                    tradings.Remove(tc);
                }
            }
        }

        private void CleanRequestList()
        {
            long now = DateTime.Now.Ticks;
            foreach (Request req in requests)
            {
                if (req.time < now)
                    requests.Remove(req);
            }
        }

        private void AddCouple(Player pl1, Player pl2)
        {
            foreach (Request req in requests)
                if (req.requester == pl1 || req.target == pl1 || req.requester == pl2 || req.target == pl2)
                    requests.Remove(req);

            tradings.Add(new TradeCouple(pl1,pl2));
        }
    }
}
