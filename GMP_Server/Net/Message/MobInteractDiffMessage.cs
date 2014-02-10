using System;
using System.Collections.Generic;
using System.Text;
using Network;

namespace GMP_Server.Net.Message
{
    public class MobInteractDiffMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            stream.Read(out id);
            Player player = Program.playerDict[id];

            bool mobInteract = false;
            stream.Read(out mobInteract);

            if (!mobInteract)
            {
                player.mobInteract = null;
            }
            else
            {
                MobInteract mi = new MobInteract();
                stream.Read(out mi.objectName);
                stream.Read(out mi.name);
                stream.Read(out mi.onStateFunc);
                stream.Read(out mi.modelName);
                stream.Read(out mi.pos[0]);
                stream.Read(out mi.pos[1]);
                stream.Read(out mi.pos[2]);

                player.mobInteract = mi;
            }

        }
    }
}
