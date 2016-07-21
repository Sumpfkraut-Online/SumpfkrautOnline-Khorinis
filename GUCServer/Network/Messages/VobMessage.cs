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
        public static void ReadPosDir(PacketReader stream, GameClient client, NPC character)
        {
            var pos = stream.ReadCompressedPosition();
            var dir = stream.ReadCompressedDirection();
            character.envState = (EnvironmentState)stream.ReadByte();
            //character.SetPosDir(pos, dir, client);

            character.ScriptObject.OnPosChanged();
        }
    }
}
