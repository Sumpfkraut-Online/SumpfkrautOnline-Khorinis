using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Network;
using GUC.Scripting;
using GUC.Types;

namespace GUC.Scripts.Arena
{
    class ScoreBoardScreen : GUCMenu
    {
        static readonly ColorRGBA PlayerColor = new ColorRGBA(200, 200, 200, 200);
        static readonly ColorRGBA HeroColor = new ColorRGBA(255, 255, 255, 255);

        const long MinOpenDuration = 750 * TimeSpan.TicksPerMillisecond;

        const string BackTex = "MENU_INGAME.TGA";
        const int Width = 400;

        const int yScreenDist = 200;

        const int xOffset = 10;
        const int yOffset = 10;
        const int NameWidth = 175;
        const int ScoreWidth = 70;
        const int KillsWidth = 45;
        const int DeathsWidth = 50;
        const int PingWidth = 45;

        GUCVisual titleVis;
        GUCVisual back;
        GUCTimer closeTimer;

        ScriptMessages msgID;

        public ScoreBoardScreen(ScriptMessages messageID, string title = "ScoreBoard")
        {
            this.msgID = messageID;
            this.closeTimer = new GUCTimer(DoClose);

            var screen = GUCView.GetScreenSize();
            int x = (screen.Width - Width) / 2;
            int y = yScreenDist;
            int height = screen.Height - yScreenDist * 2;

            titleVis = new GUCVisual(x, y - GUCView.FontsizeMenu, Width, GUCView.FontsizeMenu);
            titleVis.Font = GUCView.Fonts.Menu;
            titleVis.CreateTextCenterX(title, y);
            
            back = new GUCVisual(x, y, Width, height);
            back.SetBackTexture(BackTex);
            
            x = xOffset; y = yOffset;
            back.CreateText("Name", x, y); x += NameWidth;
            back.CreateText("Punkte", x, y); x += ScoreWidth;
            back.CreateText("Kills", x, y); x += KillsWidth;
            back.CreateText("Tode", x, y); x += DeathsWidth;
            back.CreateText("Ping", x, y);

            int bottom = y + height - GUCView.FontsizeDefault;
            y += 5;

            while (y < bottom)
            {
                GUCVisualText t;
                x = xOffset;
                y += GUCView.FontsizeDefault;

                back.CreateText("", x, y); x += NameWidth;
                t = back.CreateText("", x + ScoreWidth / 2, y); x += ScoreWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = back.CreateText("", x + KillsWidth / 2, y); x += KillsWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = back.CreateText("", x + DeathsWidth / 2, y); x += DeathsWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = back.CreateText("", x + PingWidth / 2, y); x += PingWidth; t.Format = GUCVisualText.TextFormat.Center;
            }
        }

        public override void Open()
        {
            back.Show();
            titleVis.Show();
            SendToggleMessage();
            openTime = GameTime.Ticks;
            closeTimer.Stop();
        }

        long openTime;
        public override void Close()
        {
            long diff = GameTime.Ticks - openTime;
            if (diff > MinOpenDuration)
            {
                DoClose();
            }
            else
            {
                closeTimer.SetInterval(MinOpenDuration - diff);
                closeTimer.Start();
            }
        }

        void DoClose()
        {
            titleVis.Hide();
            back.Hide();
            SendToggleMessage();
            closeTimer.Stop();
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
                if (!PlayerInfo.TryGetInfo(info.ID, out PlayerInfo pi))
                    continue;

                SetText(5 * i, pi.Name, pi.ID);
                SetText(5 * i + 1, info.Score, pi.ID);
                SetText(5 * i +2, info.Kills, pi.ID);
                SetText(5 * i + 3, info.Deaths, pi.ID);
                SetText(5 * i + 4, info.Ping < 1000 ? info.Score : 999, pi.ID);
            }

            for (int i = 5 * (count + 1); i < back.Texts.Count; i++)
                back.Texts[i].Text = "";
        }

        void SetText(int index, object text, int playerID)
        {
            GUCVisualText t = back.Texts[index];
            t.Text = text.ToString();
            t.SetColor(playerID == PlayerInfo.HeroID ? HeroColor : PlayerColor);
        }
    }
}