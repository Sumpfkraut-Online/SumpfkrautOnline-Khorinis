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
        public static void ReadPosDir(PacketReader stream, GameClient client, NPC character)
        {
            var pos = stream.ReadCompressedPosition();
            var dir = stream.ReadCompressedDirection();
            character.UpdatePosition(pos, dir, client);

            character.ScriptObject.OnPosChanged();
        }
    }
}
