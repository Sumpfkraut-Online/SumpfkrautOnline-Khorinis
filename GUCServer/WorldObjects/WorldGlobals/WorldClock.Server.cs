using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class WorldClock
    {
        #region Messages

        internal static class Messages
        {
            public static void WriteTime(WorldClock clock)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldTimeMessage);
                stream.Write(clock.time.GetTotalSeconds());
                stream.Write(clock.rate);
                clock.World.ForEachClient(client => client.Send(stream, PktPriority.Low, PktReliability.Reliable, 'W'));
            }

            public static void WriteTimeStartMessage(WorldClock clock)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldTimeStartMessage);
                clock.World.ForEachClient(client => client.Send(stream, PktPriority.Low, PktReliability.Reliable, 'W'));
            }

            public static void WriteTimeStopMessage(WorldClock clock)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldTimeStopMessage);
                clock.World.ForEachClient(client => client.Send(stream, PktPriority.Low, PktReliability.Reliable, 'W'));
            }
        }

        #endregion

        const long UpdateInterval = 5 * TimeSpan.TicksPerMinute; // synchronize the correct time every 5 minutes with this world's clients

        long nextUpdate;
        partial void pSetTime()
        {
            if (this.world.IsCreated)
            {
                Messages.WriteTime(this);
                nextUpdate = GameTime.Ticks + UpdateInterval;
            }
        }

        partial void pUpdateTime()
        {
            if (this.world.IsCreated && nextUpdate <= GameTime.Ticks)
            {
                Messages.WriteTime(this);
                nextUpdate = GameTime.Ticks + UpdateInterval;
            }
        }

        partial void pStart()
        {
            if (this.world.IsCreated)
                Messages.WriteTimeStartMessage(this);
        }

        partial void pStop()
        {
            if (this.world.IsCreated)
                Messages.WriteTimeStopMessage(this);
        }
    }
}
