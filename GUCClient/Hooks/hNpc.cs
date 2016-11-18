using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using WinApi;
using GUC.WorldObjects;

namespace GUC.Hooks
{
    static class hNpc
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            Process.AddHook(RbtForwardHook, 0x687B71, 0xB).SetSkipOldCode(true);
            Process.AddHook(RbtStandHook, 0x683BEA, 0xB).SetSkipOldCode(true);
            Process.AddHook(RbtStandHook, 0x686AF8, 0xB).SetSkipOldCode(true);
            Process.AddHook(RbtStandHook, 0x686B62, 0xB).SetSkipOldCode(true);

            Logger.Log("Added npc hooks.");
        }

        static void RbtForwardHook(Hook hook, RegisterMemory rmem)
        {
            int npcAddr = rmem[Registers.ESI];

            NPC npc;
            if (World.Current.TryGetVobByAddress(npcAddr, out npc))
            {
                //npc.DoSetState(MoveState.Forward);
            }
        }

        static void RbtStandHook(Hook hook, RegisterMemory rmem)
        {
            int npcAddr = rmem[Registers.ESI];

            NPC npc;
            if (World.Current.TryGetVobByAddress(npcAddr, out npc))
            {
                //npc.DoSetState(MoveState.Stand);
            }
        }
    }
}
