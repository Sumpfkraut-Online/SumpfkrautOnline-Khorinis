using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using RakNet;

namespace GUC.Server.Network.Messages.VobCommands
{
    class ChangeWorldMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int plID = 0;
            String levelName = "";
            stream.Read(out plID);
            stream.Read(out levelName);

            Vob pl = sWorld.VobDict[plID];
            sWorld.getWorld(levelName).addVob(pl);

            stream.ResetReadPointer();
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, true);


            Console.WriteLine(plID+" Move to World: "+levelName);
        }
    }
}
