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
        static GUCVisualText toName;
        static GUCVisualText toTime;

        static TOInfoScreen()
        {
            vis = new GUCVisual();
            vis.Font = GUCView.Fonts.Menu;

            const int yOffset = 60;
            vis.CreateText("Team Objective läuft!", GUCView.GetScreenSize()[0], yOffset).Format = GUCVisualText.TextFormat.Right;
            toName = vis.CreateText("TO_NAME", GUCView.GetScreenSize()[0], yOffset + GUCView.FontsizeMenu);
            toName.Format = GUCVisualText.TextFormat.Right;
            toTime = vis.CreateText("TIME LEFT", GUCView.GetScreenSize()[0], yOffset + 2 * GUCView.FontsizeMenu);
            toTime.Format = GUCVisualText.TextFormat.Right;
        }

        public static void Show(TODef def)
        {
            if (def == null)
                return;

            toName.Text = def.Name;
            vis.Show();
        }

        public static void Hide()
        {
            vis.Hide();
        }
    }
}
