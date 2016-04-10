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
        GUCVisual timer;

        public void SetTime(int seconds)
        {
            timer.Texts[0].Text = seconds.ToString();
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
            back.AddChild(timer);
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
        }
    }
}
