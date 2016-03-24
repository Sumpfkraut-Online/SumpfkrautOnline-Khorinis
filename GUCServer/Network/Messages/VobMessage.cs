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
            AbstractCtrlVob vob = client.character;
            vob.pos = stream.mReadVec();
            vob.dir = stream.mReadVec();
            vob.World.UpdatePosition(vob, client);

            int max = stream.mReadInt();

            uint id; Vec3f pos; Vec3f dir;
            for (int i = 0; i < max; i++)
            {
               
                id = stream.mReadUInt();
                pos = stream.mReadVec();
                dir = stream.mReadVec();

                vob = client.VobControlledList.Find(v => v.ID == id);

                if (vob != null)
                {
                    vob.pos = pos;
                    vob.dir = dir;
                    if (vob is AbstractDropVob)
                    {
                        ((AbstractDropVob)vob).Update(vob.pos);
                    }
                    else
                    {
                        vob.World.UpdatePosition(vob, client);
                    }
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
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W', client.guid, false);
        }
    }
}
