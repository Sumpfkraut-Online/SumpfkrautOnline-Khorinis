using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;

namespace GUC.Scripts.Arena.Menus
{
    static class TOInfoScreen
    {
        static GUCVisual vis;
        //static GUCVisualText toName;
        static GUCVisualText toTime;

        static TOInfoScreen()
        {
            vis = new GUCVisual();
            vis.Font = GUCView.Fonts.Menu;

            const int yOffset = 60;
            var text = vis.CreateText("Team Objective", GUCView.GetScreenSize().X, yOffset);
            text.Format = GUCVisualText.TextFormat.Right;

            //toName = vis.CreateText("TO_NAME", GUCView.GetScreenSize().Width, yOffset + GUCView.FontsizeMenu);
            //toName.Format = GUCVisualText.TextFormat.Right;

            toTime = vis.CreateText("TIME LEFT", GUCView.GetScreenSize().X, yOffset + 1 * GUCView.FontsizeMenu);
            toTime.Format = GUCVisualText.TextFormat.Right;
        }

        public static void Show()
        {
            if (TeamMode.ActiveTODef == null)
                return;

            //toName.Text = TeamMode.ActiveTODef.Name;
            vis.Show();

            GUCScripts.OnUpdate += Update;
        }

        public static void Hide()
        {
            vis.Hide();

            GUCScripts.OnUpdate -= Update;
        }

        static void Update(long now)
        {
            long timeLeft = TeamMode.PhaseEndTime - now;
            if (timeLeft < 0) timeLeft = 0;
            long mins = timeLeft / TimeSpan.TicksPerMinute;
            long secs = timeLeft % TimeSpan.TicksPerMinute / TimeSpan.TicksPerSecond;

            string phase;
            switch (TeamMode.Phase)
            {
                case TOPhases.Battle:
                    phase = "Kampf läuft.";
                    break;
                case TOPhases.Warmup:
                    phase = "Startet in";
                    break;
                case TOPhases.Finish:
                    phase = "Ende";
                    break;
                default:
                    phase = TeamMode.Phase.ToString();
                    break;
            }

            toTime.Text = string.Format("{0} {1}:{2:00}", phase, mins, secs);
        }
    }
}
