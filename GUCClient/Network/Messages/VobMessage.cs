using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;

namespace GUC.Network.Messages
{
    static class VobMessage
    {
        const float MinPositionDistance = 12.0f;
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
    }
}
