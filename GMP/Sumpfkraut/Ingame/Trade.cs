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
    class Trade : IMessage, GUCMInputReceiver
    {
        GUCMenuInventory inv;
        GUCMenuInventory sellInv;
        GUCMenuInventory buyInv;

        Player trader;

        public Trade()
        {
            buyInv = new GUCMenuInventory(80, 200, 2, 3, "Inv_Back_Buy.tga");
            sellInv = new GUCMenuInventory(290, 200, 2, 3, "Inv_Back_Sell.tga");
            inv = new GUCMenuInventory(500, 200, 4, 3, "Inv_Back.tga");

            inv.left = sellInv;
            sellInv.right = inv;
            sellInv.left = buyInv;
            buyInv.right = sellInv;

            inv.OnPressCTRL += CTRLPressedInventory;
            sellInv.OnPressCTRL += CTRLPressedSell;
            
            IngameInput.menus.Add(this);
        }

        private void CTRLPressedInventory(Item item)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.OfferItem);
            stream.Write(Player.Hero.ID);
            stream.Write(item.ID);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            inv.RemoveItem(item);
            sellInv.AddItem(item);
        }

        private void CTRLPressedSell(Item item)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.RemoveItem);
            stream.Write(Player.Hero.ID);
            stream.Write(item.ID);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            sellInv.RemoveItem(item);
            inv.AddItem(item);
        }

        public void KeyPressed(int key)
        {
            if (key == (int)VirtualKeys.T)
            {
                if (trader == null) //trade menu is not shown
                {
                    if (Player.Hero.FocusVob != null && Player.Hero.FocusVob.VobType == VobType.Player)
                    {
                       SendRequest((Player)sWorld.VobDict[Player.Hero.FocusVob.ID]);
                    }
                }
                else
                {
                    CloseTradeMenu();
                }
            }
            else if (key == (int)VirtualKeys.Escape)
            {
                if (trader != null)
                {
                    CloseTradeMenu();
                }
            }
            else if (trader != null)
            {
                if (inv.enabled)
                {
                    inv.KeyPressed(key);
                    return;
                }
                else if (sellInv.enabled)
                {
                    sellInv.KeyPressed(key);
                    return;
                }
                else if (buyInv.enabled)
                {
                    buyInv.KeyPressed(key);
                    return;
                }
            }
        }

        private void OpenTradeMenu(Player pl)
        {
            trader = pl;
            inv.SetCursor(0, 0);
            inv.Open(Player.Hero.ItemList);
            sellInv.Open(null);
            buyInv.Open(null);
            IngameInput.activateFullControl(this);
        }

        private void CloseTradeMenu()
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Break);
            stream.Write(Player.Hero.ID);
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            trader = null;
            sellInv.Hide();
            buyInv.Hide();
            inv.Hide();
            IngameInput.deactivateFullControl();
        }

        public void Update(long ticks)
        {
        }

        private void SendRequest(Player pl)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TradeMessage);
            stream.Write((byte)TradeStatus.Request);

            stream.Write(Player.Hero.ID);
            stream.Write(pl.ID);

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

                OpenTradeMenu((Player)sWorld.VobDict[traderID]);
            }
            else if (status == TradeStatus.Break)
            {
                trader = null;
                sellInv.Hide();
                buyInv.Hide();
                inv.Hide();
                IngameInput.deactivateFullControl();
            }
            else if (status == TradeStatus.OfferItem)
            {
                int itemID;
                stream.Read(out itemID);
                buyInv.AddItem((Item)sWorld.VobDict[itemID]);
            }
            else if (status == TradeStatus.RemoveItem)
            {
                int itemID;
                stream.Read(out itemID);
                buyInv.RemoveItem((Item)sWorld.VobDict[itemID]);
            }
        }
    }
}
