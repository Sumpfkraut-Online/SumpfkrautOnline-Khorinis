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
    class SpellInvestMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0;
            Vob playerVob = null;
            NPCProto player = null;

            stream.Read(out playerID);

            sWorld.VobDict.TryGetValue(playerID, out playerVob);
            if (playerVob == null)
                throw new Exception("Vob was not found: "+playerID);
            if (!(playerVob is NPCProto))
                throw new Exception("Vob was not a NPCProto: "+playerID+" "+playerVob);

            player = (NPCProto)playerVob;

            if (player.Address == 0)
                return;
            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, player.Address);
            if (player.FocusVob != null && player.FocusVob.Address != 0)
                npc.MagBook.Spell_Setup(npc, new zCVob(process, player.FocusVob.Address), 0);
            npc.MagBook.Spell_Invest();

            zERROR.GetZErr(process).Report(2, 'G', "Invest Spell! ", 0, "Program.cs", 0);
        }
    }
}
