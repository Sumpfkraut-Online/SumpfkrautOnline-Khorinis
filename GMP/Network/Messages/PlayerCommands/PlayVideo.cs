using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.PlayerCommands
{
    class PlayVideo : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            String video = "";
            stream.Read(out video);

            CGameManager.GameManager(Process.ThisProcess()).PlayVideo(video);
        }
    }
}
