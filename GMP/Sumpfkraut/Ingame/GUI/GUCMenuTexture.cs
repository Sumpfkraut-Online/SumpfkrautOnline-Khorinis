﻿using System;
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
    class GUCMenuTexture : GUCMVisual
    {
        Process proc;
        zCView thisView;
                
        public GUCMenuTexture(string tex, int x, int y, int w, int h)
        {
            proc = Process.ThisProcess();

            thisView = zCView.Create(proc, x, y, x + w, y + h);
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
            using (zString z = zString.Create(Process.ThisProcess(), tex))
                thisView.InsertBack(z);
        }
    }
}
