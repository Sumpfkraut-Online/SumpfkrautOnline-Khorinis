using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Types;

namespace GUC.Scripts.Arena
{
    class ScoreBoard : GUCView
    {
        public struct Entry
        {
            public int ID;
            public int Score;
            public int Kills;
            public int Deaths;
            public int Ping;
        }

        const string BackTex = "MENU_INGAME.TGA";

        public const int Width = 400;
        public const int YDistance = 200;

        const int xOffset = 10;
        const int yOffset = 10;
        const int NameWidth = 175;
        const int ScoreWidth = 70;
        const int KillsWidth = 45;
        const int DeathsWidth = 50;
        const int PingWidth = 45;

        GUCVisualText titleText;
        GUCVisual titleVis;
        GUCVisual vis;

        public ScoreBoard()
        {
            var screen = GetScreenSize();
            int height = screen.Y - YDistance * 2;

            titleVis = new GUCVisual(0, 0, Width, height - FontsizeMenu);
            titleText = titleVis.CreateTextCenterX("", 0);
            titleText.Font = Fonts.Menu;

            vis = new GUCVisual(0, FontsizeMenu, Width, height);
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
                y += FontsizeDefault;

                vis.CreateText("", x, y); x += NameWidth;
                t = vis.CreateText("", x + ScoreWidth / 2, y); x += ScoreWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = vis.CreateText("", x + KillsWidth / 2, y); x += KillsWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = vis.CreateText("", x + DeathsWidth / 2, y); x += DeathsWidth; t.Format = GUCVisualText.TextFormat.Center;
                t = vis.CreateText("", x + PingWidth / 2, y); x += PingWidth; t.Format = GUCVisualText.TextFormat.Center;
            }

            titleVis.AddChild(vis);
        }

        public void SetTitle(string text)
        {
            titleText.Text = text;
        }

        public void SetTitle(string text, ColorRGBA color)
        {
            titleText.Text = text;
            titleText.SetColor(color);
        }

        public override void Hide()
        {
            titleVis.Hide();
        }

        public override void Show()
        {
            titleVis.Show();
        }

        public void Fill(List<Entry> list)
        {
            int index = 5; // column specifier offset
            foreach (Entry entry in list.OrderByDescending(b => b.Score))
            {
                if (index >= vis.Texts.Count)
                    return;

                string name = PlayerInfo.TryGetInfo(entry.ID, out PlayerInfo pi) ? pi.Name : "!Unknown Player!";
                SetText(vis.Texts[index++], name, entry.ID);
                SetText(vis.Texts[index++], entry.Score, entry.ID);
                SetText(vis.Texts[index++], entry.Kills, entry.ID);
                SetText(vis.Texts[index++], entry.Deaths, entry.ID);
                SetText(vis.Texts[index++], entry.Ping, entry.ID);
            }

            for (; index < vis.Texts.Count; index++)
                vis.Texts[index].Text = string.Empty;
        }

        void SetText(GUCVisualText visText, object text, int playerID)
        {
            visText.Text = text.ToString();
            visText.Font = playerID == PlayerInfo.HeroInfo.ID ? GUCView.Fonts.Default_Hi : GUCView.Fonts.Default;
        }

        public void SetPos(int x, int y)
        {
            titleVis.SetPos(x, y);
        }
    }
}
