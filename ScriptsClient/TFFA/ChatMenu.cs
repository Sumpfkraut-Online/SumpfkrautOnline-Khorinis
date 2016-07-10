using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus;
using WinApi.User.Enumeration;
using GUC.GUI;
using GUC.Types;

namespace GUC.Scripts.TFFA
{
    class ChatMenu : GUCMenu
    {
        public static readonly ChatMenu Menu = new ChatMenu();

        GUCVisual back;
        GUCVisualText typeText;
        GUCTextBox textBox;

        const int TextBoxOffset = 52;

        public bool TeamChat = false;


        Tuple<int, string, ColorRGBA>[] lines = new Tuple<int, string, ColorRGBA>[10];
        public void AddLine(ClientInfo ci, string text, bool onlyTeam)
        {
            Log.Logger.Log("AddLine: '" + text + "'");

            int index = 0;
            while (index < 10)
            {
                if (lines[index] == null)
                {
                    break;
                }
                index++;
            }

            if (index == 10)
            {
                // no free line
                for (int i = 0; i < 9; i++)
                {
                    lines[i] = lines[i + 1];
                }
                index = 9;
            }

            ColorRGBA color;
            if (ci.Team == Team.AL)
            {
                color = new ColorRGBA(255, 190, 190);
            }
            else if (ci.Team == Team.NL)
            {
                color = new ColorRGBA(190, 190, 255);
            }
            else
            {
                color = new ColorRGBA(255, 245, 190);
            }

            lines[index] = new Tuple<int, string, ColorRGBA>(ci.ID, ci.Name + (onlyTeam ? "(TEAM): " : ": ") + text, color);
            UpdateLines();
        }

        public void UpdateLines()
        {
            for (int i = 0; i < 10; i++)
            {
                var info = lines[i];
                if (info == null) return;

                var vis = back.Texts[i];
                vis.Text = InputControl.ClientsShown ? string.Format("({0}){1}", info.Item1, info.Item2) : info.Item2;
                vis.SetColor(info.Item3);
            }
        }

        public ChatMenu()
        {
            int[] res = GUCView.GetScreenSize();

            back = new GUCVisual();

            for (int i = 0; i < 10; i++)
            {
                back.CreateText(string.Empty, 0, 20 * i);
            }
            typeText = back.CreateText(string.Empty, 0, 205);
            textBox = new GUCTextBox(TextBoxOffset, 205, res[0] - TextBoxOffset, true);
            back.AddChild(textBox);
        }

        public void Show()
        {
            back.Show();
        }

        public void Hide()
        {
            back.Hide();
        }

        public override void Open()
        {
            base.Open();
            typeText.Text = TeamChat ? "Team:" : " Alle:";
            textBox.Enabled = true;
        }

        public override void Close()
        {
            base.Close();
            typeText.Text = "";
            textBox.Input = "";
            textBox.Enabled = false;
        }

        public override void KeyDown(VirtualKeys key, long now)
        {
            switch (key)
            {
                case VirtualKeys.Escape:
                    Close();
                    break;
                case VirtualKeys.Return:
                    SendMsg();
                    Close();
                    break;
                default:
                    textBox.KeyPressed(key);
                    break;
            }
        }

        public override void KeyUp(VirtualKeys key, long now)
        {
        }

        public override void Update(long now)
        {
            textBox.Update(now);
        }

        void SendMsg()
        {
            var msg = textBox.Input;
            if (string.IsNullOrWhiteSpace(msg))
                return;

            var stream = GUC.Network.GameClient.Client.GetMenuMsgStream();
            stream.Write((byte)(TeamChat ? MenuMsgID.TeamChat : MenuMsgID.AllChat));
            stream.Write(msg);
            GUC.Network.GameClient.Client.SendMenuMsg(stream, GUC.Network.PktPriority.LOW_PRIORITY, GUC.Network.PktReliability.RELIABLE);
        }
    }
}
