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
        const float MinPositionDistance = 9.0f;
        const float MinDirectionDifference = 0.01f;

        public static void ReadPosDirMessage(PacketReader stream)
        {
            BaseVob vob;
            if (World.Current.TryGetVob(stream.ReadUShort(), out vob))
            {
                var pos = stream.ReadCompressedPosition();
                if (vob.GetPosition().GetDistance(pos) >= MinPositionDistance)
                {
                    vob.SetPosition(pos);
                }
                vob.SetDirection(stream.ReadCompressedDirection());

                vob.ScriptObject.OnPosChanged();
            }
        }

        static long nextUpdate = 0;
        const long updateTime = 1000000; // 100ms
        static Vec3f lastPos;
        static Vec3f lastDir;
        public static void WritePosDirMessage(long now)
        {
            BaseVob vob = GameClient.Client.character;

            if (now < nextUpdate || vob == null)
                return;

            Vec3f pos = GetLimitedPosition(vob);
            Vec3f dir = vob.GetDirection();
            if (now - nextUpdate < TimeSpan.TicksPerSecond && // send at least once per second
                pos.GetDistance(lastPos) < MinPositionDistance && dir.GetDistance(lastDir) < MinDirectionDifference)
                return;

            lastPos = pos;
            lastDir = dir;

            PacketWriter stream = GameClient.SetupStream(NetworkIDs.VobPosDirMessage);

            stream.WriteCompressedPosition(pos);
            stream.WriteCompressedDirection(vob.GetDirection());
            GameClient.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);

            nextUpdate = now + updateTime;

            GameClient.Client.character.ScriptObject.OnPosChanged();
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

        public static bool ChangedCoord(ref float coord)
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
