using System;
using System.Collections.Generic;
using System.Text;
using Network;

namespace GMP_Server.Net.Message
{
    class PlayerKeyMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id;
            short count;

            stream.Read(out id);
            stream.Read(out count);

            Player pl = Program.playerDict[id];

            for (ushort i = 0; i < count; i++)
            {
                int key;
                bool pressed;
                byte b;

                stream.Read(out key);
                stream.Read(out pressed);
                stream.Read(out b);

                Program.scriptManager.OnKeyAction(new Scripting.Player(pl), key, pressed, b);
            }
        }
    }
}
