using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using GMP.Modules;
using Gothic.zClasses;
using WinApi;
using Injection;
using RakNet;

namespace GMP.Network.Messages
{
    class AssessPlayerMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int npcid = 0;
            int playerID = 0;
            stream.Read(out playerID);
            stream.Read(out npcid);

            Player npcPL = StaticVars.AllPlayerDict[npcid];
            if (npcPL == null)
                return;
            Player playerPL = StaticVars.AllPlayerDict[playerID];
            if (playerPL == null)
                return;
            Process Process = Process.ThisProcess();
            new oCNpc(Process, npcPL.NPCAddress).AssessPlayer_S(new oCNpc(Process, playerPL.NPCAddress));
        }

        public void Write(RakNet.BitStream stream, Client client, int npcID)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AssessPlayerMessage);
            stream.Write(Program.Player.id);
            stream.Write(npcID);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.AssessPlayerMessage))
                StaticVars.sStats[(int)NetWorkIDS.AssessPlayerMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.AssessPlayerMessage] += 1;
        }
    }
}
