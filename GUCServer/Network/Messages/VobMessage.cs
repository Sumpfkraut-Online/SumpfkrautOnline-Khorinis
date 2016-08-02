using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;

namespace GUC.Network.Messages
{
    static class VobMessage
    {
        public static void ReadPosDir(PacketReader stream, GameClient client, World world)
        {
            int id = stream.ReadUShort();
            BaseVob vob;
            if (world.TryGetVob(id, out vob))
            {
                var pos = stream.ReadCompressedPosition();
                var dir = stream.ReadCompressedDirection();
                if (vob is NPC)
                {
                    ((NPC)vob).envState = (EnvironmentState)stream.ReadByte();
                }

                vob.SetPosDir(pos, dir, client);
                vob.ScriptObject.OnPosChanged();

                if (vob == client.Character)
                {
                    client.UpdateVobList(world, pos);
                }
            }
        }
    }
}
