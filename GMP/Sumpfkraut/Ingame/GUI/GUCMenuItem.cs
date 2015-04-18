using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.Types;
using GUC.WorldObjects;

namespace GUC.Sumpfkraut.Ingame.GUI
{
    class GUCMenuItem : GUCMVisual
    {
        zCView thisView;
        oCItem item;
        bool shown;
                
        public GUCMenuItem(int x, int y, int w, int h)
        {
            shown = false;
            SetSize(x, y, w, h);
        }

        public void SetItem(oCItem item)
        {
            this.item = item;
            if (ItemRenderer.renderList.ContainsKey(thisView))
            {
                ItemRenderer.renderList[thisView] = item;
            }
        }

        public void SetSize(int x, int y, int w, int h)
        {
            Hide();
            if (thisView != null) thisView.Dispose();

            Process proc = Process.ThisProcess();
            int[] vpos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            int[] vsize = InputHooked.PixelToVirtual(proc, new int[] { w, h });
            thisView = zCView.Create(proc, vpos[0], vpos[1], vpos[0] + vsize[0], vpos[1] + vsize[1]);
            thisView.FillZ = true;
            thisView.Blit();
            Show();
        }

        public void Show()
        {
            if (!shown)
            {
                ItemRenderer.renderList.Add(thisView, item);
                shown = true;
            }
        }

        public void Hide()
        {
            if (shown)
            {
                ItemRenderer.renderList.Remove(thisView);
                shown = false;
            }
        }
    }
}
