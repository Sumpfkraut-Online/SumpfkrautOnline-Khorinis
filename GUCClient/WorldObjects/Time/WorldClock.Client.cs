using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic;

namespace GUC.WorldObjects.Time
{
    public partial class WorldClock
    {
        partial void pSetTime()
        {
            pUpdateTime();
        }

        partial void pUpdateTime()
        {
            oCGame.WorldTimer.SetTime(this.time.GetHour(), this.time.GetMinute());
        }
    }
}
