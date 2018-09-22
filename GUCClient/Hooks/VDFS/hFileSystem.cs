using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApiNew.Hooks;
using System.IO;
using GUC.Log;

namespace GUC.Hooks.VDFS
{
    static partial class hFileSystem
    {
        public static void AddHooks()
        {
            ReplacerHook.CDECL(InitFileSystem, 0x558CC9).Inject();
            ReplacerHook.CDECL(DeinitFileSystem, 0x5593A7).Inject();

            AddHooks_Virtual();
            AddHooks_Physical();
            Logger.Log("Added VDFS file system hooks.");
        }

        static Dictionary<string, VDFSDirectoryInfo> vDirs { get { return VDFSArchive.vDirs; } }
        static Dictionary<string, VDFSFileInfo> vFiles { get { return VDFSArchive.vFiles; } }

        static int InitFileSystem()
        {
            try
            {
                Logger.Log("Initializing GUC FileSystem...");

                long m1 = GC.GetTotalMemory(true);
                var watch = System.Diagnostics.Stopwatch.StartNew();

                vDirs.Clear();
                vFiles.Clear();
                // Check the project folder for vdfs
                DirectoryInfo folder = new DirectoryInfo(Program.ProjectPath + "\\data");
                if (folder.Exists)
                {
                    foreach (FileInfo archive in folder.EnumerateFiles(".vdf", ".mod"))
                    {
                        new VDFSArchive(archive, true);
                    }
                }

                // Check the gothic folder for vdfs
                folder = new DirectoryInfo(Program.GothicPath + "\\data");
                if (folder.Exists)
                {
                    foreach (FileInfo archive in folder.EnumerateFiles(".vdf", ".mod"))
                    {
                        new VDFSArchive(archive, false);
                    }
                }

                Gothic.System.zFile_VDFS.VDFSInitialized = true;

                watch.Stop();
                long m2 = GC.GetTotalMemory(true);

                Logger.Log("Initialized {0} VDFS-Files in {1:0}ms. {2}kB", vFiles.Count, watch.Elapsed.TotalMilliseconds, (m2 - m1) / 1000);

                Gothic.System.zFile_File.InitFileSystem();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }

            return 1;
        }

        static int DeinitFileSystem()
        {
            try
            {
                Logger.Log("Deinitializing GUC FileSystem...");
                vDirs.Clear();
                vFiles.Clear();
                foreach (var handle in openedFiles.Values)
                    handle.Close();
                openedFiles.Clear();

                Gothic.System.zFile_File.DeinitFileSystem();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
            return 1;
        }

        static Dictionary<int, IFileHandle> openedFiles = new Dictionary<int, IFileHandle>(1000);

    }
}
