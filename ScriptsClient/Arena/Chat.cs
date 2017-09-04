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
        public static readonly Chat ChatMenu = new Chat();
        public GUCVisual chatBackground;
        public GUCTextBox textBox;
        public GUCVisual prefix;
        private ChatMode chatMode;
        int[] screenSize;
        int chatHeigth, chatWidth;
        Scripting.GUCTimer chatInactivityTimer;

        static ColorRGBA white = new ColorRGBA(255, 255, 255);
        static ColorRGBA red = new ColorRGBA(255, 0, 0);

        public Chat()
        {
            screenSize = GUCView.GetScreenSize();
            chatHeigth = screenSize[1] / 5;
            chatWidth = screenSize[0] - 350;

            chatBackground = new GUCVisual(0, 0, chatWidth, chatHeigth + 5);
            chatBackground.SetBackTexture("Dlg_Conversation.tga");

            const int space = 20;
            int lines = chatHeigth / space;
            for (int i = 0; i < lines; i++)
            {
                chatBackground.CreateText("" + i, 20, 5 + i * space);
                chatBackground.Texts[i].Text = "";
            }

            textBox = new GUCTextBox(70, chatHeigth + 5, chatWidth - 90, false);
            prefix = new GUCVisual(15, chatHeigth + 5, chatWidth, 20);
            prefix.CreateText("", 0, 0);

            chatInactivityTimer = new Scripting.GUCTimer();
            chatInactivityTimer.SetCallback(() => { if(!textBox.Enabled) chatBackground.Hide(); chatInactivityTimer.Stop(); });
            chatInactivityTimer.SetInterval(8 * TimeSpan.TicksPerSecond);
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
            ArenaClient.SendChatMessage(chatMode, textBox.Input);
            textBox.Input = "";
        }

        public void ReceiveServerMessage(ChatMode chatmode, string message)
        {
            AddMessage(chatmode, message);
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
        public void AddMessage(ChatMode chatmode, string message)
        {
            // resort chat rows if necessary
            int maxScreenSize = chatWidth - 30;
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
                        InsertMessage(chatMode, newMessage);
                        // remains of the message
                        if(message.Length > charCounter)
                            AddMessage(chatmode, message.Substring(charCounter));
                        return;
                    }
                }
            }

            // set message in the correct place
            InsertMessage(chatmode, message);
        }

        /// <summary>
        /// Makes sure messages is added to the correct row in the chat.
        /// </summary>
        /// <param name="message"></param>
        private void InsertMessage(ChatMode chatmode,string message)
        {
            int index = 0;
            while (index < chatBackground.Texts.Count - 1)
            {
                if (chatBackground.Texts[index].Text.Length == 0)
                {
                    chatBackground.Texts[index].Text = message;
                    break;
                }
                index++;
            }
            if (chatMode == ChatMode.Team)
                chatBackground.Texts[index].SetColor(red);
            else
                chatBackground.Texts[index].SetColor(white);
            chatBackground.Texts[index].Text = message;
            //chatBackground.Texts[chatBackground.Texts.Count - 1].Text = message;
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
