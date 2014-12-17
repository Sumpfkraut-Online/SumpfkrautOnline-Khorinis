using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages.NpcCommands
{
    class ScaleMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0;
            Vec3f scale = new Vec3f();
            stream.Read(out playerID);
            stream.Read(out scale);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPCProto!");

            ((NPCProto)vob).Scale = scale;

            if (vob.Address != 0)
            {
                ((NPCProto)vob).setScale(scale);
            }
        }
    }
}
