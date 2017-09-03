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
 *  TODO
 *  bug fixen beim nachrichten splitten
 *  nachrichten nur für teams anzeigen
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
        public static readonly Chat ChatMenu = new Chat();
        public GUCVisual chatBackground;
        public Dictionary<string, ColorRGBA> playerNameColors;
        public GUCTextBox textBox;
        public GUCVisual prefix;
        private ChatMode chatMode;
        int[] screenSize;
        Scripting.GUCTimer chatInactivityTimer;

        public Chat()
        {
            screenSize = GUCView.GetScreenSize();
            chatBackground = new GUCVisual(0, 0, screenSize[0], screenSize[1] / 5 + 5);
            chatBackground.SetBackTexture("Menu_Ingame.tga");

            int space = 20;
            int lines = (screenSize[1] / 5) / space;
            for (int i = 0; i < lines; i++)
            {
                chatBackground.CreateText("" + i, 20, 5 + i * space);
                chatBackground.Texts[i].Text = "";
            }

            textBox = new GUCTextBox(70, screenSize[1] / 5 + 5, screenSize[0] - 90, false);
            prefix = new GUCVisual(15, screenSize[1] / 5 + 5, screenSize[0], 20);
            prefix.CreateText("", 0, 0);

            chatInactivityTimer = new Scripting.GUCTimer();
            chatInactivityTimer.SetCallback(() => { if(!textBox.Enabled) chatBackground.Hide(); chatInactivityTimer.Stop(); });
            chatInactivityTimer.SetInterval(5 * TimeSpan.TicksPerSecond);
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
            if (!chatBackground.Shown)
                chatBackground.Show();
            base.Open();
        }

        public override void Close()
        {
            textBox.Enabled = false;
            textBox.Hide();
            prefix.Hide();
            StartInactivityTimer();
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
                case VirtualKeys.Delete:
                    if (InputHandler.IsPressed(VirtualKeys.Control))
                        ClearChat();
                    else
                        textBox.Input = "";
                    break;
                case VirtualKeys.Return:
                    if (!(textBox.Input.Length == 0))
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
            StartInactivityTimer();
        }

        public void StartInactivityTimer()
        {
            if (chatInactivityTimer.Started)
                chatInactivityTimer.Restart();
            else
                chatInactivityTimer.Start();
        }

        /// <summary>
        /// Shifts chat rows to create space for the new message and controls message length
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            // resort chat rows if necessary
            int maxScreenSize = screenSize[0] - 100;
            if(chatBackground.Texts[chatBackground.Texts.Count - 1].Text.Length > 0)
                for (int i = 0; i < chatBackground.Texts.Count - 1; i++)
                {
                    chatBackground.Texts[i].Text = chatBackground.Texts[i + 1].Text;
                }

            // split messages to multiple rows
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
                        InsertMessage(newMessage);
                        AddMessage(message.Substring(charCounter + 1));
                        return;
                    }
                }
            }

            // set message in the correct place
            InsertMessage(message);
        }

        /// <summary>
        /// Makes sure messages is added to the correct row in the chat.
        /// </summary>
        /// <param name="message"></param>
        private void InsertMessage(string message)
        {
            int index = 0;
            while (index < chatBackground.Texts.Count - 1)
            {
                if (chatBackground.Texts[index].Text.Length == 0)
                {
                    chatBackground.Texts[index].Text = message;
                    return;
                }
                index++;
            }
            chatBackground.Texts[chatBackground.Texts.Count - 1].Text = message;
        }

        public void ClearChat()
        {
            for (int i = 0; i < chatBackground.Texts.Count; i++)
            {
                chatBackground.Texts[i].Text = "";
            }
        }
    }
}
