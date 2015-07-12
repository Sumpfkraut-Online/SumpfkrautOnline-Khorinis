using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Client.WorldObjects;

namespace GUC.Client.Network.Messages
{
    static class NPCMessage
    {
        public static void ReadAnimation(BitStream stream)
        {
            uint id = stream.mReadUInt();
            short anim = stream.mReadShort();

            Vob vob;
            World.VobDict.TryGetValue(id, out vob);
            if (vob == null || !(vob is NPC)) return;

            short oldAni = ((NPC)vob).Animation;
            ((NPC)vob).Animation = anim;
            ((NPC)vob).AnimationStartTime = DateTime.Now.Ticks;

            if (oldAni != short.MaxValue)
                ((NPC)vob).gNpc.GetModel().StopAni(oldAni);
            ((NPC)vob).gNpc.GetModel().StartAni(anim, 0);
        }

        public static void WriteAnimation(NPC npc)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCAnimationMessage);
            stream.mWrite(npc.Animation);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE);
        }
    }
}
