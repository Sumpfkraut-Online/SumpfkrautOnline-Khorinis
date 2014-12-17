using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.NpcCommands
{
    class NPCSetInvisibleMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0; bool invisible = false;
            stream.Read(out playerID);
            stream.Read(out invisible);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPCProto!");

            ((NPCProto)vob).setInvisible( invisible );
        }
    }
}
