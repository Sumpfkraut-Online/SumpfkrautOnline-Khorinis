using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network.Messages;

namespace GUC.WorldObjects.Time
{
    public partial class WorldClock
    {
        const long UpdateInterval = 5 * TimeSpan.TicksPerMinute;

        long nextUpdate;
        partial void pSetTime()
        {
            nextUpdate = GameTime.Ticks;
            WorldMessage.WriteTimeMessage(this);
        }

        partial void pUpdateTime()
        {
            if (nextUpdate <= GameTime.Ticks)
            {
                WorldMessage.WriteTimeMessage(this);
                nextUpdate = GameTime.Ticks + UpdateInterval;
            }
        }

        partial void pStart()
        {
            WorldMessage.WriteTimeStartMessage(this, true);
        }

        partial void pStop()
        {
            WorldMessage.WriteTimeStartMessage(this, false);
        }
    }
}
