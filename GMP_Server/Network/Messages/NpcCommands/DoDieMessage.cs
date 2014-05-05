using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Types;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.NpcCommands
{
    class DoDieMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int victimID = 0, killerID = 0;

            stream.Read(out victimID);
            stream.Read(out killerID);

            NPCProto victim = (NPCProto)sWorld.VobDict[victimID];
            NPCProto attacker = (NPCProto)sWorld.VobDict[killerID];

            Scripting.Objects.Item weapon = null;
            if (attacker.Weapon != null)
            {
                weapon = attacker.Weapon.ScriptingProto;
            }

            victim.ScriptingNPC.HP = 0;

            Scripting.Objects.Character.NPCProto.OnPlayerDamages(victim.ScriptingNPC, DamageType.DAM_POINT, new Vec3f(), null, attacker.ScriptingNPC, attacker.WeaponMode, 0, weapon, 0);
        }
    }
}
