using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using GMP.Modules;

namespace GMP.Network.Messages
{
    public class StartLevelChangeMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            String world = "";
            stream.Read(out world);

            StaticVars.StartWorld = world;
        }

        public override void Write(RakNet.BitStream stream, Client client)
        {
            
        }
    }
}
