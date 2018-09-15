using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;

using GUC.Network;

using GUC.GUI;
using GUC.Scripts.Sumpfkraut.GUI;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.ItemContainers;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using Gothic.Objects;

namespace GUC.Scripts.Sumpfkraut.Menus
{

    class TradeMenu : GUCMenu
    {

        public static readonly TradeMenu Menu = new TradeMenu();

        public VirtualKeys RequestTradeKey = VirtualKeys.T;
        public const VirtualKeys AcceptTradeKey = VirtualKeys.T;

        Dictionary<int, TradeRequest> requests;

        GUCInventory inv;
        GUCInventory sellInv;
        GUCInventory buyInv;

        ScriptInventory itemsInv;
        ScriptInventory itemsBuy;
        ScriptInventory itemsSell;

        bool confirmed;
        bool otherConfirmed;

        NPCInst player;

        public TradeMenu()
        {
            itemsBuy = new ScriptInventory(player);
            itemsSell = new ScriptInventory(player);

            buyInv = new GUCInventory(80, 200, 2, 3, "Inv_Back_Sell.tga");
            sellInv = new GUCInventory(290, 200, 2, 3, "Inv_Back_Sell.tga");
            inv = new GUCInventory(500, 200, 4, 3, "Inv_Back.tga");

            sellInv.RightInfoBox = "";
            buyInv.RightInfoBox = "";

            sellInv.SetContents(itemsSell);
            buyInv.SetContents(itemsBuy);

            inv.Left = sellInv;
            sellInv.Right = inv;
            sellInv.Left = buyInv;
            buyInv.Right = sellInv;

            player = ScriptClient.Client.Character;
            requests = new Dictionary<int, TradeRequest>();
        }

        public override void Open()
        {
            itemsInv = new ScriptInventory(player);

            buyInv.Show();
            sellInv.Show();
            inv.Show();

            inv.SetContents(player.Inventory);
            inv.Enabled = true;
            base.Open();
        }

        public override void Close()
        {
            inv.Hide();
            itemsBuy.Clear();
            buyInv.Hide();
            buyInv.SetContents(itemsBuy);
            //buyInv.SetAcceptStateColor(false);
            itemsSell.Clear();
            sellInv.Hide();
            sellInv.SetContents(itemsSell);
            //sellInv.SetAcceptStateColor(false);
            base.Close();
        }

        protected override void KeyDown(VirtualKeys key)
        {
            switch (key)
            {
                case VirtualKeys.Escape:
                    TradeCancelled();
                    break;
                case VirtualKeys.Control:
                    if (inv.Enabled)
                    {
                        NPCInst.Requests.OfferItem(player, inv.GetSelectedItem().ID);
                    }
                    else if (sellInv.Enabled)
                    {
                        NPCInst.Requests.RemoveItem(player, sellInv.GetSelectedItem().ID);
                    }
                    break;
                case AcceptTradeKey:
                    if (!confirmed)
                    {
                        confirmed = true;
                        NPCInst.Requests.ConfirmOffer(player);
                        //sellInv.SetAcceptStateColor(true);
                    }
                    else
                    {
                        NPCInst.Requests.DeclineOffer(player);
                        DeclineOffer();
                    }
                    break;

            }
        }

        public void DeclineOffer()
        {
            confirmed = false;
            //sellInv.SetAcceptStateColor(false);
        }

        public void OtherConfirmedOffer()
        {
            otherConfirmed = true;
            //buyInv.SetAcceptStateColor(true);
        }

        public void OtherDeclinedOffer()
        {
            otherConfirmed = false;
            //buyInv.SetAcceptStateColor(false);
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
            sellInv.LeftInfoBox = player.CustomName;
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

        public void ChangeItem(int itemID, bool self, bool add)
        {
            ItemInst item = player.Inventory.GetItem(itemID);
            if (confirmed)
            {
                confirmed = false;
                //sellInv.SetAcceptStateColor(false);
            }

            if (otherConfirmed)
            {
                OtherDeclinedOffer();
            }

            if (self)
            {
                if (add)
                {
                    itemsSell.AddItem(item);
                    itemsInv.RemoveItem(item);
                }
                else
                {
                    itemsInv.AddItem(item);
                    itemsSell.RemoveItem(item);
                }
                inv.SetContents(itemsInv);
                sellInv.SetContents(itemsSell);
            }
            else
            {
                if (add)
                {
                    itemsBuy.AddItem(item);
                }
                else
                {
                    itemsBuy.RemoveItem(item);
                }
                buyInv.SetContents(itemsBuy);
            }
        }

        public void ShortMsg(String text)
        {
            return;
        }

        public void RequestTrade()
        {
            BaseVobInst other = player.GetFocusVob();
            NPCInst npc = (NPCInst)other;
            if (npc != null)
            {
                NPCInst.Requests.SendRequest(player, npc);
                ShortMsg("Handelsanfrage an " + npc.CustomName);
                if(!requests.ContainsKey(npc.ID))
                {
                    NPCInst.Requests.SendRequest(player, npc);
                    ShortMsg("Handelsanfrage an " + npc.CustomName);
                    Log.Logger.Log("Erstelle Anfrage an ID= " +npc.ID.ToString());
                    requests.Add(npc.ID, new TradeRequest(DateTime.Now.Ticks, false)); // delay: + 300000000
                    
                }
                else if (requests[npc.ID].Received)
                {
                    Log.Logger.Log("Erhaltene Anfrage bestätigen!");
                    NPCInst.Requests.SendRequest(player, npc);
                }
                else if(requests[npc.ID].Time < DateTime.Now.Ticks)
                {
                    NPCInst.Requests.SendRequest(player, npc);
                    ShortMsg("Handelsanfrage an " + npc.CustomName);
                    requests[npc.ID].Time = DateTime.Now.Ticks + 0; // 30s
                    Log.Logger.Log("Eine abgelaufener Request wird erneuert");
                }
            }
        }

        class TradeRequest
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
                    Log.Logger.Log("setted request to " + value.ToString());
                    received = value;
                }
            }


            public TradeRequest(long time, bool received)
            {
                Time = time;
                Received = received;
                Log.Logger.Log("created request with " + received.ToString());
            }

        }

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
}
