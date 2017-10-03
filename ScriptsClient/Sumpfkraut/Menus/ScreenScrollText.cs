using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.Menus
{
    public static class ScreenScrollText
    {
        const long TextDuration = 4 * TimeSpan.TicksPerSecond;

        static List<long> endTimes = new List<long>();
        static GUCVisual vis = new GUCVisual();

        public static void AddText(string text, GUCView.Fonts font = GUCView.Fonts.Default)
        {
            AddText(text, font, ColorRGBA.White);
        }

        public static void AddText(string text, GUCView.Fonts font, ColorRGBA color)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            if (vis.Texts.Count == 0)
                vis.CreateTextCenterX("", 0);

            int vFontSize = GUCView.PixelToVirtualY(GUCView.GetFontSize(font));

            // push up active texts
            for (int i = endTimes.Count - 1; i >= 0; i--)
            {
                if (i == vis.Texts.Count - 1)
                    vis.CreateTextCenterX("", 0);

                var newText = vis.Texts[i + 1];
                var curText = vis.Texts[i];

                newText.Font = curText.Font;
                newText.SetColor(curText.GetColor());
                newText.Text = curText.Text;
                newText.SetPosY(curText.VPos.Y - vFontSize, true);
            }

            // add new text
            var visText = vis.Texts[0];
            visText.Font = font;
            visText.SetColor(color);
            visText.Text = text;
            visText.SetPosY(0xB00 - vFontSize, true);

            endTimes.Insert(0, GameTime.Ticks + TextDuration);

            if (endTimes.Count == 1)
            {
                vis.Show();
                GUCScripts.OnUpdate += Update;
            }
        }

        static void Update(long now)
        {
            for (int i = endTimes.Count - 1; i >= 0; i--)
            {
                long endTime = endTimes[i];
                if (endTime <= now)
                {
                    vis.Texts[i].Text = "";
                    endTimes.RemoveAt(i);
                }
                else
                {

                }
            }

            if (endTimes.Count == 0)
            {
                vis.Hide();
                GUCScripts.OnUpdate -= Update;
            }
        }
    }
}
