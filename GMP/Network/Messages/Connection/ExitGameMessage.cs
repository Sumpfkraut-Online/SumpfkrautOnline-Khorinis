using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.Connection
{
    class ExitGameMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            CGameManager.ExitGameFunc(Process.ThisProcess());
        }
    }
}
