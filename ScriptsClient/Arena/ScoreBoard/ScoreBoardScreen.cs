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
    abstract class ScoreBoardScreen : GUCMenu
    {
        protected struct BoardEntry
        {
            public int ID;
            public int Score;
            public int Kills;
            public int Deaths;
            public int Ping;
        }

        protected void FillBoard(GUCVisual board, List<BoardEntry> list)
        {
            int index = 5; // column specifier offset
            foreach (BoardEntry entry in list.OrderByDescending(b => b.Score))
            {
                if (index >= board.Texts.Count)
                    return;

                string name = PlayerInfo.TryGetInfo(entry.ID, out PlayerInfo pi) ? pi.Name : "!Unknown Player!";
                SetText(board.Texts[index++], name, entry.ID);
                SetText(board.Texts[index++], entry.Score, entry.ID);
                SetText(board.Texts[index++], entry.Kills, entry.ID);
                SetText(board.Texts[index++], entry.Deaths, entry.ID);
                SetText(board.Texts[index++], entry.Ping, entry.ID);
            }

            for (; index < board.Texts.Count; index++)
                board.Texts[index].Text = string.Empty;
        }

        protected void SetText(GUCVisualText visText, object text, int playerID)
        {
            visText.Text = text.ToString();
            visText.Font = playerID == PlayerInfo.HeroID ? GUCView.Fonts.Default_Hi : GUCView.Fonts.Default;
        }

        const long MinOpenDuration = 750 * TimeSpan.TicksPerMillisecond;

        const string BackTex = "MENU_INGAME.TGA";

        protected const int Width = 400;
        protected const int yScreenDist = 200;

        const int xOffset = 10;
        const int yOffset = 10;
        const int NameWidth = 175;
        const int ScoreWidth = 70;
        const int KillsWidth = 45;
        const int DeathsWidth = 50;
        const int PingWidth = 45;

        GUCVisual titleVis;
        GUCTimer closeTimer;

        ScriptMessages msgID;

        public ScoreBoardScreen(ScriptMessages messageID, string title = "ScoreBoard")
        {
            this.msgID = messageID;
            this.closeTimer = new GUCTimer(DoClose);

            var screen = GUCView.GetScreenSize();
            int x = (screen.X - Width) / 2;
            int y = yScreenDist;

            titleVis = new GUCVisual(x, y - GUCView.FontsizeMenu, Width, GUCView.FontsizeMenu);
            titleVis.Font = GUCView.Fonts.Menu;
            titleVis.CreateTextCenterX(title, 0);

        }

        protected GUCVisual CreateBoard()
        {
            var screen = GUCView.GetScreenSize();
            int height = screen.Y - yScreenDist * 2;

            var vis = new GUCVisual(0, 0, Width, height);
            vis.SetBackTexture(BackTex);

            int x = xOffset; int y = yOffset;
            vis.CreateText("Name", x, y); x += NameWidth;
            vis.CreateText("Punkte", x, y); x += ScoreWidth;
            vis.CreateText("Kills", x, y); x += KillsWidth;
            vis.CreateText("Tode", x, y); x += DeathsWidth;
            vis.CreateText("Ping", x, y);

            int bottom = y + height - GUCView.FontsizeDefault;
            y += 5;

            while (y < bottom)
            {
                GUCVisualText t;
                x = xOffset;
                y += GUCView.FontsizeDefault;

                vis.CreateText("", x, y); x += NameWidth;
                t = vis.CreateText("", x + ScoreWidth / 2, y); x += ScoreWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = vis.CreateText("", x + KillsWidth / 2, y); x += KillsWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = vis.CreateText("", x + DeathsWidth / 2, y); x += DeathsWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = vis.CreateText("", x + PingWidth / 2, y); x += PingWidth; t.Format = GUCVisualText.TextFormat.Center;
            }

            return vis;
        }

        bool shown = false;
        public bool Shown { get { return shown; } }
        public override void Open()
        {
            Log.Logger.Log("Open");
            if (shown)
                return;

            ShowBoard();
            titleVis.Show();
            SendToggleMessage();
            openTime = GameTime.Ticks;
            closeTimer.Stop();
            shown = true;
        }

        protected abstract void ShowBoard();

        long openTime;
        public override void Close()
        {
            if (!shown)
                return;

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

        protected abstract void HideBoard();
        void DoClose()
        {
            HideBoard();
            titleVis.Hide();
            SendToggleMessage();
            closeTimer.Stop();
            shown = false;
        }

        void SendToggleMessage()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)msgID);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public abstract void ReadMessage(PacketReader stream);
    }
}