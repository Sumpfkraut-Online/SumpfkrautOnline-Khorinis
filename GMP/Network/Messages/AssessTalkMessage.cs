using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using RakNet;
using Network;
using Injection;
using GMP.Modules;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace GMP.Network.Messages
{
    public class AssessTalkMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int senderid, receiverid;
            byte type;
            stream.Read(out type);
            stream.Read(out senderid);
            stream.Read(out receiverid);


            Player npcSender = StaticVars.AllPlayerDict[senderid];//Onar
            Player npcReceiver = StaticVars.AllPlayerDict[receiverid];//Spieler
            
            if (type != 3)
            {
                Process  Process = Process.ThisProcess();
                if (new oCNpc(Process, npcSender.NPCAddress).CanBeTalkedTo() == 1 && oCInformationManager.GetInformationManager(Process).NPC.Address != npcSender.NPCAddress)
                    new AssessTalkMessage().Write(Program.client.sentBitStream, Program.client, senderid, receiverid, 1);
                else
                    new AssessTalkMessage().Write(Program.client.sentBitStream, Program.client, senderid, receiverid, 3);
            }
            else
            {
                oCNpc npc = new oCNpc(Process.ThisProcess(), npcSender.NPCAddress);
                int hp = npc.HP;
                zVec3 pos = new oCNpc(Process.ThisProcess(), npcSender.NPCAddress).GetPosition();
                npc.ResetPos(pos);
                pos.Dispose();
                npc.HP = hp;

            }
        }

        public void Write(RakNet.BitStream stream, Client client, int senderid, int receiverid, byte type)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AssessTalkMessage);
            stream.Write(type);
            stream.Write(senderid);
            stream.Write(receiverid);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.AssessTalkMessage))
                StaticVars.sStats[(int)NetWorkIDS.AssessTalkMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.AssessTalkMessage] += 1;
        }
    }
}
