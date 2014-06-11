using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;

namespace GUC.timer
{
    public class DespawnTimer : Timer
    {
        protected int mNPCAddress = 0;
        public DespawnTimer(int npcAddress)
            : base(10000*5000)
        {
            mNPCAddress = npcAddress;
            Start();
            OnTick += new TimerEvent(tick);
        }

        protected void tick()
        {
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, this.mNPCAddress);
            oCGame.Game(process).GetSpawnManager().DeleteNPC(npc);

            End();
        }
    }
}
