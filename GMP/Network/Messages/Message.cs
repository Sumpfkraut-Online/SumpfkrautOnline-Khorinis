using System;
using System.Collections.Generic;
using System.Text;
using RakNet;

namespace GMP.Net
{
    public class Message
    {
        public virtual void Read(BitStream stream, Packet packet, Client client)
        {

        }

        public virtual void Write(RakNet.BitStream stream, Client client)
        {

        }
    }
}
