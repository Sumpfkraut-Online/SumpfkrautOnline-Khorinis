using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.WorldObjects;

namespace GUC.Sumpfkraut.GUI
{
    interface GUCMVisual
    {
        void Show();
        void Hide();
    }

    interface GUCInputReceiver
    {
        void KeyPressed(int key);
        void Update(long ticks);
    }
}
