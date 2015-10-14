using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.PlayerCommands
{
    class CastSpellMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
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

            NPC caster = null;
            Vob target = null;

            sWorld.VobDict.TryGetValue(casterID, out casterVob);
            sWorld.VobDict.TryGetValue(itemID, out itemVob);

            if (casterVob == null)
                throw new Exception("Caster was not found!");
            if (!(casterVob is NPC))
                throw new Exception("Caster was not a npcproto " + casterVob);
            caster = (NPC)casterVob;
            if (targetID != 0)
            {
                sWorld.VobDict.TryGetValue(targetID, out target);
            }

            Spell.SpellDict.TryGetValue(spellID, out spell);
            if (spell == null)
                throw new Exception("Spell can not be null!");




            if (caster.Address == 0)
                return;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, caster.Address);
            if (target != null)
                npc.MagBook.Spell_Setup(npc, new zCVob(process, target.Address), 0);
                //npc.MagBook.GetSelectedSpell().Target = new zCVob(process, target.Address);
            npc.MagBook.SpellCast();

            zERROR.GetZErr(process).Report(2, 'G', "Cast Spell! 2 ", 0, "Program.cs", 0);
        }
    }
}
