using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.WorldObjects;

namespace GUC.Network.Messages.PlayerCommands
{
    class DropUnconsciousMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            Process process = Process.ThisProcess();
            //oCNpc npc = oCNpc.Player(process);
            //Player.Hero.Attributes[(int)NPCAttributeFlags.ATR_HITPOINTS] = 1;



            float length = 0;
            int playerID = 0;
            stream.Read(out playerID);
            stream.Read(out length);

            if (!sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("PlayerID: "+playerID+" was not found!");
            NPCProto proto = (NPCProto)sWorld.VobDict[playerID];
            proto.Attributes[(int)NPCAttributeFlags.ATR_HITPOINTS] = 1;

            if (proto.IsSpawned)
            {
                oCNpc npc = new oCNpc(process, proto.Address);
                npc.DropUnconscious(length, new oCNpc(process, 0));
            }
            

        }
    }
}
