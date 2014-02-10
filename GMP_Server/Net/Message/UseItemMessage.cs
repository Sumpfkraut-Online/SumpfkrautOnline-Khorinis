using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class UseItemMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id;
            String instance;

            stream.Read(out id);
            stream.Read(out instance);

            Player pl = Program.playerDict[id];
            if (pl == null)
                return;

            Program.scriptManager.OnUseItem(new Scripting.Player(pl), instance);
        }
    }
}
