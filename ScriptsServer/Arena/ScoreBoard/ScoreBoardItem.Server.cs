using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Arena
{
    partial struct ScoreBoardItem
    {
        public void Fill(ArenaClient client)
        {
            this.Name = client.CharInfo.Name;

            int ping = client.BaseClient.GetLastPing();
            if (ping < 0)
                this.Ping = -1;
            else if (ping > short.MaxValue)
                this.Ping = short.MaxValue;
            else
                this.Ping = (short)ping;

            this.Score = 0;
            this.Kills = 0;
            this.Deaths = 0;
        }
    }
}
