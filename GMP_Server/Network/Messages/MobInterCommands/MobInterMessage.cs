using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Mobs;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using RakNet;

namespace GUC.Server.Network.Messages.MobInterCommands
{
    class MobInterMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int vobID = 0, playerID = 0;
            byte mobInterTypeInt = 0;
            char mobInterKey = '0';

            stream.Read(out mobInterTypeInt);
            stream.Read(out playerID);
            stream.Read(out vobID);

            MobInterNetwork mobInterFlags = (MobInterNetwork)mobInterTypeInt;

            if (mobInterFlags.HasFlag(MobInterNetwork.PickLock))
                stream.Read(out mobInterKey);


            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Player not found!");
            Vob plVob = sWorld.VobDict[playerID];
            if (!(plVob is NPCProto))
                throw new Exception("PlayerVob was not from type Player: " + plVob);
            NPCProto player = (NPCProto)plVob;

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            if (!(vob is MobInter))
                throw new Exception("Vob was not from type MobInter: "+vob);

            stream.ResetReadPointer();
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, true);


            MobInter mob = (MobInter)vob;

            if (mobInterFlags == MobInterNetwork.PickLock)
            {
                if (!(vob is MobLockable))
                    throw new Exception("Vob was not from type MobLockable: " + vob);
                MobLockable mobLock = (MobLockable)mob;
                Scripting.Objects.Mob.MobLockable.OnContainerPickLock((Scripting.Objects.Mob.MobLockable)mobLock.ScriptingVob, player.ScriptingNPC, mobInterKey);
            }
            else if (mobInterFlags == MobInterNetwork.OnTrigger)
            {
                mob.State = 1;
                Scripting.Objects.Mob.MobInter.isOnTrigger((Scripting.Objects.Mob.MobInter)mob.ScriptingVob, player.ScriptingNPC);
            }
            else if (mobInterFlags == MobInterNetwork.OnUnTrigger)
            {
                mob.State = 0;
                Scripting.Objects.Mob.MobInter.isOnUnTrigger((Scripting.Objects.Mob.MobInter)mob.ScriptingVob, player.ScriptingNPC);
            }
            else if (mobInterFlags == MobInterNetwork.StartInteraction)
            {
                Scripting.Objects.Mob.MobInter.isOnStartInteraction((Scripting.Objects.Mob.MobInter)mob.ScriptingVob, player.ScriptingNPC);
            }
            else if (mobInterFlags == MobInterNetwork.StopInteraction)
            {
                Scripting.Objects.Mob.MobInter.isOnStopInteraction((Scripting.Objects.Mob.MobInter)mob.ScriptingVob, player.ScriptingNPC);
            }

            
        }
    }
}
