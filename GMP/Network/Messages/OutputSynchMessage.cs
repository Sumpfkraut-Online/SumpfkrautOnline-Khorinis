using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using RakNet;
using Injection;
using Gothic.zStruct;
using GMP.Helper;
using WinApi;
using Gothic.zClasses;
using GMP.Modules;

namespace GMP.Network.Messages
{
    public class OutputSynchMessage : Message
    {
        public void Write(RakNet.BitStream stream, Client client, Player pl1, Player pl2, int type, String name)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.SoundSynch);
            stream.Write(type);
            stream.Write(pl1.id);
            if (pl2 == null)
                stream.Write(-1);
            else
                stream.Write(pl2.id);
            stream.Write(name);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.SoundSynch))
                StaticVars.sStats[(int)NetWorkIDS.SoundSynch] = 0;
            StaticVars.sStats[(int)NetWorkIDS.SoundSynch] += 1;
        }


        public override void Read(BitStream stream, Packet packet, Client client)
        {
            int type, id1, id2;String output;
            stream.Read(out type);
            stream.Read(out id1);
            stream.Read(out id2);
            stream.Read(out output);

            Player pl1 = StaticVars.AllPlayerDict[id1];
            if (pl1 == null)
                return;

            Player pl2 = null;
            int npc2Address = 0;
            if (id2 != -1)
            {
                pl2 = StaticVars.AllPlayerDict[id2];
                npc2Address = pl2.NPCAddress;
            }
            
            Process process = Process.ThisProcess();
            if (type == (int)oCMsgConversation.TConversationSubType.EV_OUTPUT)
            {
                External_Helper.AI_OutputSVM_Overlay(process, new oCNpc(process, pl1.NPCAddress), new oCNpc(process, npc2Address), output);
            }
            else if (type == (int)oCMsgConversation.TConversationSubType.EV_OUTPUTSVM)
            {
                External_Helper.AI_OutputSVM_Overlay(process, new oCNpc(process, pl1.NPCAddress), new oCNpc(process, npc2Address), output);
            }
            else if (type == (int)oCMsgConversation.TConversationSubType.EV_OUTPUTSVM_OVERLAY)
            {
                External_Helper.AI_OutputSVM_Overlay(process, new oCNpc(process, pl1.NPCAddress), new oCNpc(process, npc2Address), output);
            }

        }
    }
}
