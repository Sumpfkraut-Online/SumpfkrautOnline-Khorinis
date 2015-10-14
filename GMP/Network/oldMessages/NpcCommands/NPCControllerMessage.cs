using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;

namespace GUC.Network.Messages.NpcCommands
{
    class NPCControllerMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int npcId = 0;
            bool r = false;

            stream.Read(out npcId);
            stream.Read(out r);

            NPC npc = (NPC)sWorld.VobDict[npcId];

            if (!r)
            {
                npc.NpcController.NPCControlledList.Remove(npc);
                npc.NpcController = null;
            }
            else
            {
                Player.Hero.NPCControlledList.Add(npc);
                npc.NpcController = Player.Hero;
            }
        }
    }
}
