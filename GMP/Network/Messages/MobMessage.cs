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

namespace GUC.Client.Network.Messages
{
    static class MobMessage
    {
        public static void WriteUseMob(MobInter mob)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.MobUseMessage);
            stream.mWrite(mob.ID);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);
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
                    oCMobMsg msg = oCMobMsg.Create(Program.Process, oCMobMsg.SubTypes.EV_StartInteraction, npc.gNpc);
                    vob.gVob.GetEM(0).StartMessage(msg, npc.gVob);
                }
            }
        }

    }
}
