using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Enumeration;
using RakNet;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class CastSpell : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int casterID = 0, targetID = 0, spellID = 0, itemID = 0, castLevel = 0;

            stream.Read(out itemID);
            stream.Read(out casterID);
            stream.Read(out targetID);
            stream.Read(out spellID);
            stream.Read(out castLevel);

            Vob itemVob = null;
            Item item = null;

            Vob casterVob = null;
            Spell spell = null;

            NPCProto caster = null;
            Vob target = null;

            sWorld.VobDict.TryGetValue(casterID, out casterVob);
            sWorld.VobDict.TryGetValue(itemID, out itemVob);

            if (casterVob == null)
                throw new Exception("Caster was not found!");
            if (!(casterVob is NPCProto))
                throw new Exception("Caster was not a npcproto "+casterVob);
            caster = (NPCProto)casterVob;
            if (targetID != 0)
            {
                sWorld.VobDict.TryGetValue(targetID, out target);
            }
            
            Spell.SpellDict.TryGetValue(spellID, out spell);
            if (spell == null)
                throw new Exception("Spell can not be null!");


            Scripting.Objects.Vob sT = (target == null) ? null : target.ScriptingVob;


            if (itemVob != null)
            {
                item = (Item)itemVob;
                if(item.ItemInstance.Flags.HasFlag(Flags.ITEM_MULTI))
                    item.Amount -= 1;
            }

            int manaInvested = 0;
            int realLevel = (spell.processMana.Length > castLevel + 1) ? castLevel : spell.processMana.Length - 1;
            for (int i = 0; i <= realLevel; i++)
                manaInvested += spell.processMana[i];
            caster.ScriptingNPC.MP -= manaInvested;
            
            Scripting.Objects.Character.NPCProto.isOnCastSpell(
                caster.ScriptingNPC, spell.ScriptingProto, sT);



            Write(caster, item, target, spell, packet.guid);
        }


        public static void Write(NPCProto proto, Item itm, Vob target, Spell spell)
        {
            Write(proto, itm, target, spell, null);
        }
        public static void Write(NPCProto proto, Item itm, Vob target, Spell spell, AddressOrGUID guidExclude)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.CastSpell);

            stream.Write(itm.ID);
            stream.Write(proto.ID);
            if (target == null)
                stream.Write(0);
            else
                stream.Write(target.ID);
            if (spell == null)
                stream.Write(0);
            else
                stream.Write(spell.ID);

            if (guidExclude == null)
                guidExclude = RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS;
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guidExclude, true);
        }

    }
}
