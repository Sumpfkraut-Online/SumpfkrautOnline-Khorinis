using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using Injection;
using WinApi;
using Gothic.zClasses;
using GMP.Helper;
using RakNet;
using GMP.Modules;

namespace ListModule.Messages
{
    public class SoundSynchMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id; String guid;

            stream.Read(out id);
            stream.Read(out guid);

            String sfx = "";
            stream.Read(out sfx);

            if (Program.Player == null || Program.Player.id == id || sfx.Trim().Length == 0)
                return;

            Process process = Process.ThisProcess();
            Player player = Player.getPlayer(id, StaticVars.playerlist);

            if (player == null ||player.NPCAddress == 0 || !player.isSpawned)
                return;
            oCNpc npc = new oCNpc(process, player.NPCAddress);
            External_Helper.AI_OutputSVM_Overlay(process, npc, oCNpc.Player(process), sfx);
        }

        public void Write(RakNet.BitStream stream, Client client, String sfx)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xFC);

            stream.Write(Program.Player.id);
            stream.Write(client.client.GetMyGUID().ToString());

            stream.Write(sfx);
            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }
    }
}
