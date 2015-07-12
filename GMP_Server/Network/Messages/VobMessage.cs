using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Types;
using GUC.Network;
using GUC.Server.WorldObjects;

namespace GUC.Server.Network.Messages
{
    static class VobMessage
    {
        public static void ReadPosition(BitStream stream, Client client)
        {
            uint id = stream.mReadUInt();
            Vec3f pos = stream.mReadVec();
            AbstractVob vob;

            sWorld.AllVobs.TryGetValue(id, out vob);
            if (vob == null || !(vob is AbstractCtrlVob))
                return;
            
            if (vob == client.character || client.VobControlledList.Contains(vob))
            {
                vob.pos = pos;
                if (vob is AbstractDropVob)
                {
                    ((AbstractDropVob)vob).Update(pos);
                }
                else
                {
                    vob.World.UpdatePosition(vob, client);
                }
            }
        }

        public static void ReadDirection(BitStream stream, Client client)
        {
            uint id = stream.mReadUInt();
            Vec3f dir = stream.mReadVec();
            AbstractVob vob;

            sWorld.AllVobs.TryGetValue(id, out vob);
            if (vob == null || !(vob is AbstractCtrlVob))
                return;

            if (vob == client.character || client.VobControlledList.Contains(vob))
            {
                vob.dir = dir;
                WriteDirection(vob.cell.SurroundingClients(client), vob);
            }

        }

        public static void WritePosition(IEnumerable<Client> list, AbstractVob vob)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.VobPositionMessage);
            stream.mWrite(vob.ID);
            stream.mWrite(vob.Position);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, (char)0, client.guid, false);
        }

        public static void WriteDirection(IEnumerable<Client> list, AbstractVob vob)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.VobDirectionMessage);
            stream.mWrite(vob.ID);
            stream.mWrite(vob.dir);

            foreach(Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, (char)0, client.guid, false);
        }
    }
}
