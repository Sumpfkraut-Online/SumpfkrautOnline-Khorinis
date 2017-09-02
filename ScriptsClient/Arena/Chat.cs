using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.GUI;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Menus;
using WinApi.User.Enumeration;
using GUC.Scripts.Sumpfkraut.Networking;


/*
 * Todo: 
 * Nachrichten im Display sortieren
 * Keine leeren nachrichten abschicken
 * eine löschen taste um den gesammten chat zu löschen
 */
namespace GUC.Scripts.Arena
{
    class Chat : GUCMenu
    {
        public enum ChatMode
        {
            All,
            Team,
        }
        public static Chat ChatMenu;
        public GUCVisual chatBackground;
        public Dictionary<string, ColorRGBA> playerNameColors;
        public GUCTextBox textBox;
        public GUCVisual prefix;
        private ChatMode chatMode;
        int[] screenSize;

        public Chat()
        {
            screenSize = GUCView.GetScreenSize();
            chatBackground = new GUCVisual(0, 0, screenSize[0], screenSize[1] / 5 + 5);
            chatBackground.SetBackTexture("Menu_Ingame.tga");
            chatBackground.Show();

            int space = 20;
            int lines = (screenSize[1] / 5) / space;
            for (int i = 0; i < lines; i++)
            {
                chatBackground.CreateText("" + i, 20, 5 + i * space);
                chatBackground.Texts[i].Text = "";
            }

            textBox = new GUCTextBox(70, screenSize[1] / 5 + 5, screenSize[0] - 70, false);
            prefix = new GUCVisual(15, screenSize[1] / 5 + 5, screenSize[0], 20);
            prefix.CreateText("", 0, 0);
            
        }

        public void ToggleBackground()
        {
            if( chatBackground.Shown )
            {
                chatBackground.Hide();
            }
            else
            {
                chatBackground.Show();
            }
        }

        public override void Open()
        {
            textBox.Enabled = true;
            textBox.Show();
            prefix.Show();
            base.Open();
        }

        public override void Close()
        {
            textBox.Enabled = false;
            textBox.Hide();
            prefix.Hide();
            base.Close();
        }

        public void OpenAllChat()
        {
            chatMode = ChatMode.All;
            prefix.Texts[0].Text = "All: ";
            Open();
        }

        public void OpenTeamChat()
        {
            chatMode = ChatMode.Team;
            prefix.Texts[0].Text = "Team: ";
            Open();
        }

        protected override void KeyDown(VirtualKeys key)
        {
            switch(key)
            {
                case VirtualKeys.Escape:
                    Close();
                    break;
                case VirtualKeys.Return:
                    SendInput();
                    if (!InputHandler.IsPressed(VirtualKeys.Shift))
                        Close();
                    break;
                default:
                    textBox.KeyPressed(key);
                    break;
            }
            base.KeyDown(key);
        }

        public void SendInput()
        {
            string playerName = ScriptClient.Client.Character.CustomName;
            ArenaClient.SendChatMessage(chatMode, playerName + ": " + textBox.Input);
            textBox.Input = "";
        }

        public void ReceiveServerMessage(ChatMode chatmode, string message)
        {
            if(chatmode == ChatMode.Team)
            {
                message = "(Team) " + message;
            }
            AddMessage(message);
        }

        public void AddMessage(string message)
        {
            int maxScreenSize = screenSize[0] - 100;
            for (int i = 0; i < chatBackground.Texts.Count - 1; i++)
            {
                chatBackground.Texts[i].Text = chatBackground.Texts[i + 1].Text;
            }
            // for multiple lines
            if(GUCView.StringPixelWidth(message) > maxScreenSize)
            {
                int charCounter = 0;
                string newMessage = "";
                foreach( char c in message)
                {
                    newMessage += c;
                    charCounter++;
                    if(! (GUCView.StringPixelWidth(newMessage)<maxScreenSize) )
                    {
                        chatBackground.Texts[chatBackground.Texts.Count - 1].Text = newMessage;
                        AddMessage(message.Substring(charCounter + 1));
                        return;
                    }
                }
            }
            chatBackground.Texts[chatBackground.Texts.Count - 1].Text = message;
        }
    }
}
