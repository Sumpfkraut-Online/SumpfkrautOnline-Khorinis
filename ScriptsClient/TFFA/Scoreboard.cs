using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using GUC.Network;
using GUC.Scripts.TFFA;
using GUC.Scripting;

namespace GUC.Client.Scripts.TFFA
{
    class Scoreboard
    {
        public static readonly Scoreboard Menu = new Scoreboard();

        GUCVisual back;
        int alCount = 4;
        int nlCount = 4;
        GUCVisual alPlayers;
        GUCVisual nlPlayers;
        GUCVisual timer;
        GUCVisual winner;

        public void OpenWinner(Team winner)
        {
            Open();
            openTime = GameTime.Ticks - 97500000L;
            this.winner.Texts[0].Text = winner == Team.AL ? "!!! TEAM GOMEZ HAT GEWONNEN !!!" : "!!! TETRIANDOCH HAT GEWONNEN !!!";
            closeTimer.SetInterval(100000000L);
            closeTimer.Start();
        }

        public void SetTime(int seconds)
        {
            timer.Texts[0].Text = seconds.ToString();
            for (int i = 4; i < nlPlayers.Texts.Count; i++)
                nlPlayers.Texts[i].Text = "";
            for (int i = 4; i < alPlayers.Texts.Count; i++)
                alPlayers.Texts[i].Text = "";
            alCount = 4;
            nlCount = 4;
        }

        public void SetKills(int alKills, int nlKills)
        {
            timer.Texts[1].Text = alKills.ToString();
            timer.Texts[2].Text = nlKills.ToString();
        }

        public void AddPlayer(Team team, string name, int kills, int deaths, int damage)
        {
            int[] res = GUCView.GetScreenSize();
            if (team == Team.AL)
            {
                if (alCount < alPlayers.Texts.Count)
                {
                    string pn = name;
                    while (GUCView.StringPixelWidth(pn) > 140)
                        pn = pn.Remove(pn.Length - 1);

                    alPlayers.Texts[alCount++].Text = pn;
                    alPlayers.Texts[alCount++].Text = kills.ToString();
                    alPlayers.Texts[alCount++].Text = deaths.ToString();
                    alPlayers.Texts[alCount++].Text = damage.ToString();
                }
            }
            else
            {
                if (nlCount < nlPlayers.Texts.Count)
                {
                    string pn = name;
                    while (GUCView.StringPixelWidth(pn) > 140)
                        pn = pn.Remove(pn.Length - 1);

                    nlPlayers.Texts[nlCount++].Text = pn;
                    nlPlayers.Texts[nlCount++].Text = kills.ToString();
                    nlPlayers.Texts[nlCount++].Text = deaths.ToString();
                    nlPlayers.Texts[nlCount++].Text = damage.ToString();
                }
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

            winner = new GUCVisual();
            winner.CreateTextCenterX("", res[1] / 2 - 290);
            winner.Font = GUCView.Fonts.Menu;
            winner.Texts[0].Format = GUCVisualText.TextFormat.Center;
            back.AddChild(winner);

            alPlayers = new GUCVisual();
            alPlayers.CreateText("Name", res[0] / 2 - 300, res[1] / 2 - 220); alPlayers.Texts[0].SetColor(new Types.ColorRGBA(200, 200, 200));
            alPlayers.CreateText("K", res[0] / 2 - 140, res[1] / 2 - 220); alPlayers.Texts[1].SetColor(new Types.ColorRGBA(200, 200, 200));
            alPlayers.CreateText("D", res[0] / 2 - 100, res[1] / 2 - 220); alPlayers.Texts[2].SetColor(new Types.ColorRGBA(200, 200, 200));
            alPlayers.CreateText("DMG", res[0] / 2 - 60, res[1] / 2 - 220); alPlayers.Texts[3].SetColor(new Types.ColorRGBA(200, 200, 200));
            for (int i = 1; i < 20; i++)
            {
                alPlayers.CreateText("", res[0] / 2 - 300, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
                alPlayers.CreateText("", res[0] / 2 - 140, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
                alPlayers.CreateText("", res[0] / 2 - 100, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
                alPlayers.CreateText("", res[0] / 2 - 60, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
            }
            back.AddChild(alPlayers);

            nlPlayers = new GUCVisual();
            nlPlayers.CreateText("Name", res[0] / 2 + 30, res[1] / 2 - 220); nlPlayers.Texts[0].SetColor(new Types.ColorRGBA(200, 200, 200));
            nlPlayers.CreateText("K", res[0] / 2 + 180, res[1] / 2 - 220); nlPlayers.Texts[1].SetColor(new Types.ColorRGBA(200, 200, 200));
            nlPlayers.CreateText("D", res[0] / 2 + 220, res[1] / 2 - 220); nlPlayers.Texts[2].SetColor(new Types.ColorRGBA(200, 200, 200));
            nlPlayers.CreateText("DMG", res[0] / 2 + 260, res[1] / 2 - 220); nlPlayers.Texts[3].SetColor(new Types.ColorRGBA(200, 200, 200));
            for (int i = 1; i < 20; i++)
            {
                nlPlayers.CreateText("", res[0] / 2 + 30, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
                nlPlayers.CreateText("", res[0] / 2 + 180, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
                nlPlayers.CreateText("", res[0] / 2 + 220, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
                nlPlayers.CreateText("", res[0] / 2 + 260, res[1] / 2 - 220 + i * GUCView.FontsizeDefault);
            }
            back.AddChild(nlPlayers);
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

            PhaseInfo.info.Open();

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
            winner.Texts[0].Text = "";
            
            PhaseInfo.info.Close();
            isOpen = false;
        }
    }
}
