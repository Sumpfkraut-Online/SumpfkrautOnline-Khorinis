using System;
using System.Collections.Generic;
using System.Text;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class Message
    {
        public virtual void Read(BitStream stream, Packet packet, Server server)
        {
            
        }

        public virtual void Write(RakNet.BitStream stream, Server server)
        {

        }
    }
}
