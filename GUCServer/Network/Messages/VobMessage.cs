using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Server.WorldObjects.Cells;

namespace GUC.Server.Network.Messages
{
    static class VobMessage
    {
        /*public static void ReadPosDir(PacketReader stream, GameClient client, NPC character)
        {
            Vob vob = client.Character;
            vob.Position = stream.ReadVec3f();
            vob.Direction = stream.ReadVec3f();
            //vob.World.UpdatePosition(vob, client);
            
            int max = stream.ReadInt();

            uint id; Vec3f pos; Vec3f dir;
            for (int i = 0; i < max; i++)
            {
               
                id = stream.ReadUInt();
                pos = stream.ReadVec();
                dir = stream.ReadVec();

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

        public static void WritePosDir(NetCell cell, BaseVob vob)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.VobPosDirMessage);
            stream.Write((ushort)vob.ID);
            stream.Write(vob.GetPosition());
            stream.Write(vob.Direction);
            foreach (GameClient client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE, 'W');
        }*/
    }
}
