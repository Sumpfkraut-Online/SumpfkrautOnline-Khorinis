using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using GUC.Network;
using GUC.Scripts.TFFA;
using GUC.Scripting;
using GUC.Types;

namespace GUC.Client.Scripts.TFFA
{
    class Scoreboard
    {
        static readonly ColorRGBA LineColor = new ColorRGBA(200, 200, 200);
        static readonly ColorRGBA PlayerColor = new ColorRGBA(250, 255, 250);

        class BoardLine
        {
            public GUCVisualText name, kills, deaths, damage, ping;

            public ClientInfo Info { get; private set; }

            public BoardLine(GUCVisual vis, int x, int y, bool white = false)
            {
                name = vis.CreateText("", x, y);
                kills = vis.CreateText("", x + 130, y);
                deaths = vis.CreateText("", x + 170, y);
                damage = vis.CreateText("", x + 210, y);
                ping = vis.CreateText("", x + 255, y);

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
                while (GUCView.StringPixelWidth(pn) > 125)
                    pn = pn.Remove(pn.Length - 1);

                this.name.Text = pn;
            }
        }


        public static readonly Scoreboard Menu = new Scoreboard();

        GUCVisual back;
        List<BoardLine> alPlayers = new List<BoardLine>();
        List<BoardLine> nlPlayers = new List<BoardLine>();
        GUCVisual timer;

        const int nlXOffset = 330, alXOffset = 10, yOffset = 10, yDist = 20;

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
                        line = new BoardLine(this.back, alXOffset, yOffset + yDist * alCounter);
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
                        line = new BoardLine(this.back, nlXOffset, yOffset + yDist * nlCounter);
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
            
            for (int i = alCounter + 1; i < alPlayers.Count; i++)
                alPlayers[i].Clear();
            for (int i = nlCounter + 1; i < nlPlayers.Count; i++)
                nlPlayers[i].Clear();
        }

        public void UpdateStats(int seconds, int alKills, int nlKills)
        {
            timer.Texts[0].Text = seconds.ToString();
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
                alPlayers[i].SetName(alPlayers[i].Info);
            }
        }

        public Scoreboard()
        {
            int[] res = GUCView.GetScreenSize();
            back = new GUCVisual(res[0] / 2 - 320, res[1] / 2 - 240, 640, 480);
            back.SetBackTexture("scoreboard.tga");
            closeTimer = new GUCTimer(Close);

            timer = new GUCVisual();
            timer.CreateTextCenterX("Timer", res[1] / 2 - 260);
            timer.Font = GUCView.Fonts.Menu;
            timer.Texts[0].Format = GUCVisualText.TextFormat.Center;

            timer.CreateText("ALKILLS", res[0] / 2 - 150, res[1] / 2 - 260); timer.Texts[1].Format = GUCVisualText.TextFormat.Center; timer.Texts[1].SetColor(new Types.ColorRGBA(255, 200, 200));
            timer.CreateText("NLKILLS", res[0] / 2 + 150, res[1] / 2 - 260); timer.Texts[2].Format = GUCVisualText.TextFormat.Center; timer.Texts[2].SetColor(new Types.ColorRGBA(200, 200, 255));
            back.AddChild(timer);

            var line = new BoardLine(back, alXOffset, yOffset, true);
            line.name.Text = "Name"; line.kills.Text = "K /"; line.deaths.Text = "D"; line.damage.Text = "DMG"; line.ping.Text = "Ping";
            alPlayers.Add(line);

            line = new BoardLine(back, nlXOffset, yOffset, true);
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
    }
}
