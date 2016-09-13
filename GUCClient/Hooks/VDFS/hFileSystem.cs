using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using System.IO;
using Gothic.Types;

namespace GUC.Hooks.VDFS
{
    static class hFileSystem
    {
        public static void AddHooks()
        {
            var h = Process.AddHook(InitFileSystem, 0x558CC8, 5);
            //Process.Nop(5, h.OldInNewAddress);

            h = Process.AddHook(InitFileSystem, 0x1593A6, 5);
            //Process.Nop(5, h.OldInNewAddress);


            h = Process.AddHook(File_Append, 0x444270, 6);
            h = Process.AddHook(File_ChangeDir, 0x446670, 6, 1);
            h = Process.AddHook(File_Close, 0x444010, 7);
            h = Process.AddHook(File_Create, 0x4439F0, 7);
            h = Process.AddHook(File_DirCreate, 0x4472F0, 7);
            h = Process.AddHook(File_DirExists, 0x447500, 7);
            h = Process.AddHook(File_DirRemove, 0x447400, 7);
            h = Process.AddHook(File_DirStepInto, 0x4468C0, 7, 1);
            h = Process.AddHook(File_Eof, 0x444290, 6);
            h = Process.AddHook(File_Exists, 0x4436B0, 6);
        }

        static void InitFileSystem(Hook hook)
        {
            Log.Logger.Log("Init GUC FileSystem.");
        }

        static void DeinitFileSystem(Hook hook)
        {
            Log.Logger.Log("Deinit GUC FileSystem.");
        }



        static void File_Append(Hook hook)
        {
            Log.Logger.Log("File Append.");
        }

        static void File_ChangeDir(Hook hook)
        {
            Log.Logger.Log("File ChangeDir. " + hook.GetArgument(0));
        }

        static void File_Close(Hook hook)
        {
            Log.Logger.Log("File Close.");
        }

        static void File_Create(Hook hook)
        {
            Log.Logger.Log("File Create.");
        }

        static void File_DirCreate(Hook hook)
        {
            Log.Logger.Log("File DirCreate.");
        }

        static void File_DirExists(Hook hook)
        {
            Log.Logger.Log("File DirExists.");
        }

        static void File_DirRemove(Hook hook)
        {
            Log.Logger.Log("File DirRemove.");
        }

        static void File_DirStepInto(Hook hook)
        {
            Log.Logger.Log("File DirStepInto. " + new zString(hook.GetArgument(0)));
        }

        static void File_Eof(Hook hook)
        {
            Log.Logger.Log("File Eof.");
        }

        static void File_Exists(Hook hook)
        {
            Log.Logger.Log("File Exists.");
        }
    }
}
