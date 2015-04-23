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
                Program.server.MessageListener.Add((byte)NetworkID.TradeMessage, new TradeMessage());
            }
            messenger = (TradeMessage)Program.server.MessageListener[(byte)NetworkID.TradeMessage];

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
            if (!trader.ItemList.Contains(item))
                return;

            foreach (TradeCouple tc in tradings)
            {
                if (trader == tc.pl1)
                {
                    if (add && !tc.list1.Contains(item))
                    {
                        tc.list1.Add(item);
                    }
                    else if (tc.list1.Contains(item))
                    {
                        tc.list1.Remove(item);
                    }
                    messenger.SendOffer(tc.pl1, tc.pl2, item, add);
                    return;
                }
                else if (trader == tc.pl2)
                {
                    if (add)
                    {
                        tc.list2.Add(item);
                    }
                    else if (tc.list2.Contains(item))
                    {
                        tc.list2.Remove(item);
                    }
                    messenger.SendOffer(tc.pl2, tc.pl1, item, add);
                    return;
                }
            }
        }

        private void HandleRequest(Player requester, Player target)
        {
            foreach (TradeCouple tc in tradings)
            {
                if (tc.pl1 == target || tc.pl2 == target)
                {
                    return;
                }
            }

            CleanRequestList();

            foreach (Request req in requests)
            {
                if (req.requester == target && req.target == requester)
                {
                    AddCouple(requester, target);
                    messenger.SendAccept(requester, target);
                    return;
                }
                else if (req.requester == requester && req.target == target)
                {
                    req.time = DateTime.Now.Ticks + 300000000; //30sec long
                    return;
                }
            }

            Request newReq = new Request();
            newReq.requester = requester;
            newReq.target = target;
            newReq.time = DateTime.Now.Ticks + 300000000; //30sec long
            requests.Add(newReq);
        }

        private void BreakTrade(Player sender)
        {
            for (int i = tradings.Count - 1; i >= 0; i--)
            {
                if (tradings[i].pl1 == sender)
                {
                    messenger.SendBreak(tradings[i].pl2);
                    tradings.Remove(tradings[i]);
                }
                else if (tradings[i].pl2 == sender)
                {
                    messenger.SendBreak(tradings[i].pl1);
                    tradings.Remove(tradings[i]);
                }
            }
        }

        private void CleanRequestList()
        {
            long now = DateTime.Now.Ticks;
            for (int i = requests.Count - 1; i >= 0; i--)
            {
                if (requests[i].time < now)
                    requests.Remove(requests[i]);
            }
        }

        private void AddCouple(Player pl1, Player pl2)
        {
            for (int i = requests.Count - 1; i >= 0; i--)
                if (requests[i].requester == pl1 || requests[i].target == pl1 || requests[i].requester == pl2 || requests[i].target == pl2)
                    requests.Remove(requests[i]);

            tradings.Add(new TradeCouple(pl1, pl2));
        }
    }
}
