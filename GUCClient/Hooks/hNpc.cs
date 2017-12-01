using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using WinApi;
using GUC.WorldObjects;
using GUC.Types;

namespace GUC.Hooks
{
    static class hNpc
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            Process.AddHook(RbtForwardHook, 0x687B71, 6);
            Process.AddHook(RbtStandHook, 0x683BEA, 6);
            Process.AddHook(RbtStandHook, 0x686AF8, 6);
            Process.AddHook(RbtStandHook, 0x686B62, 6);

            Logger.Log("Added npc hooks.");
        }

        static void RbtForwardHook(Hook hook, RegisterMemory rmem)
        {
            int npcAddr = rmem[Registers.ESI];

            if (World.Current.TryGetVobByAddress(npcAddr, out NPC npc) && npc.Guide == Network.GameClient.Client)
            {
                npc.ScriptObject.SetMovement(NPCMovement.Forward);
            }
        }

        static void RbtStandHook(Hook hook, RegisterMemory rmem)
        {
            int npcAddr = rmem[Registers.ESI];

            if (World.Current.TryGetVobByAddress(npcAddr, out NPC npc) && npc.Guide == Network.GameClient.Client)
            {
                npc.ScriptObject.SetMovement(NPCMovement.Stand);
            }
        }
    }
}
