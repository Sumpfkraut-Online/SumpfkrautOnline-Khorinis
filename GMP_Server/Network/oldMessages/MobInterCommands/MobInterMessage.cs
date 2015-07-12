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
        public static void Write(MobInterNetwork network, NPC player, Vob vob)
        {
            Write(network, player, vob, 'L');
        }

        public static void Write(MobInterNetwork network, NPC player, Vob vob, char pickLock)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.MobInterMessage);
            stream.Write((byte)network);
            stream.Write(player.ID);
            stream.Write(vob.ID);

            if (network.HasFlag(MobInterNetwork.PickLock))
                stream.Write(pickLock);

            if (player is Player)
            {
                using (RakNet.RakNetGUID guid = new RakNetGUID(player.Guid))
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
            }
            else
            {
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            }
        }


        public void Read(BitStream stream, Client client)
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
            if (!(plVob is NPC))
                throw new Exception("PlayerVob was not from type Player: " + plVob);
            NPC player = (NPC)plVob;

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            if (!(vob is MobInter))
                throw new Exception("Vob was not from type MobInter: "+vob);

            stream.ResetReadPointer();
            using (RakNetGUID guid = new RakNetGUID(client.guid))
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, true);


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
