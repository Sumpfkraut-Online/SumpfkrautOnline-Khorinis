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
        public static void ReadPosDir(BitStream stream, Client client)
        {
            uint id = stream.mReadUInt();
            AbstractVob vob;

            sWorld.AllVobs.TryGetValue(id, out vob);
            if (vob == null || !(vob is AbstractCtrlVob))
                return;


            
            if (vob == client.character || client.VobControlledList.Contains(vob))
            {
                Vec3f pos = stream.mReadVec();
                Vec3f dir = stream.mReadVec();
                vob.pos = pos;
                vob.dir = dir;
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

        public static void WritePosDir(IEnumerable<Client> list, AbstractVob vob)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.VobPosDirMessage);
            stream.mWrite(vob.ID);
            stream.mWrite(vob.Position);
            stream.mWrite(vob.Direction);
            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, (char)0, client.guid, false);
        }
    }
}
