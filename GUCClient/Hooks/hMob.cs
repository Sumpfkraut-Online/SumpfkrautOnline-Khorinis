using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApiNew;
using GUC.WorldObjects;
using GUC.Log;
using Gothic.Types;

namespace GUC.Hooks
{
    static class hMob
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited)
                return;
            inited = true;

            Process.AddFastHook(GetName, 0x71BC30, 0x5).SetOriCodeReturn(1);
            Logger.Log("Added Mob hooks.");
        }

        static void GetName(RegisterMemory mem)
        {
            int self = mem.ECX;
            int arg = mem.GetArg(0);

            byte[] arr;
            if (World.Current.TryGetVobByAddress(self, out GUCMobInst mob) && mob.FocusName != null && mob.FocusName.Length > 0)
            {
                arr = Encoding.Default.GetBytes(mob.FocusName);
            }
            else
            {
                arr = new byte[0];
            }
            
            int charArr = Process.Alloc(arr.Length + 1);

            if (arr.Length > 0)
            {
                Process.WriteBytes(charArr, arr);
            }
            Process.WriteByte(charArr + arr.Length, 0);

            WinApi.Process.THISCALL<zString>(arg, 0x004010C0, (WinApi.IntArg)charArr);
            Process.Free(charArr, arr.Length + 1);
        }
    }
}
