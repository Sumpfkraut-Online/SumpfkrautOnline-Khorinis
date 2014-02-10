using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using Injection;
using Gothic.zClasses;
using WinApi;
using GMP.Injection.Synch;
using Gothic.zTypes;
using GMP.Modules;

namespace GMP.Net.Messages
{
    public class AnimationOverlayMessage : Message
    {

        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id = 0;
            byte type = 0;
            string aniID = "";
            float value = 0;

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out aniID);
            stream.Read(out value);

            if (Program.Player == null || id == Program.Player.id)
                return;

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null|| !pl.isSpawned)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            zString anim = zString.Create(process, aniID);
            if (type == 1)
                npc.ApplyOverlay(anim);
            else if (type == 2)
                npc.ApplyTimedOverlayMds(anim, value);
            else if (type == 3)
                npc.RemoveOverlay(anim);
            anim.Dispose();
        }

        public void Write(RakNet.BitStream stream, Client client, byte type, String id, float value)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AnimationOverlayMessage);
            stream.Write(Program.Player.id);
            stream.Write(type);
            stream.Write(id);
            stream.Write(value);

            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.AnimationOverlayMessage))
                StaticVars.sStats[(int)NetWorkIDS.AnimationOverlayMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.AnimationOverlayMessage] += 1;
        }
    }
}
