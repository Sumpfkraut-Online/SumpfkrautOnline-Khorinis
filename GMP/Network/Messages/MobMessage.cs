using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Client.WorldObjects;
using GUC.Enumeration;
using GUC.Network;
using GUC.Types;
using Gothic.zStruct;
using GUC.Client.Hooks;
using Gothic.zClasses;

namespace GUC.Client.Network.Messages
{
    static class MobMessage
    {
        const int DelayBetweenMessages = 5000000; //500ms

        static long nextUseTime = 0;
        public static void WriteUseMob(MobInter mob)
        {
            if (DateTime.UtcNow.Ticks > nextUseTime)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.MobUseMessage);
                stream.mWrite(mob.ID);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
                nextUseTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        static long nextUnUseTime = 0;
        public static void WriteUnUseMob()
        {
            if (DateTime.UtcNow.Ticks > nextUnUseTime)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.MobUnUseMessage);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
                nextUnUseTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        public static void ReadUseMob(BitStream stream)
        {
            uint npcID = stream.mReadUInt();
            uint mobID = stream.mReadUInt();

            NPC npc;

            if (World.npcDict.TryGetValue(npcID, out npc))
            {
                AbstractVob vob;
                if (World.vobDict.TryGetValue(mobID, out vob) && vob is MobInter)
                {
                    npc.UsedMob = (MobInter)vob;
                    oCMobMsg msg = oCMobMsg.Create(Program.Process, oCMobMsg.SubTypes.EV_StartInteraction, npc.gNpc);
                    vob.gVob.GetEM(0).StartMessage(msg, npc.gVob);
                }
            }
        }

        public static void ReadUnUseMob(BitStream stream)
        {
            uint npcID = stream.mReadUInt();

            NPC npc;

            if (World.npcDict.TryGetValue(npcID, out npc) && npc.UsedMob != null)
            {
                oCMobMsg msg = oCMobMsg.Create(Program.Process, oCMobMsg.SubTypes.EV_StartStateChange, npc.gNpc);
                msg.StateChangeLeaving = true;
                npc.UsedMob.gVob.GetEM(0).StartMessage(msg, new zCVob());
            }
        }

    }
}
