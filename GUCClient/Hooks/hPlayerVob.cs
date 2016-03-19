using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Objects;
using GUC.Log;

namespace GUC.Client.Hooks
{
    static class hPlayerVob
    {
        public static void AddHooks()
        {
            // hook hero creating
            Process.Write(new byte[] { 0xE9, 0xBD, 0x00, 0x00, 0x00 }, 0x006C434B);
            var hi = Process.Hook(Program.GUCDll, typeof(hPlayerVob).GetMethod("CreatePlayerVob"), 0x006C440D, 5, 0);
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }, hi.oldFuncInNewFunc.ToInt32());
            Process.Write(new byte[] { 0xEB, 0x67 }, 0x006C4662); //skip instance check

            Logger.Log("Added player hero creation hooks.");
        }

        public static Int32 CreatePlayerVob(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);

                oCNpc player = oCNpc.Create();
                //player.SetVisual("HUMANS.MDS");
                //player.SetAdditionalVisuals("hum_body_Naked0", 9, 0, "Hum_Head_Pony", 2, 0, -1);
                player.SetVisual("Scavenger.mds");
                player.SetAdditionalVisuals("Sca_Body", 0, 0, "", 0, 0, -1);
                player.SetToFistMode();

                player.HPMax = 10;
                player.HP = 10;

                Process.Write(player.Address, address + 4); //write address into eax
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }
    }
}
