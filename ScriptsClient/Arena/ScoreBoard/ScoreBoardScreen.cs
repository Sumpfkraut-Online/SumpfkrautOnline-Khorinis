using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Network;
using WinApi.User.Enumeration;

namespace GUC.Scripts.Arena
{
    class ScoreBoardScreen : GUCMenu
    {
        const string BackTex = "MENU_INGAME.TGA";
        const int Width = 400;

        const int xOffset = 10;
        const int yOffset = 10;
        const int NameWidth = 175;
        const int ScoreWidth = 70;
        const int KillsWidth = 45;
        const int DeathsWidth = 50;
        const int PingWidth = 45;

        GUCVisual back;

        ScriptMessages msgID;

        public ScoreBoardScreen(ScriptMessages messageID)
        {
            this.msgID = messageID;

            const int yScreenDist = 200;

            int[] screen = GUCView.GetScreenSize();
            int bottom = screen[1] - yScreenDist * 2;
            back = new GUCVisual((screen[0] - Width) / 2, yScreenDist, Width, bottom);
            back.SetBackTexture(BackTex);

            bottom -= (yOffset + GUCView.FontsizeDefault);
            int y = yOffset;
            while (y < bottom)
            {
                int x = xOffset;
                back.CreateText("Name", x, y); x += NameWidth;
                back.CreateText("Punkte", x, y); x += ScoreWidth;
                back.CreateText("Kills", x, y); x += KillsWidth;
                back.CreateText("Tode", x, y); x += DeathsWidth;
                back.CreateText("Ping", x, y);

                y += GUCView.FontsizeDefault;
                if (back.Texts.Count == 5)
                    y += 5;
            }
        }

        public override void Open()
        {
            back.Show();
            SendToggleMessage();
        }

        public override void Close()
        {
            back.Hide();
            SendToggleMessage();
        }

        void SendToggleMessage()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ScoreDuelMessage);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public void ReadMessage(PacketReader stream)
        {
            int count = stream.ReadByte();
            var info = new ScoreBoardItem();
            for (int i = 1; i <= count; i++)
            {
                if (i >= back.Texts.Count / 5)
                    return;

                info.Read(stream);
                back.Texts[5 * i].Text = info.Name;
                back.Texts[5 * i + 1].Text = info.Score.ToString();
                back.Texts[5 * i + 2].Text = info.Kills.ToString();
                back.Texts[5 * i + 3].Text = info.Deaths.ToString();
                back.Texts[5 * i + 4].Text = info.Ping < 1000 ? info.Score.ToString() : "999";
            }

            for (int i = 5 * (count + 1); i < back.Texts.Count; i++)
                back.Texts[i].Text = "";
        }
    }
}