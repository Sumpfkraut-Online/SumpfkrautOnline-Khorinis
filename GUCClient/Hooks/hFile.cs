using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using WinApi;
using System.IO;

namespace GUC.Hooks
{
    static class hFile
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            var hook = Process.AddHook(AccessHook, 0x7D197A, 5, 1);
            Process.Write(new byte[5] { 0xC3, 0x90, 0x90, 0x90, 0x90 }, hook.OldInNewAddress);

            Logger.Log("Added file system hooks.");
        }

        static List<byte> str = new List<byte>(byte.MaxValue);
        static void AccessHook(Hook hook)
        {
            int addr = hook.GetArgument(0);

            byte readByte;
            while ((readByte = Process.ReadByte(addr++)) > 0)
            {
                str.Add(readByte);
            }

            string fileName = Encoding.ASCII.GetString(str.ToArray());
            str.Clear();

            if (File.Exists(fileName) || File.Exists(Program.GetFullPath(fileName)))
            {
                hook.SetEAX(0);
            }
            else
            {
                hook.SetEAX(-1);
            }
        }
    }
}
