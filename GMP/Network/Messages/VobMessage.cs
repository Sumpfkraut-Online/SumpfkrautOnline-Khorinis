using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Client.WorldObjects;
using GUC.Enumeration;
using GUC.Network;
using GUC.Types;

namespace GUC.Client.Network.Messages
{
    static class VobMessage
    {
        public static void ReadPosition(BitStream stream)
        {
            uint id = stream.mReadUInt();
            Vec3f pos = stream.mReadVec();

            Vob vob = null;
            World.VobDict.TryGetValue(id, out vob);
            if (vob != null)
            {
                //vob.Position = pos;
                vob.gVob.BitField1 &= ~(int)Gothic.zClasses.zCVob.BitFlag0.physicsEnabled;
            }
        }

        public static void WritePosition(Vob vob)
        {   
            BitStream stream = Program.client.SetupSendStream(NetworkID.VobPositionMessage);
            stream.mWrite(vob.ID);
            stream.mWrite(vob.Position);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void ReadDirection(BitStream stream)
        {
            uint id = stream.mReadUInt();
            Vec3f dir = stream.mReadVec();

            Vob vob = null;
            World.VobDict.TryGetValue(id, out vob);
            if (vob != null)
            {
                vob.Direction = dir;
                if (vob is NPC && vob != Player.Hero && ((NPC)vob).gNpc.GetBodyState() == 0)
                {
                    ((NPC)vob).gNpc.GetModel().StartAni(Player.AniTurnRight, 0);
                    ((NPC)vob).Animation = (short)Player.AniTurnRight;
                    ((NPC)vob).AnimationStartTime = DateTime.Now.Ticks;
                }
            }
        }

        public static void WriteDirection(Vob vob)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.VobDirectionMessage);
            stream.mWrite(vob.ID);
            stream.mWrite(vob.Direction);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);
        }
    }
}
