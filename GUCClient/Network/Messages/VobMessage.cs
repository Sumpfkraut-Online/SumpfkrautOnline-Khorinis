using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Enumeration;
using GUC.Types;
using RakNet;

namespace GUC.Client.Network.Messages
{
    static class VobMessage
    {
        public static void ReadPosDirMessage(PacketReader stream)
        {
            BaseVob vob;
            if (World.Current.TryGetVob(stream.ReadUShort(), out vob))
            {
                vob.SetPosition(stream.ReadVec3f());
                vob.SetDirection(stream.ReadVec3f());
            }
        }

        static long nextUpdate = 0;
        const long updateTime = 1000000; // 100ms
        public static void WritePosDirMessage(long now)
        {
            if (now < nextUpdate || GameClient.Client.character == null)
                return;

            BaseVob vob = GameClient.Client.character;

            PacketWriter stream = GameClient.SetupStream(NetworkIDs.VobPosDirMessage);

            stream.WriteCompressedPosition(GetLimitedPosition(vob));
            stream.WriteCompressedDirection(vob.GetDirection());
            GameClient.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);

            nextUpdate = now + updateTime;
        }

        static Vec3f GetLimitedPosition(BaseVob vob)
        {
            Vec3f pos = vob.GetPosition();
            if (ChangedCoord(ref pos.X) || ChangedCoord(ref pos.Y) || ChangedCoord(ref pos.Z))
            {
                vob.SetPosition(pos);
            }
            return pos;
        }

        static bool ChangedCoord(ref float coord)
        {
            bool changed = false;
            if (coord < -838860.8f)
            {
                coord = 838860.8f;
                changed = true;
            }
            if (coord > 838860.7f)
            {
                coord = 838860.7f;
                changed = true;
            }
            return changed;
        }
    }
}
