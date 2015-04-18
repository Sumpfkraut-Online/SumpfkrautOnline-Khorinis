using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.Types;

namespace GUC.Sumpfkraut.Ingame.GUI
{
    class GUCMenuText : GUCMVisual
    {
        Process proc;
        zCView thisView;
        zCViewText viewText;
                
        public GUCMenuText(string t, int x, int y)
        {
            proc = Process.ThisProcess();

            int[] pos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            thisView = zCView.Create(proc, 0, 0, 0x2000, 0x2000);
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

        public void SetText(string t)
        {
            viewText.Text.Set(t);
        }

        public void SetPos(int x, int y)
        {
            int[] pos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            viewText.PosX = pos[0];
            viewText.PosY = pos[1];
        }

        public void SetColor(ColorRGBA c)
        {
            viewText.Color.R = c.R;
            viewText.Color.G = c.G;
            viewText.Color.B = c.B;
            viewText.Color.A = c.A;
        }
    }
}
