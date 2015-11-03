using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Server.Network.Messages;

namespace GUC.Server.Interface
{
    public class Trade
    {
        private static Trade tradeCtrl;
        public static Trade GetTrade()
        {
            if (tradeCtrl == null)
            {
                tradeCtrl = new Trade();
            }
            return tradeCtrl;
        }

        private List<Request> requests;
        private List<TradeCouple> tradings;

        public Trade()
        {
            requests = new List<Request>();
            tradings = new List<TradeCouple>();
        }


        private class Request
        {
            public NPC requester;
            public NPC target;
            public long time;

            public Request(NPC requester, NPC target)
            {
                Log.Logger.log("created new request");
                this.requester = requester;
                this.target = target;
                time = DateTime.Now.Ticks + 300000000; // 30 secs
            }
        }

        private class TradeCouple
        {
            public NPC pl1;
            public List<Item> list1;
            public NPC pl2;
            public List<Item> list2;
            public bool accepted1;
            public bool accepted2;

            public TradeCouple(NPC p1, NPC p2)
            {
                pl1 = p1; pl2 = p2;
                list1 = new List<Item>();
                list2 = new List<Item>();
                accepted1 = false;
                accepted2 = false;
            }
        }

        public void OnRequestMessage(NPC requester, NPC target)
        {
            Log.Logger.log("request by " + target.ID.ToString() + " to " + requester.ID.ToString());

            // target already in trade?
            foreach (TradeCouple tc in tradings)
            {
                if (tc.pl1 == target || tc.pl2 == target)
                {
                    return;
                }

                if (tc.pl1 == requester || tc.pl2 == requester)
                {
                    return;
                }
            }

            CleanRequestList();

            foreach (Request req in requests)
            {
                if (req.requester == target && req.target == requester)
                {
                    Log.Logger.log("request accepted -> send message");
                    AddCouple(requester, target);
                    requests.Remove(req);
                    TradeMessage.SendAccept(requester, target);
                    return;
                }
                else if (req.requester == requester && req.target == target)
                {
                    req.time = DateTime.Now.Ticks + 300000000; //30sec long
                    return;
                }
            }

            // no request so far -> create new request
            Request request = new Request(requester, target);
            requests.Add(request);
        }

        public void OnBreakMessage(NPC sender)
        {
            for (int i = tradings.Count - 1; i >= 0; i--)
            {
                if (tradings[i].pl1 == sender)
                {
                    TradeMessage.SendBreak(tradings[i].pl2);
                    tradings.Remove(tradings[i]);
                }
                else if (tradings[i].pl2 == sender)
                {
                    TradeMessage.SendBreak(tradings[i].pl1);
                    tradings.Remove(tradings[i]);
                }
            }
        }

        public void OfferDeclined(NPC sender, bool send)
        {
            for (int i = tradings.Count - 1; i >= 0; i--)
            {
                if (tradings[i].pl1 == sender)
                {
                    tradings[i].accepted1 = false;
                    if (send)
                    {
                        TradeMessage.SendOfferDeclined(tradings[i].pl2);
                    }
                }
                else if (tradings[i].pl2 == sender)
                {
                    tradings[i].accepted2 = false;
                    if (send)
                    {
                        TradeMessage.SendOfferDeclined(tradings[i].pl1);
                    }
                }
            }
        }

        public void OfferConfirmed(NPC sender)
        {
            for (int i = tradings.Count - 1; i >= 0; i--)
            {
                if (tradings[i].pl1 == sender)
                {
                    tradings[i].accepted1 = true;
                    TradeMessage.SendOfferConfirmed(tradings[i].pl2);
                }
                else if (tradings[i].pl2 == sender)
                {
                    tradings[i].accepted2 = true;
                    TradeMessage.SendOfferConfirmed(tradings[i].pl1);
                }

                if(tradings[i].accepted1 && tradings[i].accepted2)
                {
                    Log.Logger.log("both accepted the trade");
                    // trade has been accepted by both players
                }
            }
        }

        public void OnOfferMessage(NPC trader, Item item, bool add)
        {
            Log.Logger.log(trader.ID.ToString() + " offers " + item.Instance.Name.ToString());
            if (!trader.HasItem(item))
                return;

            foreach (TradeCouple tc in tradings)
            {
                if (trader == tc.pl1)
                {
                    //if (tc.accepted1 || tc.accepted2)
                    //    return;
                    if (add && !tc.list1.Contains(item))
                    {
                        tc.list1.Add(item);
                    }
                    else if (tc.list1.Contains(item))
                    {
                        tc.list1.Remove(item);
                    }
                    tc.accepted2 = false;
                    TradeMessage.SendOffer(tc.pl1, tc.pl2, item, add);
                    return;
                }
                else if (trader == tc.pl2)
                {
                   // if (tc.accepted1 || tc.accepted2)
                   //     return;
                    if (add && !tc.list2.Contains(item))
                    {
                        tc.list2.Add(item);
                    }
                    else if (tc.list2.Contains(item))
                    {
                        tc.list2.Remove(item);
                    }
                    tc.accepted1 = false;
                    TradeMessage.SendOffer(tc.pl2, tc.pl1, item, add);
                    return;
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

        private void AddCouple(NPC pl1, NPC pl2)
        {
            for (int i = requests.Count - 1; i >= 0; i--)
                if (requests[i].requester == pl1 || requests[i].target == pl1 || requests[i].requester == pl2 || requests[i].target == pl2)
                    requests.Remove(requests[i]);

            tradings.Add(new TradeCouple(pl1, pl2));
        }
    }
}