using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Objects;
using GUC.Log;

namespace GUC.Hooks
{
    static class hPlayerVob
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            // hook hero creating
            Process.Write(0x006C434B, 0xE9, 0xBD, 0x00, 0x00, 0x00);

            Process.AddHook(CreatePlayerVob, 0x006C440D, 5).SetSkipOldCode(true);
            Process.Write(0x006C4662, 0xEB, 0x67); //skip instance check

            Logger.Log("Added player hero creation hooks.");
        }

        static void CreatePlayerVob(Hook hook, RegisterMemory rmem)
        {
            try
            {
                oCNpc player = oCNpc.Create();
                //player.SetVisual("HUMANS.MDS");
                //player.SetAdditionalVisuals("hum_body_Naked0", 9, 0, "Hum_Head_Pony", 2, 0, -1);
                player.SetVisual("Scavenger.mds");
                player.SetAdditionalVisuals("Sca_Body", 0, 0, "", 0, 0, -1);
                player.SetToFistMode();

                player.HPMax = 10;
                player.HP = 10;

                rmem[Registers.EAX] = player.Address;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
