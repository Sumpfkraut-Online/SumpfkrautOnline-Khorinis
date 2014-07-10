using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Network;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class OnDamageMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            Vec3f locHit = null, flydir = null;
            int victim = 0, attacker = 0, weaponMode = 0, spellID = 0, weapon = 0;
            float fallDownDistanceY = 0.0f;
            
            byte sendFlags, damageMode;

            stream.Read(out victim);
            stream.Read(out damageMode);
            stream.Read(out sendFlags);

            if ((sendFlags & 1) == 1)
                stream.Read(out locHit);
            if ((sendFlags & 2) == 2)
                stream.Read(out flydir);
            if ((sendFlags & 4) == 4)
                stream.Read(out attacker);
            if ((sendFlags & 8) == 8)
                stream.Read(out weaponMode);
            if ((sendFlags & 16) == 16)
                stream.Read(out spellID);
            if ((sendFlags & 32) == 32)
                stream.Read(out weapon);
            if ((sendFlags & 64) == 64)
                stream.Read(out fallDownDistanceY);
            
            NPCProto vicProto = (NPCProto)sWorld.VobDict[victim];
            Vob attProto = null;

            Scripting.Objects.Vob attackerScriptProto = null;

            if (attacker != 0)
            {
                attProto = sWorld.VobDict[attacker];
                attackerScriptProto = attProto.ScriptingVob;
            }

            Item weaponIt = null;
            Scripting.Objects.Item weaponScriptItem = null;
            if (weapon != 0)
            {
                weaponIt = (Item)sWorld.VobDict[weapon];
                weaponScriptItem = weaponIt.ScriptingProto;
            }


            Spell spell = null;
            Scripting.Objects.Spell scriptSpell = null;
            if (spellID > 100)
            {
                Spell.SpellDict.TryGetValue(spellID, out spell);
                if (spell != null)
                    scriptSpell = spell.ScriptingProto;

            }

            Scripting.Objects.Character.NPCProto.OnPlayerDamages(vicProto.ScriptingNPC, (DamageType)damageMode, locHit, flydir, attackerScriptProto, weaponMode, scriptSpell, weaponScriptItem, fallDownDistanceY);
            Write(vicProto, (DamageType)damageMode, locHit, flydir, attProto, weaponMode, spell, weaponIt, fallDownDistanceY, packet.guid);
        }

        public static void Write(NPCProto victim, DamageType damageMode, Vec3f hitLoc, Vec3f flyDir, Vob attacker, int weaponMode, Spell spellID, Item weapon, float fallDownDistanceY, AddressOrGUID guidExclude)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.OnDamageMessage);

            byte sendFlags = 0;
            if (hitLoc != null)
                sendFlags |= 1;
            if (flyDir != null)
                sendFlags |= 2;
            if (attacker != null)
                sendFlags |= 4;
            if (weaponMode != 0)
                sendFlags |= 8;
            if (spellID != null)
                sendFlags |= 16;
            if (weapon != null)
                sendFlags |= 32;
            if (fallDownDistanceY >= -float.Epsilon && fallDownDistanceY <= float.Epsilon)
                sendFlags |= 64;

            stream.Write(victim.ID);
            stream.Write((byte)damageMode);
            stream.Write(sendFlags);

            if (hitLoc != null)
                stream.Write(hitLoc);
            if (flyDir != null)
                stream.Write(flyDir);
            if (attacker != null)
                stream.Write(attacker.ID);
            if (weaponMode != 0)
                stream.Write(weaponMode);
            if (spellID != null)
                stream.Write(spellID.ID);
            if (weapon != null)
                stream.Write(weapon.ID);
            if ((sendFlags & 64) == 64)
                stream.Write(fallDownDistanceY);

            if (guidExclude == null)
                guidExclude = RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS;
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guidExclude, true);
        }
    }
}
