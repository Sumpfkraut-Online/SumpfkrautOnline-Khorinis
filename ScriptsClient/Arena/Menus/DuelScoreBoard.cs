using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;

namespace GUC.Scripts.Arena.Menus
{
    static class DuelScoreBoard
    {
        static GUCVisual visual;

        static DuelScoreBoard()
        {
            const int width = 600;
            const int yOffset = 200;

            int[] screen = GUCView.GetScreenSize();
            visual = new GUCVisual((screen[0]-width)/2, yOffset, width, screen[1] - yOffset*2);
            visual.SetBackTexture("MENU_INGAME.TGA");
        }

        public static void Show()
        {
            visual.Show();
        }

        public static void Hide()
        {
            visual.Hide();
        }
    }
}
