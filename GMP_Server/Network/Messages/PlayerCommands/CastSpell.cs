using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class CastSpell : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int casterID = 0, targetID = 0, spellID = 0, itemID = 0;

            stream.Read(out itemID);
            stream.Read(out casterID);
            stream.Read(out targetID);
            stream.Read(out spellID);


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


            
            Scripting.Objects.Character.NPCProto.isOnCastSpell(
                caster.ScriptingNPC, spell.ScriptingProto, sT);

            
        }
    }
}
