using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using Injection;
using Gothic.zClasses;
using WinApi;
using GMP.Injection.Synch;
using GMP.Modules;

namespace GMP.Net.Messages
{
    public class AnimationMessage : Message
    {

        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id = 0;
            byte type = 0;
            int aniID = 0;
            int value = 0;
            float value2 = 0;

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out aniID);
            if (type == 1 || type == 5 || type == 4)
                stream.Read(out value);
            if (type == 6)
                stream.Read(out value2);

            if (Program.Player == null || id == Program.Player.id)
                return;

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl != null && type == 1)
            {
                pl.lastAniID = aniID;
                pl.lastAniValue = value;
            }

            if (pl == null || !pl.isSpawned)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            if (npc.Address == 0 || npc.GetModel().Address == 0)
                return;

            if (type == 1)
            {
                Animation.startAnimEnabled = true;
                npc.GetModel().StartAni(aniID, value);
            }
            else if (type == 2)
            {
                npc.GetModel().StopAni(aniID);
            }
            else if (type == 3)
                npc.GetModel().FadeOutAni(aniID);
            else if (type == 4)
                npc.GetModel().FadeOutAnisLayerRange(aniID, value);
            else if (type == 5)
                npc.GetModel().StopAnisLayerRange(aniID, value);
            else if (type == 6)
            {
                npc.GetModel().GetActiveAni(aniID).SetActFrame(value2);
            }
            else if (type == 7)
            {
                npc.MagBook.SpellCast();
            }
            else if (type == 8)
                npc.MagBook.Open(aniID);
            else if (type == 9)
                npc.MagBook.Close(aniID);
            else if (type == 10)
                npc.MagBook.Spell_Invest();
        }

        public static long dataLength = 0;
        public static int length = 0;
        public static long startTime = 0;

        public void Write(RakNet.BitStream stream, Client client, byte type, Player player, int id, int value)
        {
            Write(stream, client, type, player, id, value, 0);
        }

        public void Write(RakNet.BitStream stream, Client client, byte type, Player player, int id, int value, float value2)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AnimationMessage);
            stream.Write(player.id);
            stream.Write(type);
            stream.Write((short)id);
            if (type == 1 || type == 5 || type == 4)
                stream.Write(value);
            if (type == 6)
                stream.Write(value2);

            if (id > length)
                length = id;
                

            client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.AnimationMessage))
            {
                startTime = DateTime.Now.Ticks;
                StaticVars.sStats[(int)NetWorkIDS.AnimationMessage] = 0;
            }
            dataLength += stream.GetData().Length;
            StaticVars.sStats[(int)NetWorkIDS.AnimationMessage] += 1;
        }
    }
}
