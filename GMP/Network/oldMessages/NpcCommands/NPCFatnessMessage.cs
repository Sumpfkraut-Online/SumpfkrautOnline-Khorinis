using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages.NpcCommands
{
    class NPCFatnessMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0;
            float fatness = 1f;
            stream.Read(out playerID);
            stream.Read(out fatness);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is NPC))
                throw new Exception("Vob is not an NPCProto!");

            ((NPC)vob).Fatness = fatness;

            if (vob.Address != 0)
            {
                ((NPC)vob).setFatness(fatness);
            }
        }
    }
}
