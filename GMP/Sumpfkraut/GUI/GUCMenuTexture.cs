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
    class GUCMenuTexture : GUCMVisual
    {
        Process proc;
        zCView thisView;

        int[] vpos;

        public GUCMenuTexture(string tex)
        {
            proc = Process.ThisProcess();
            thisView = zCView.Create(proc, 0, 0, 0x2000, 0x2000);
            using (zString z = zString.Create(proc, tex))
                thisView.InsertBack(z);
        }
      
        public GUCMenuTexture(string tex, int x, int y, int w, int h)
        {
            proc = Process.ThisProcess();

            vpos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            int[] vsize = InputHooked.PixelToVirtual(proc, new int[] { w, h });
            thisView = zCView.Create(proc, vpos[0], vpos[1], vpos[0] + vsize[0], vpos[1] + vsize[1]);

            using (zString z = zString.Create(proc, tex))
                thisView.InsertBack(z);
        }

        public void Show()
        {
            zCView.GetStartscreen(proc).InsertItem(thisView, 1);
        }

        public void Hide()
        {
            zCView.GetStartscreen(proc).RemoveItem(thisView);
        }

        public void SetTexture(string tex)
        {
            using (zString z = zString.Create(proc, tex))
                thisView.InsertBack(z);
        }

        public void SetPos(int x, int y)
        {
            if (x >= 0) vpos[0] = InputHooked.PixelToVirtualX(proc, x);
            if (y >= 0) vpos[1] = InputHooked.PixelToVirtualY(proc, y);
            thisView.SetPos(vpos[0], vpos[1]);
        }
    }
}
