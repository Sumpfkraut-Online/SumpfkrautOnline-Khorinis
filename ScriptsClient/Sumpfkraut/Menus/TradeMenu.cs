using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;

using GUC.Network;
using GUC.Enumeration;
using GUC.Client.GUI;
using GUC.Client.Network.Messages;

namespace GUC.Client.Menus
{
    /*class TradeMenu : GUCMenu
    {

        private static TradeMenu trade;
        public static TradeMenu GetTrade()
        {
            if (trade == null)
            {
                trade = new TradeMenu();
            }
            return trade;
        }
        
        public VirtualKeys RequestTradeKey = VirtualKeys.T;
        public VirtualKeys AcceptTradeKey = VirtualKeys.T;

        GUCInventory inv;
        GUCInventory sellInv;
        GUCInventory buyInv;

        Dictionary<uint, Item> itemsInv;
        Dictionary<uint, Item> itemsBuy;
        Dictionary<uint, Item> itemsSell;

        //public Dictionary<uint, TradeRequest> requests;

        bool confirmed = false;
        bool otherConfirmed = false;

        public TradeMenu()
        {
            itemsBuy = new Dictionary<uint, Item>();
            itemsSell = new Dictionary<uint, Item>();

            buyInv = new GUCInventory(80, 200, 2, 3, "Inv_Back_Sell.tga");
            sellInv = new GUCInventory(290, 200, 2, 3, "Inv_Back_Sell.tga");
            inv = new GUCInventory(500, 200, 4, 3, "Inv_Back.tga");

            sellInv.RightInfoBox = "";
            buyInv.RightInfoBox = "";

            sellInv.SetContents(itemsSell);
            buyInv.SetContents(itemsBuy);

            inv.left = sellInv;
            sellInv.right = inv;
            sellInv.left = buyInv;
            buyInv.right = sellInv;

            //requests = new Dictionary<uint, TradeRequest>();
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Escape)
            {
                TradeMessage.SendBreak();
                TradeCancelled();
            }
            else if (key == VirtualKeys.Control)
            {
                if (inv.Enabled)
                {
                    TradeMessage.OfferItem(inv.selectedItem);
                }
                else if (sellInv.Enabled)
                {
                    TradeMessage.RemoveItem(sellInv.selectedItem);
                }
            }
            else if(key == AcceptTradeKey)
            {
                if (!confirmed)
                {
                    confirmed = true;
                    TradeMessage.ConfirmOffer();
                    sellInv.SetAcceptStateColor(true);
                }
                else
                {
                    TradeMessage.DeclineOffer();
                    DeclineOffer();
                }
            }
            else
            {
                if (inv.Enabled)
                {
                    inv.KeyPressed(key);
                    return;
                }
                else if (sellInv.Enabled)
                {
                    sellInv.KeyPressed(key);
                    return;
                }
                else if (buyInv.Enabled)
                {
                    buyInv.KeyPressed(key);
                    return;
                }
            }
        }

        public override void Open()
        {
            itemsInv = new Dictionary<uint, Item>(Player.Inventory);

            buyInv.Show();
            sellInv.Show();
            inv.Show();

            inv.SetContents(Player.Inventory);
            inv.Enabled = true;
            base.Open();
        }

        public override void Close()
        {
            inv.Hide();
            itemsBuy.Clear();
            buyInv.Hide();
            buyInv.SetContents(itemsBuy);
            buyInv.SetAcceptStateColor(false);
            itemsSell.Clear();
            sellInv.Hide();
            sellInv.SetContents(itemsSell);
            sellInv.SetAcceptStateColor(false);
            base.Close();
        }

        public void DeclineOffer()
        {
            confirmed = false;
            sellInv.SetAcceptStateColor(false);
        }

        public void OtherConfirmedOffer()
        {
            otherConfirmed = true;
            buyInv.SetAcceptStateColor(true);
        }

        public void OtherDeclinedOffer()
        {
            otherConfirmed = false;
            buyInv.SetAcceptStateColor(false);
        }

        public void TradeRequested(NPC trader)
        {
            //requests.Add(trader.ID, new TradeRequest(DateTime.Now.Ticks, true));
            // eine anfrage zum Handel hat stattgefunden
            ShortMsg(trader.Name.ToString() + " möchte mit dir handeln");
        }

        public void TradeAccepted(NPC trader)
        {
            // Anfrage akzeptiert -> Handel beginnt
            Open();
            sellInv.LeftInfoBox = Player.Hero.Name;
            buyInv.LeftInfoBox = trader.Name;
        }

        public void TradeCancelled()
        {
            // Handel abgebrochen
            ShortMsg("Der Handel wurde abgebrochen");
            Close();
        }

        public void TradeDone()
        {
            // Handel erfolgreich beendet
            ShortMsg("Der Handel war erfolgreich");
            Close();
        }

        public void ChangeItem(Item item, bool self, bool add)
        {
            if(confirmed)
            {
                confirmed = false;
                sellInv.SetAcceptStateColor(false);
            }

            if(otherConfirmed)
            { 
                OtherDeclinedOffer();
            }

            if (self)
            {
                if (add)
                {
                    itemsSell.Add(item.ID, item);
                    itemsInv.Remove(item.ID);
                }
                else
                {
                    itemsInv.Add(item.ID, item);
                    itemsSell.Remove(item.ID);
                }
                inv.SetContents(itemsInv);
                sellInv.SetContents(itemsSell);
            }
            else
            {
                if (add)
                {
                    itemsBuy.Add(item.ID, item);
                }
                else
                {
                    itemsBuy.Remove(item.ID);
                }
                buyInv.SetContents(itemsBuy);
            }
        }
        
        public void RequestTrade()
        {
            oCNpc ocnpc = Player.Hero.gVob.GetFocusNpc();
            Vob vob;
            World.vobAddr.TryGetValue(ocnpc.Address, out vob);
            NPC npc = (NPC)vob;
            if (npc != null)
            {
                TradeMessage.SendRequest(npc.ID);
                ShortMsg("Handelsanfrage an " + npc.Name);
                /*if(!requests.ContainsKey(npc.ID))
                {
                    TradeMessage.SendRequest(npc.ID);
                    ShortMsg("Handelsanfrage an " + npc.Name);
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "Erstelle Anfrage an ID= "+npc.ID.ToString(), 0, "GUCInventory.cs", 0);
                    requests.Add(npc.ID, new TradeRequest(DateTime.Now.Ticks, false)); // delay: + 300000000
                    
                }
                else if (requests[npc.ID].Received)
                {
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "Erhaltene Anfrage bestätigen!", 0, "GUCInventory.cs", 0);
                    TradeMessage.SendRequest(npc.ID);
                }
                else if(requests[npc.ID].Time < DateTime.Now.Ticks)
                {
                    TradeMessage.SendRequest(npc.ID);
                    ShortMsg("Handelsanfrage an " + npc.Name);
                    requests[npc.ID].Time = DateTime.Now.Ticks + 0; // 30s
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "Eine abgelaufener Request wird erneuert", 0, "GUCInventory.cs", 0);
                }*//*
            }
        }

        public void ShortMsg(string text)
        {
            zCView.Printwin(Program.Process, text);
        }
    }*/

    /*class TradeRequest
    {
        // this class is only for showing up the right ingame messages 
        public long Time;
        
        bool received;
        public bool Received
        {
            get
            {
                return received;
            }
            set
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', "setted request to "+value.ToString(), 0, "GUCInventory.cs", 0);
                received = value;
            }
        }


        public TradeRequest(long time, bool received)
        {
            Time = time;
            Received = received;
            zERROR.GetZErr(Program.Process).Report(2, 'G', "created request with "+received.ToString(), 0, "GUCInventory.cs", 0);
        }

    }*/
}
