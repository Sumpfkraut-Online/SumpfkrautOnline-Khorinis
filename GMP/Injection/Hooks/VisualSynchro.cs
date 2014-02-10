using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using GMP.Modules;
using Gothic.zClasses;
using GMP.Net.Messages;
using Injection;
using Network;

namespace GMP.Injection.Synch
{
    public class VisualSynchro
    {
        public static int oldPlayerAddress;
        public static Int32 ocSetAsPlayer(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            oldPlayerAddress = oCNpc.Player(process).Address;

            oCNpc npc = new oCNpc(process, process.ReadInt(address));
            Program.Player.NPCAddress = npc.Address;

            StaticVars.spawnedPlayerDict.Remove(oldPlayerAddress);
            StaticVars.spawnedPlayerDict.Add(Program.Player.NPCAddress, Program.Player);
            StaticVars.spawnedPlayerList.Sort(new Player.PlayerAddressComparer());
            


            new VisualSynchro_SetAsPlayer().Write(Program.client.sentBitStream, Program.client, npc.ObjectName.Value);

            
            return 0;
        }
    }
}
