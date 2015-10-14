using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.PlayerCommands
{
    class PlayerFreezeMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            bool freeze = false;
            stream.Read(out freeze);
            oCNpc.Freeze(Process.ThisProcess(), freeze);

            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Player Freeze: "+freeze, 0, "Program.cs", 0);
        }
    }
}
