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
        public void Read(RakNet.BitStream stream, Client client)
        {
            int victimID = 0, killerID = 0;

            stream.Read(out victimID);
            stream.Read(out killerID);

            NPC victim = (NPC)sWorld.VobDict[victimID];
            NPC attacker = (NPC)sWorld.VobDict[killerID];

            Scripting.Objects.Item weapon = null;
            if (attacker.Weapon != null)
            {
                weapon = attacker.Weapon.ScriptingProto;
            }

            victim.ScriptingNPC.HP = 0;

            Scripting.Objects.Character.NPC.isOnDamage(victim.ScriptingNPC, DamageTypes.DAM_POINT, new Vec3f(), null, attacker.ScriptingNPC, attacker.WeaponMode, null, weapon, 0);
        }
    }
}
