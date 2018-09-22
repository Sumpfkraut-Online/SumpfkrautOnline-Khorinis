using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.GUI;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Menus;
using WinApi.User.Enumeration;

namespace GUC.Scripts.Arena
{
    /*
     *  TODO
     *  - bug fixen beim nachrichten splitten
     */
    class ChatMenu : GUCMenu
    {
        public static readonly ChatMenu Menu = new ChatMenu();

        public GUCVisual chatBackground;
        public GUCTextBox textBox;
        public GUCVisual prefix;
        private ChatMode openChatMode;
        ViewPoint screenSize;
        int chatHeigth, chatWidth;
        GUCTimer chatInactivityTimer;

        public ChatMenu()
        {
            screenSize = GUCView.GetScreenSize();
            chatHeigth = screenSize.Y / 5;
            chatWidth = screenSize.X - 350;

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

            chatInactivityTimer = new GUCTimer();
            chatInactivityTimer.SetCallback(() => { if (!textBox.Enabled) chatBackground.Hide(); chatInactivityTimer.Stop(); });
            chatInactivityTimer.SetInterval(6 * TimeSpan.TicksPerSecond);
        }

        public void ToggleBackground()
        {
            if (chatBackground.Shown)
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
            openChatMode = ChatMode.All;
            prefix.Texts[0].Text = "All: ";
            Open();
        }

        public void OpenTeamChat()
        {
            //if (TeamMode.TeamDef == null)
            {
                //AddMessage(ChatMode.All, "Du musst erst einem Team beitreten bevor du den Teamchat verwenden kannst!");
                // OpenAllChat();
                return;
            }
            openChatMode = ChatMode.Team;
            prefix.Texts[0].Text = "Team: ";
            Open();
        }

        protected override void KeyPress(VirtualKeys key, bool hold)
        {
            switch (key)
            {
                case VirtualKeys.Escape:
                    if (!hold)
                        Close();
                    break;
                case VirtualKeys.Delete:
                    if (!hold)
                    {
                        if (InputHandler.IsPressed(VirtualKeys.Control))
                            ClearChat();
                        else
                            textBox.Input = "";
                    }
                    break;
                case VirtualKeys.Return:
                    if (!hold)
                    {
                        if (!(textBox.Input.Length == 0))
                            SendInput();
                        if (!InputHandler.IsPressed(VirtualKeys.Shift))
                            Close();
                    }
                    break;
                default:
                    textBox.KeyPressed(key);
                    break;
            }
            base.KeyPress(key, hold);
        }

        public void SendInput()
        {
            string message = textBox.Input.Trim();
            if (message.Length == 0)
                return;

            if (message == "/detectschinken")
            {
                ArenaClient.DetectSchinken = true;
                textBox.Input = "";
                return;
            }
            else if (message.StartsWith("/tell"))
            {
                string name = message.Substring(6).Trim();
                var vob = GothicGlobals.Game.GetWorld().SearchVobByName(name);
                if (vob.Address != 0)
                {
                    Vec3f pos = (Vec3f)vob.Position;
                    Angles ang = Angles.FromMatrix(vob.TrafoObjToWorld);
                    string msg = string.Format(System.Globalization.CultureInfo.InvariantCulture, "new HordeBarrier() {{ Definition = \"barrier\", Position = new Vec3f({0:0}, {1:0}, {2:0}), Angles = new Angles({3:0.000}, {4:0.000}, {5:0.000}) }},\n", pos.X, pos.Y, pos.Z, ang.Pitch, ang.Yaw, ang.Roll);
                    System.IO.File.AppendAllText("barriers.txt", msg);
                }
                else
                {
                    Log.Logger.Log("not found");
                }
                return;
            }
            else if (message.StartsWith("/scale"))
            {
                string[] strs = message.Substring(7).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length <= 0 || !float.TryParse(strs[0], out float x))
                    x = 1;
                if (strs.Length <= 1 || !float.TryParse(strs[1], out float y))
                    y = 1;
                if (strs.Length <= 2 || !float.TryParse(strs[2], out float z))
                    z = 1;

                Log.Logger.Log(x + " " + y + " " + z);
                return;
            }

            switch (openChatMode)
            {
                case ChatMode.Team:
                    Chat.SendTeamMessage(message);
                    break;
                default:
                    Chat.SendMessage(message);
                    break;
            }
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
            Log.Logger.Log(chatmode);
            if (!this.textBox.Enabled)
            {
                chatBackground.Show();
                StartInactivityTimer();
            }

            // resort chat rows if necessary
            int maxScreenSize = chatWidth - 30;
            if (chatBackground.Texts[chatBackground.Texts.Count - 1].Text.Length > 0)
                for (int i = 0; i < chatBackground.Texts.Count - 1; i++)
                {
                    chatBackground.Texts[i].Text = chatBackground.Texts[i + 1].Text;
                    chatBackground.Texts[i].SetColor(chatBackground.Texts[i + 1].GetColor());
                }

            // split messages to multiple rows
            if (GUCView.StringPixelWidth(message) > maxScreenSize)
            {
                int charCounter = 0;
                string newMessage = "";
                foreach (char c in message)
                {
                    newMessage += c;
                    charCounter++;
                    if (!(GUCView.StringPixelWidth(newMessage) < maxScreenSize))
                    {
                        InsertMessage(openChatMode, newMessage);
                        // remains of the message
                        if (message.Length > charCounter)
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
        private void InsertMessage(ChatMode chatMode, string message)
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

            //if (chatMode == ChatMode.Team && TeamMode.TeamDef != null)
            //{
            //    chatBackground.Texts[index].SetColor(TeamMode.TeamDef.Color);
            //}
            if (chatMode == ChatMode.Private)
            {
                chatBackground.Texts[index].SetColor(ColorRGBA.Pink);
            }
            else
            {
                chatBackground.Texts[index].SetColor(ColorRGBA.White);
            }

            chatBackground.Texts[index].Text = message;
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
