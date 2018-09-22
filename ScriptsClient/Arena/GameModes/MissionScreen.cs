using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;

namespace GUC.Scripts.Arena.GameModes
{
    static class MissionScreen
    {
        static GUCVisual countdown;
        static GUCVisual mission;

        static MissionScreen()
        {
            countdown = new GUCVisual();
            countdown.CreateTextCenterX("", 200).Font = GUCView.Fonts.Menu;

            const int missionHeight = 300;
            const int missionWidth = 300;
            mission = new GUCVisual(20, GUCView.GetScreenSize().Y - missionHeight - 50, missionWidth, missionHeight);
            mission.CreateText("", 25, 25).Font = GUCView.Fonts.Book;
            mission.SetBackTexture("Letters.tga");
        }

        static bool shown = false;
        public static void Show(string missionText = null)
        {
            if (shown) return;
            shown = true;

            Update(GameTime.Ticks);
            GUCScripts.OnUpdate += Update;


            countdown.Show();
            if (!string.IsNullOrWhiteSpace(missionText))
            {
                mission.Texts[0].Text = missionText;
                mission.Show();
            }
        }

        static void Update(long now)
        {
            var active = GameMode.ActiveMode;
            if (active == null || active.Phase > GamePhase.WarmUp)
            {
                Hide();
                return;
            }

            countdown.Texts[0].Text = active.Phase == GamePhase.None ? "" : new TimeSpan(active.PhaseEndTime - now).ToString(@"mm\:ss");
        }

        public static void Hide()
        {
            if (!shown) return;
            shown = false;

            GUCScripts.OnUpdate -= Update;

            countdown.Hide();
            mission.Hide();
        }
    }
}
