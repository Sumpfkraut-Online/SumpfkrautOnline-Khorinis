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
        public void Read(BitStream stream, Client client)
        {
            int plID = 0;
            String levelName = "";
            stream.Read(out plID);
            stream.Read(out levelName);

            Vob pl = sWorld.VobDict[plID];
            sWorld.getWorld(levelName).addVob(pl);

            stream.ResetReadPointer();

            using (RakNetGUID guid = new RakNetGUID(client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, true);
        }
    }
}
