using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Client.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Network;
using GUC.Scripts.TFFA;

namespace GUC.Client.Scripts.TFFA
{
    class NameMenu : GUCMainMenu
    {
        public static readonly NameMenu Menu = new NameMenu();

        MainMenuTextBox box;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Namen eingeben", 70);
            box = AddTextBox("Name:", "", 320, 240, 200, 100, Close);
            OnEscape = MainMenu.Menu.Open;
        }

        public override void Close()
        {
            PacketWriter stream = GameClient.Client.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.SetName);
            stream.Write(box.Input);
            GameClient.Client.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
            base.Close();
        }
    }
}
