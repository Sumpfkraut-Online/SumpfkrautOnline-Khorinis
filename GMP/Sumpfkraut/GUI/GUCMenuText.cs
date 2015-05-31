using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.Types;

namespace GUC.Sumpfkraut.GUI
{
    class GUCMenuText : GUCMVisual
    {
        Process proc;
        zCView thisView;
        zCViewText viewText;

        public string Text
        {
            set
            {
                viewText.Text.Set(value);
            }
            get
            {
                return viewText.Text.ToString();
            }
        }

        public GUCMenuText(string t, int x, int y, bool menuFont)
        {
            proc = Process.ThisProcess();

            int[] pos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            thisView = zCView.Create(proc, 0, 0, 0x2000, 0x2000);
            if (menuFont) thisView.SetFont("FONT_OLD_20_WHITE.TGA");
            using (zString z = zString.Create(proc, t))
                viewText = thisView.CreateText(pos[0], pos[1], z);
        }

        public void Show()
        {
            zCView.GetStartscreen(proc).InsertItem(thisView, 1);
        }

        public void Hide()
        {
            zCView.GetStartscreen(proc).RemoveItem(thisView);
        }

        public void SetPos(int x, int y)
        {            
            int[] pos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            if (x >= 0) viewText.PosX = pos[0];
            if (y >= 0) viewText.PosY = pos[1];
        }

        public void CenterText()
        {
            viewText.PosX = (0x2000 - thisView.FontSize(Text)) / 2;
        }

        public void SetColor(ColorRGBA c)
        {
            viewText.Color.R = c.R;
            viewText.Color.G = c.G;
            viewText.Color.B = c.B;
            viewText.Color.A = c.A;
        }

        public void SetFont(string font)
        {
            thisView.SetFont(font);
            viewText.Font = thisView.Font;
        }

        public int pixelLen
        {
            private set { }
            get { return viewText.Font.GetFontX(viewText.Text); }
        }
    }
}
