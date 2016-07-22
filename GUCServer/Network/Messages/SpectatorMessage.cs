using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Network.Messages
{
    static class SpectatorMessage
    {
        static System.IO.StreamWriter sw = new System.IO.StreamWriter("stats.txt");
        public static void ReadPos(PacketReader stream, GameClient client)
        {
            GameClient.CleanWatch.Reset();
            GameClient.UpdateWatch.Reset();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            client.SetPosition(stream.ReadCompressedPosition(), false);
            watch.Stop();

            sw.WriteLine(String.Format("{0:0.00}, Cleaning: {1:0.00}, Updating: {2:0.00}", watch.Elapsed.TotalMilliseconds, GameClient.CleanWatch.Elapsed.TotalMilliseconds, GameClient.UpdateWatch.Elapsed.TotalMilliseconds));
            sw.Flush();
        }
    }
}
