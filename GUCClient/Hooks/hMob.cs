using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApiNew;
using GUC.WorldObjects;
using GUC.WorldObjects.Mobs;
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
            
            if (World.Current.TryGetVobByAddress(self, out Mob mob) && mob.FocusName != null && mob.FocusName.Length > 0)
            {
                byte[] bytes = Encoding.Default.GetBytes(mob.FocusName);
                int charPtr = Process.Alloc(bytes.Length + 1);

                Process.WriteBytes(charPtr, bytes);
                Process.WriteByte(charPtr + bytes.Length, 0); // end '\0'
                
                WinApi.Process.THISCALL<WinApi.NullReturnCall>(arg, 0x004010C0, (WinApi.IntArg)charPtr);
                Process.Free(charPtr, bytes.Length + 1);
            }
            else
            {
                WinApi.Process.THISCALL<WinApi.NullReturnCall>(arg, 0x004010C0, (WinApi.IntArg)0);
            }
        }
    }
}
