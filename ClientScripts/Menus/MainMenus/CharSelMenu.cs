using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.Network.Messages;
using GUC.Network;

namespace GUC.Client.Menus.MainMenus
{
    /*class CharSelMenu : GUCMainMenu
    {
        public CharSelMenu()
        {
            AccountMessage.OnLogin = ReceiveCharList;
        }

        CharListHandle ListHandle;

        protected override void OnCreate()
        {
            Back.SetBackTexture("Menu_SaveLoad_Back.tga");
            Back.CreateText("Charakterauswahl", 80, 15);

            MainMenuCharacter Character = AddCharacter(378, 8, 277, 208);
            ListHandle = AddCharList(40, 60, AccCharInfo.Max_Slots, Character, SelectCharacter, SelectEmptySlot);

            AddButton("Ausloggen", "Aus dem eingeloggten Account ausloggen.", 150, 425, GUCMenus.Main.Open);
            OnEscape = GUCMenus.Main.Open;
        }

        void ReceiveCharList(AccCharInfo[] chars)
        {
            Open();
            ListHandle.Fill(chars);
            items[cursor].Select();
            UpdateHelpText();
        }

        void SelectCharacter()
        {
            AccountMessage.StartInWorld(ListHandle.GetSlotNum(items[cursor]));
        }

        void SelectEmptySlot()
        {
            GUCMenus.CharCreation.Open(ListHandle.GetSlotNum(items[cursor]));
        }
    }*/
}
