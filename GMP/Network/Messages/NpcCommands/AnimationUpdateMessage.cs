using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.NpcCommands
{
    class AnimationUpdateMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID = 0;
            short anim = 0;

            stream.Read(out plID);
            stream.Read(out anim);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPC!");

            short oldAni = ((NPCProto)vob).Animation;
            ((NPCProto)vob).Animation = anim;
            ((NPCProto)vob).AnimationStartTime = Program.Now;

            if (vob.Address == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);

            if(oldAni != short.MaxValue)
                npc.GetModel().StopAni(oldAni);
            npc.GetModel().StartAni(anim, 0);

            //npc.GetModel().GetAniFromAniID(anim).AniName.Value;

        }

        public static void Write(NPCProto proto)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationUpdateMessage);
            stream.Write(proto.ID);
            stream.Write(proto.Animation);

            Program.client.client.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
