using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Client.WorldObjects;
using GUC.Enumeration;
using GUC.Network;
using GUC.Types;
using Gothic.zTypes;
using Gothic.zStruct;
using Gothic.zClasses;

namespace GUC.Client.Network.Messages
{
    static class VobMessage
    {
        public static void ReadPosDir(BitStream stream)
        {
            uint id = stream.mReadUInt();

            Vob vob = World.GetVobByID(id);
            if (vob != null)
            {
                Vec3f pos = stream.mReadVec();
                Vec3f dir = stream.mReadVec();

                vob.Position = pos;

                if (vob is NPC)
                {
                    NPC npc = (NPC)vob;
                    if (npc.State == NPCState.Stand)
                    {
                        Vec3f curDir = npc.nextDir == null ? npc.Direction : npc.nextDir;

                        if ((dir.X - curDir.X) * (dir.X - curDir.X) + (dir.Y - curDir.Y) * (dir.Y - curDir.Y) + (dir.Z - curDir.Z) * (dir.Z - curDir.Z) > 0.000000001f)
                        {
                            npc.StartTurnAni(npc.Direction.Z * dir.X - dir.Z * npc.Direction.X > 0);
                            npc.lastDir = npc.Direction;
                            npc.nextDir = dir;
                            npc.lastDirTime = DateTime.Now.Ticks;
                            return;
                        }
                    }
                }

                vob.Direction = dir;
            }
        }

        public static void WritePosDir(Vob vob)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.VobPosDirMessage);
            stream.mWrite(vob.ID);
            stream.mWrite(vob.Position);
            stream.mWrite(vob.Direction);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);
        }
    }
}
