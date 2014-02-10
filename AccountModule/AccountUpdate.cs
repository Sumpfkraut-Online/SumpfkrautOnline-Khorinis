using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using AccountModule.Ingame;
using Injection;
using AccountModule.Login;

namespace AccountModule
{
    class AccountUpdate : UpdateModule
    {
        static bool started;
        static long startTime;
        static bool startFinished;

        public override void update(Network.Module module)
        {
            if (!Program.FullLoaded)
                return;

            if (!started)
            {
                Program.client.messageListener.Add((byte)0xde, new StartMessage());
                startTime = DateTime.Now.Ticks;
                started = true;
            }

            if (!startFinished && startTime + 10000*100 < DateTime.Now.Ticks)
            {
                //Setzen der alten Positionsdaten usw.
                new StartMessage().Write(Program.client.sentBitStream, Program.client, 0);
                
                startFinished = true;
            }


        }
    }
}
