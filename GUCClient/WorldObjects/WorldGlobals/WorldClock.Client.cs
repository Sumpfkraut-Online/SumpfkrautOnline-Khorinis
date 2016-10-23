using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic;
using GUC.Network;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class WorldClock
    {
        #region Network Messages

        internal static class Messages
        {
            public static void ReadTime(PacketReader stream)
            {
                var clock = World.Current.Clock;
                clock.ReadStream(stream);
                clock.ScriptObject.SetTime(clock.Time, clock.Rate);
            }

            public static void ReadTimeStart(PacketReader stream, bool start)
            {
                if (start)
                {
                    World.Current.Clock.ScriptObject.Start();
                }
                else
                {
                    World.Current.Clock.ScriptObject.Stop();
                }
            }
        }

        #endregion

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
