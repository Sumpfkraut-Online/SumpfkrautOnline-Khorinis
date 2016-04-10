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
        int alCount = 0;
        int nlCount = 0;
        GUCVisual alPlayers;
        GUCVisual nlPlayers;
        GUCVisual timer;
        GUCVisual winner;
        int alKills = 0;
        int nlKills = 0;

        public void OpenWinner(Team winner)
        {
            Open();
            openTime = GameTime.Ticks - 50000000L;
            this.winner.Texts[0].Text = winner == Team.AL ? "TEAM GOMEZ HAT GEWONNEN." : "TETRIANDOCH HAT GEWONNEN.";
            closeTimer.SetInterval(60000000L);
            closeTimer.Start();
        }

        public void SetTime(int seconds)
        {
            timer.Texts[0].Text = seconds.ToString();
            for (int i = 0; i < nlPlayers.Texts.Count; i++)
                nlPlayers.Texts[i].Text = "";
            for (int i = 0; i < alPlayers.Texts.Count; i++)
                alPlayers.Texts[i].Text = "";
            alCount = 0;
            nlCount = 0;
            alKills = 0;
            nlKills = 0;
        }

        public void AddPlayer(Team team, string name, int kills, int deaths, int damage)
        {
            int[] res = GUCView.GetScreenSize();
            GUCVisualText view;
            if (team == Team.AL)
            {
                if (alCount < alPlayers.Texts.Count)
                {
                    view = alPlayers.Texts[alCount++];
                }
                else
                {
                    view = alPlayers.CreateText("", res[0] / 2 - 300, res[1] / 2 - 220 + alPlayers.Texts.Count * 10);
                }
                alKills += kills;
                timer.Texts[1].Text = alKills.ToString();
            }
            else
            {
                if (nlCount < nlPlayers.Texts.Count)
                {
                    view = nlPlayers.Texts[nlCount++];
                }
                else
                {
                    view = nlPlayers.CreateText("", res[0] / 2 + 20, res[1] / 2 - 220 + nlPlayers.Texts.Count * 10);
                }
                nlKills += kills;
                timer.Texts[2].Text = nlKills.ToString();
            }

            view.Text = name + " K:" + kills + " D:" + deaths + " DMG:" + damage;
        }

        public Scoreboard()
        {
            int[] res = GUCView.GetScreenSize();
            back = new GUCVisual(res[0]/2 - 320, res[1]/2 - 240, 640, 480);
            back.SetBackTexture("scoreboard.tga");
            closeTimer = new GUCTimer(Close);

            timer = new GUCVisual();
            timer.CreateTextCenterX("Timer", res[1] / 2 - 260);
            timer.Font = GUCView.Fonts.Menu;
            timer.Texts[0].Format = GUCVisualText.TextFormat.Center;
            
            timer.CreateText("ALKILLS", res[0]/2 - 100, res[1] / 2 - 260); timer.Texts[1].Format = GUCVisualText.TextFormat.Center; timer.Texts[1].SetColor(new Types.ColorRGBA(255, 200, 200));
            timer.CreateText("NLKILLS", res[0] / 2 + 100, res[1] / 2 - 260); timer.Texts[2].Format = GUCVisualText.TextFormat.Center; timer.Texts[2].SetColor(new Types.ColorRGBA(200, 200, 255));
            back.AddChild(timer);

            winner = new GUCVisual();
            winner.CreateTextCenterX("", res[1] / 2 - 280);
            winner.Font = GUCView.Fonts.Menu;
            winner.Texts[0].Format = GUCVisualText.TextFormat.Center;
            back.AddChild(winner);

            alPlayers = new GUCVisual();
            nlPlayers = new GUCVisual();
            back.AddChild(alPlayers);
            back.AddChild(nlPlayers);
        }

        long openTime;
        public void Open()
        {
            if (closeTimer.Started) // still open
            {
                closeTimer.Stop();
                return;
            }
            PacketWriter stream = GameClient.Client.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.OpenScoreboard);
            GameClient.Client.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);
            back.Show();

            openTime = GameTime.Ticks;
        }

        GUCTimer closeTimer;
        public void Close()
        {
            long diff = 10000000L - GameTime.Ticks + openTime;
            if (diff > 0) // shorter than 1000ms
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
        }
    }
}
