using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Network;

namespace GUC.Scripts.TFFA
{
    class NameMenu : GUCMainMenu
    {
        public static readonly NameMenu Menu = new NameMenu();

        MainMenuTextBox box;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Namen eingeben", 70);
            box = AddTextBox("Name:", "", 320, 240, 240, 100, MainMenu.Menu.Open);
            OnEscape = MainMenu.Menu.Open;
        }

        public override void Close()
        {
            string newName = box.Input;
            if (!string.IsNullOrWhiteSpace(newName) && newName != TFFAClient.Info.Name)
            {
                PacketWriter stream = GameClient.Client.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.ClientName);
                stream.Write(newName);
                GameClient.Client.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
            }
            base.Close();
        }
    }
}
