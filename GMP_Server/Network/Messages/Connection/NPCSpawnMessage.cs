using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Server.Network.Messages.VobCommands;

namespace GUC.Server.Network.Messages.Connection
{
    class NPCSpawnMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int plId = 0;
            stream.Read(out plId);

            if (!sWorld.VobDict.ContainsKey(plId))
            {
                Log.Logger.log(Log.Logger.LOG_WARNING, "No Playerid found! "+plId);
                return;
            }

            Player pl = (Player)sWorld.VobDict[plId];
            pl.spawned();

            sWorld.getWorld(pl.Map).addVob(pl);

            SpawnVobMessage.Write(pl, packet.guid);
            Scripting.Objects.Character.Player.isOnPlayerSpawn((Scripting.Objects.Character.Player)pl.ScriptingNPC);

           
        }
    }
}
