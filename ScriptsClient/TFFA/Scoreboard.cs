using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Network;
using GUC.Scripting;
using GUC.Types;

namespace GUC.Scripts.TFFA
{
    class Scoreboard
    {
        const int BoardWidth = 800;
        const int BoardHeight = 600;
        const int TimerOffset = -20;

        static readonly ColorRGBA LineColor = new ColorRGBA(200, 200, 200);
        static readonly ColorRGBA PlayerColor = new ColorRGBA(250, 255, 250);

        class BoardLine
        {
            const int PingWidth = 55;
            const int DamageWidth = 50;
            const int DeathsWidth = 30;
            const int KillsWidth = 40;
            const int NameWidth = (BoardWidth / 2) - xOffset - PingWidth - DamageWidth - DeathsWidth - KillsWidth;

            public GUCVisualText name, kills, deaths, damage, ping;

            public ClientInfo Info { get; private set; }

            public BoardLine(GUCVisual vis, int x, int y, bool white = false)
            {
                name = vis.CreateText("", x, y);
                kills = vis.CreateText("", x + NameWidth, y);
                deaths = vis.CreateText("", x + NameWidth + KillsWidth, y);
                damage = vis.CreateText("", x + NameWidth + KillsWidth + DeathsWidth, y);
                ping = vis.CreateText("", x + NameWidth + KillsWidth + DeathsWidth + DamageWidth, y);

                if (!white)
                    for (int i = vis.Texts.Count - 5; i < vis.Texts.Count; i++)
                        vis.Texts[i].SetColor(LineColor);
            }

            public void Clear()
            {
                name.Text = "";
                kills.Text = "";
                deaths.Text = "";
                damage.Text = "";
                ping.Text = "";
            }

            public void SetInfo(ClientInfo info)
            {
                if (info == null)
                    return;

                this.Info = info;

                SetName(info);
                kills.Text = info.Kills.ToString();
                deaths.Text = info.Deaths.ToString();
                damage.Text = info.Damage.ToString();
                ping.Text = info.Ping.ToString();

                if (info == TFFAClient.Info)
                {
                    name.SetColor(PlayerColor);
                    kills.SetColor(PlayerColor);
                    deaths.SetColor(PlayerColor);
                    damage.SetColor(PlayerColor);
                    ping.SetColor(PlayerColor);
                }
                else
                {
                    name.SetColor(LineColor);
                    kills.SetColor(LineColor);
                    deaths.SetColor(LineColor);
                    damage.SetColor(LineColor);
                    ping.SetColor(LineColor);
                }
            }

            public void SetName(ClientInfo info)
            {
                string pn = InputControl.ClientsShown ? string.Format("({0}){1}", info.ID, info.Name) : info.Name;
                while (GUCView.StringPixelWidth(pn) > NameWidth)
                    pn = pn.Remove(pn.Length - 1);

                this.name.Text = pn;
            }
        }


        public static readonly Scoreboard Menu = new Scoreboard();

        GUCVisual back;
        List<BoardLine> alPlayers = new List<BoardLine>();
        List<BoardLine> nlPlayers = new List<BoardLine>();
        GUCVisual timer;
        GUCVisual specView;

        const int xOffset = 15, yOffset = 10, yDist = 20;

        public void UpdateStats()
        {
            int alCounter = 1, nlCounter = 1;
            
            foreach (ClientInfo ci in ClientInfo.ClientInfos.Values.OrderByDescending(c => c.Kills))
            {
                BoardLine line;
                if (ci.Team == Team.AL)
                {
                    if (alCounter >= alPlayers.Count)
                    {
                        line = new BoardLine(this.back, xOffset, yOffset + yDist * alCounter);
                        alPlayers.Add(line);
                    }
                    else
                    {
                        line = alPlayers[alCounter];
                    }
                    alCounter++;
                }
                else if (ci.Team == Team.NL)
                {
                    if (nlCounter >= nlPlayers.Count)
                    {
                        line = new BoardLine(this.back, BoardWidth / 2 + xOffset, yOffset + yDist * nlCounter);
                        nlPlayers.Add(line);
                    }
                    else
                    {
                        line = nlPlayers[nlCounter];
                    }
                    nlCounter++;
                }
                else
                {
                    continue;
                }
                line.SetInfo(ci);
            }

            for (int i = alCounter; i < alPlayers.Count; i++)
                alPlayers[i].Clear();
            for (int i = nlCounter; i < nlPlayers.Count; i++)
                nlPlayers[i].Clear();

            UpdateSpectators();
        }

        public void UpdateStats(int alKills, int nlKills)
        {
            timer.Texts[1].Text = alKills.ToString();
            timer.Texts[2].Text = nlKills.ToString();

            UpdateStats();
        }

        public void UpdateNames()
        {
            for (int i = 1; i < alPlayers.Count; i++)
            {
                alPlayers[i].SetName(alPlayers[i].Info);
            }
            for (int i = 1; i < nlPlayers.Count; i++)
            {
                nlPlayers[i].SetName(nlPlayers[i].Info);
            }
            UpdateSpectators();
        }

        void UpdateSpectators()
        {
            specView.Texts[0].Text = "";
            foreach (ClientInfo ci in ClientInfo.ClientInfos.Values.OrderBy(c => c.ID))
            {
                if (ci.Team == Team.Spec)
                {
                    specView.Texts[0].Text += (InputControl.ClientsShown ? string.Format("({0}){1}, ", ci.ID, ci.Name) : (ci.Name + ", "));
                }
            }
            if (specView.Texts[0].Text != "")
                specView.Texts[0].Text = "Zuschauer: " + specView.Texts[0].Text;
        }

        public Scoreboard()
        {
            int[] res = GUCView.GetScreenSize();
            back = new GUCVisual((res[0] - BoardWidth) / 2, (res[1] - BoardHeight) / 2, BoardWidth, BoardHeight);
            back.SetBackTexture("scoreboard.tga");
            closeTimer = new GUCTimer(Close);

            timer = new GUCVisual();
            timer.CreateTextCenterX("Timer", (res[1] - BoardHeight) / 2 + TimerOffset);
            timer.Font = GUCView.Fonts.Menu;
            timer.Texts[0].Format = GUCVisualText.TextFormat.Center;

            timer.CreateText("ALKILLS", res[0] / 2 - BoardWidth / 4, (res[1] - BoardHeight) / 2 + TimerOffset); timer.Texts[1].Format = GUCVisualText.TextFormat.Center; timer.Texts[1].SetColor(new ColorRGBA(255, 200, 200));
            timer.CreateText("NLKILLS", res[0] / 2 + BoardWidth / 4, (res[1] - BoardHeight) / 2 + TimerOffset); timer.Texts[2].Format = GUCVisualText.TextFormat.Center; timer.Texts[2].SetColor(new ColorRGBA(200, 200, 255));
            back.AddChild(timer);

            specView = GUCVisualText.Create("Zuschauer", (res[0] - BoardWidth) / 2 + xOffset, (res[1] + BoardHeight) / 2);
            specView.Texts[0].SetColor(LineColor);
            back.AddChild(specView);

            var line = new BoardLine(back, xOffset, yOffset, true);
            line.name.Text = "Name"; line.kills.Text = "K /"; line.deaths.Text = "D"; line.damage.Text = "DMG"; line.ping.Text = "Ping";
            alPlayers.Add(line);

            line = new BoardLine(back, BoardWidth / 2 + xOffset, yOffset, true);
            line.name.Text = "Name"; line.kills.Text = "K /"; line.deaths.Text = "D"; line.damage.Text = "DMG"; line.ping.Text = "Ping";
            nlPlayers.Add(line);
        }

        bool isOpen = false;
        long openTime;
        public void Open()
        {
            if (closeTimer.Started) // still open
            {
                closeTimer.Stop();
                return;
            }

            if (isOpen)
                return;

            PacketWriter stream = GameClient.Client.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.OpenScoreboard);
            GameClient.Client.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);
            back.Show();

            StatusMenu.Menu.ScoreShow = true;

            openTime = GameTime.Ticks;
            isOpen = true;

            Update(openTime);
        }

        GUCTimer closeTimer;
        public void Close()
        {
            if (!isOpen)
                return;

            long diff = 2500000L - GameTime.Ticks + openTime;
            if (diff > 0) // shorter than 250ms
            {
                closeTimer.SetInterval(diff);
                closeTimer.Start();
                return;
            }
            closeTimer.Stop();
            PacketWriter stream = GameClient.Client.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.CloseScoreboard);
            GameClient.Client.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
            back.Hide();

            StatusMenu.Menu.ScoreShow = false;
            isOpen = false;
        }

        public void Update(long now)
        {
            if (!isOpen) return;

            long secsLeft = (TFFAClient.PhaseEndTime - now) / TimeSpan.TicksPerSecond;

            if (secsLeft < 0) secsLeft = 0;

            timer.Texts[0].Text = secsLeft.ToString();
        }
    }
}
