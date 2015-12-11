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
                PacketWriter stream = Program.client.SetupStream(NetworkID.MobUseMessage);
                stream.Write(mob.ID);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
                nextUseTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        static long nextUnUseTime = 0;
        public static void WriteUnUseMob()
        {
            if (DateTime.UtcNow.Ticks > nextUnUseTime)
            {
                PacketWriter stream = Program.client.SetupStream(NetworkID.MobUnUseMessage);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
                nextUnUseTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        public static void ReadUseMob(BitStream stream)
        {
            uint id = stream.mReadUInt();
            NPC npc = (NPC)World.Vobs.Get(VobType.NPC, id);
            if (npc == null) return;

            id = stream.mReadUInt();
            MobInter mob = (MobInter)World.Vobs.Get(VobType.MobInter, id);
            if (mob == null) return;
            
            npc.UsedMob = mob;
            oCMobMsg msg = oCMobMsg.Create(Program.Process, oCMobMsg.SubTypes.EV_StartInteraction, npc.gVob);
            mob.gVob.GetEM(0).StartMessage(msg, npc.gVob);

            //Turn collision off while attending the mob, so the NPC doesn't get stuck or sits in the air or smth, THX to Situ
            npc.gVob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionStatic;
        }

        public static void ReadUnUseMob(BitStream stream)
        {
            uint id = stream.mReadUInt();
            NPC npc = (NPC)World.Vobs.Get(VobType.NPC, id);
            if (npc == null) return;

            if (npc.UsedMob != null)
            {
                oCMobMsg msg = oCMobMsg.Create(Program.Process, oCMobMsg.SubTypes.EV_StartStateChange, npc.gVob);
                msg.StateChangeLeaving = true;
                npc.UsedMob.gVob.GetEM(0).StartMessage(msg, new zCVob());
            }
        }

    }
}
