using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;

namespace GUC.Server.Network
{
    public interface IMessage
    {
        void Read(BitStream stream, Packet packet, Server server);
    }
}
