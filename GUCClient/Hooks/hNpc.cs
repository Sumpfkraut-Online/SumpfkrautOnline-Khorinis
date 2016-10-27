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

            var emptyBuf = Enumerable.Repeat<byte>(0x90, 0xB).ToArray();

            var hook = Process.AddHook(RbtForwardHook, 0x687B71, 0xB);
            Process.Write(emptyBuf, hook.OldInNewAddress);


            hook = Process.AddHook(RbtStandHook, 0x683BEA, 0xB);
            Process.Write(emptyBuf, hook.OldInNewAddress);

            hook = Process.AddHook(RbtStandHook, 0x686AF8, 0xB);
            Process.Write(emptyBuf, hook.OldInNewAddress);

            hook = Process.AddHook(RbtStandHook, 0x686B62, 0xB);
            Process.Write(emptyBuf, hook.OldInNewAddress);

            Logger.Log("Added npc hooks.");
        }

        static void RbtForwardHook(Hook hook)
        {
            int npcAddr = hook.GetESI();

            NPC npc;
            if (World.Current.TryGetVobByAddress(npcAddr, out npc))
            {
                //npc.DoSetState(MoveState.Forward);
            }
        }

        static void RbtStandHook(Hook hook)
        {
            int npcAddr = hook.GetESI();

            NPC npc;
            if (World.Current.TryGetVobByAddress(npcAddr, out npc))
            {
                //npc.DoSetState(MoveState.Stand);
            }
        }
    }
}
