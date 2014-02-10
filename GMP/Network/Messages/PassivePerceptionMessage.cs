using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using Injection;
using Gothic.zClasses;
using WinApi;
using RakNet;
using GMP.Injection.Hooks;
using GMP.Modules;

namespace GMP.Network.Messages
{
    public class PassivePerceptionMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int type, npc1_ID, npc2_ID, sNPC_ID;

            stream.Read(out type);
            stream.Read(out sNPC_ID);
            stream.Read(out npc1_ID);
            stream.Read(out npc2_ID);

            if (!StaticVars.AllPlayerDict.ContainsKey(sNPC_ID))
                return;

            int sNPC_Addr = 0, npc2_Addr = 0, npc1_Addr = 0;


            sNPC_Addr = StaticVars.AllPlayerDict[sNPC_ID].NPCAddress;

            if (sNPC_Addr == 0)
                return;

            if (StaticVars.AllPlayerDict.ContainsKey(npc1_ID))
                npc1_Addr = StaticVars.AllPlayerDict[npc1_ID].NPCAddress;

            if (StaticVars.AllPlayerDict.ContainsKey(npc2_ID))
                npc2_Addr = StaticVars.AllPlayerDict[npc2_ID].NPCAddress;

            Process Process = Process.ThisProcess();
            oCNpc npc = new oCNpc(Process, sNPC_Addr);

            AI.disableFunction = true;
            npc.CreatePassivePerception(type, new zCVob(Process, npc1_Addr), new zCVob(Process, npc2_Addr));
        }

        public void Write(RakNet.BitStream stream, Client client, int type, int sNPC_ID, int npc1_ID, int npc2_ID)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.PassivePerceptionMessage);
            stream.Write(type);
            stream.Write(sNPC_ID);
            stream.Write(npc1_ID);
            stream.Write(npc2_ID);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.PassivePerceptionMessage))
                StaticVars.sStats[(int)NetWorkIDS.PassivePerceptionMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.PassivePerceptionMessage] += 1;
        }
    }
}
