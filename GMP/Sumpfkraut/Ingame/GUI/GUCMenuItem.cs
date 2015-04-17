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
                
        public GUCMenuItem(oCItem item, int x, int y, int w, int h)
        {
            Process proc = Process.ThisProcess();
            this.item = item;
            thisView = zCView.Create(proc, x, y, x + w, y + h);
            thisView.FillZ = true;
            thisView.Blit();
        }

        public void Show()
        {
            ItemRenderer.renderList.Add(thisView, item);
        }

        public void Hide()
        {
            ItemRenderer.renderList.Remove(thisView);
        }
    }
}
