using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using System.IO;
using Gothic.Types;
using GUC.Log;

namespace GUC.Hooks.VDFS
{
    static class hFileSystem
    {
        public static void AddHooks()
        {
            var h = Process.AddHook(InitFileSystem, 0x558CC8, 5);
            Process.Nop(5, h.OldInNewAddress);

            h = Process.AddHook(DeinitFileSystem, 0x5593A6, 5);
            Process.Nop(5, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Exists, 0x449020, 6);
            Process.Write(new byte[6] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Open, 0x449120, 6);
            Process.Write(new byte[6] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Close, 0x4493A8, 8);
            Process.Write(new byte[8] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Seek, 0x449490, 6, 1);
            Process.Write(new byte[6] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Size, 0x449410, 5);
            Process.Write(new byte[5] { 0xC3, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Read, 0x44A8D0, 6, 1);
            Process.Write(new byte[6] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

           // h = Process.AddHook(VDFS_ReadSize, 0x44ABF0, 6, 2);
           // Process.Write(new byte[6] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            /*h = Process.AddHook(File_Append, 0x444270, 6);
            h = Process.AddHook(File_ChangeDir, 0x446670, 6, 1);
            h = Process.AddHook(File_Close, 0x444010, 7);
            h = Process.AddHook(File_Create, 0x4439F0, 7);
            h = Process.AddHook(File_DirCreate, 0x4472F0, 7);
            h = Process.AddHook(File_DirExists, 0x447500, 7);
            h = Process.AddHook(File_DirRemove, 0x447400, 7);
            h = Process.AddHook(File_DirStepInto, 0x4468C0, 7, 1);
            h = Process.AddHook(File_Eof, 0x444290, 6);
            h = Process.AddHook(File_Exists, 0x4436B0, 6);*/

            Logger.Log("Added file system hooks.");
        }

        static Dictionary<string, VDFSFileInfo> vFiles = new Dictionary<string, VDFSFileInfo>(64000, StringComparer.OrdinalIgnoreCase);
        static List<VDFSArchive> vArchives = new List<VDFSArchive>(24);

        static void InitFileSystem(Hook hook)
        {
            Logger.Log("Initializing GUC FileSystem...");

            var watch = System.Diagnostics.Stopwatch.StartNew();

            vArchives.Clear();
            vFiles.Clear();

            // Check the project folder for vdfs
            DirectoryInfo folder = new DirectoryInfo(Program.ProjectPath + "\\data");
            if (folder.Exists)
            {
                FileInfo[] archArray = folder.GetFiles("*.vdf");
                for (int i = 0; i < archArray.Length; i++)
                {
                    vArchives.Add(new VDFSArchive(archArray[i], vFiles, true));
                }
            }

            // Check the gothic folder for vdfs
            folder = new DirectoryInfo(Program.GothicPath + "\\data");
            if (folder.Exists)
            {
                FileInfo[] archArray = folder.GetFiles("*.vdf");
                for (int i = 0; i < archArray.Length; i++)
                {
                    vArchives.Add(new VDFSArchive(archArray[i], vFiles, false));
                }
            }

            vArchives.RemoveAll(a => a.FileCount == 0);

            Gothic.System.zFile_VDFS.VDFSInitialized = true; // vArchives.Count > 0;
            watch.Stop();

            Logger.Log("Initialized {0} VDFS-Archives with {1} Files in {2:0}ms.", vArchives.Count, vFiles.Count, watch.Elapsed.TotalMilliseconds);

            Gothic.System.zFile_File.InitFileSystem();
        }

        static void DeinitFileSystem(Hook hook)
        {
            Logger.Log("Deinitializing GUC FileSystem...");
            vFiles.Clear();
            vArchives.Clear();
            foreach (var handle in openedFiles.Values)
                handle.Close();
            openedFiles.Clear();

            Gothic.System.zFile_File.DeinitFileSystem();
        }

        static string GetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            if (path[0] == '\\' || path[0] == '/')
            {
                return path.Substring(1);
            }
            else if (path.StartsWith(Program.GothicPath))
            {
                return path.Substring(Program.GothicPath.Length);
            }
            else if (path.StartsWith(Program.ProjectPath))
            {
                return path.Substring(Program.ProjectPath.Length);
            }
            return path;
        }

        static void VDFS_Exists(Hook hook)
        {
            string path = GetPath(new zString(hook.GetECX() + 0x60).ToString());
            if (!string.IsNullOrWhiteSpace(path))
            {
                if ((vFiles.Count > 0 && vFiles.ContainsKey(path))
                    || File.Exists(Program.GetProjectPath(path))
                    || File.Exists(Program.GetGothicRootPath(path)))
                {
                    hook.SetEAX(1);
                    return;
                }
            }
            hook.SetEAX(0);
        }

        static Dictionary<int, IFileHandle> openedFiles = new Dictionary<int, IFileHandle>(1000);

        static void VDFS_Open(Hook hook)
        {
            int self = hook.GetECX();
            if (openedFiles.ContainsKey(self))
            {
                Logger.Log("File is already opened!");
                hook.SetEAX(0);
                return;
            }

            string path = GetPath(new zString(self + 0x60).ToString());
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.Log("Path is null or white space!");
                hook.SetEAX(0);
                return;
            }

            IFileHandle fileHandle;
            VDFSFileInfo vdfsFileInfo;
            if (vFiles.Count > 0 && vFiles.TryGetValue(path, out vdfsFileInfo))
            {
                fileHandle = new VDFSFileHandle(vdfsFileInfo);
            }
            else
            {
                FileInfo fullPath = new FileInfo(Program.GetProjectPath(path));
                if (fullPath.Exists)
                {
                    fileHandle = new FileHandle(fullPath);
                }
                else
                {
                    fullPath = new FileInfo(Program.GetGothicPath(path));
                    if (fullPath.Exists)
                    {
                        fileHandle = new FileHandle(fullPath);
                    }
                    else
                    {
                        hook.SetEAX(0);
                        return;
                    }
                }
            }

            fileHandle.Open();
            openedFiles.Add(self, fileHandle);
            hook.SetEAX(1);
            Process.Write(true, self + 0x8C);
        }

        static void VDFS_Close(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                handle.Close();
                openedFiles.Remove(self);
                Process.Write(false, self + 0x8C);
            }
        }
        
        static void VDFS_Seek(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (!openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                Logger.LogError("Seek: File is not opened!" + new zString(self + 0x60));
                return;
            }

            handle.Seek(hook.GetArgumentLong(0));
        }

        static void VDFS_Size(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (!openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                Logger.LogError("Size: File is not opened!" + new zString(self + 0x60));
                return;
            }

            //hook.SetEDX((int)(handle.GetSize() >> 32));
            hook.SetEAX((int)handle.GetSize());
        }
        
        static void VDFS_Read(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (!openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                Logger.LogError("Read: File is not opened!");
                return;
            }

            hook.SetEAX(handle.Read(hook.GetArgument(0), 1));
        }

        static void VDFS_ReadSize(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (!openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                Logger.LogError("File is not opened!");
                return;
            }

            hook.SetEAX(handle.Read(hook.GetArgument(0), 1));
        }

        /*static void File_Append(Hook hook)
        {
            Logger.Log("File Append.");
        }

        static void File_ChangeDir(Hook hook)
        {
            Logger.Log("File ChangeDir. " + hook.GetArgument(0));
        }

        static void File_Close(Hook hook)
        {
            Logger.Log("File Close.");
        }

        static void File_Create(Hook hook)
        {
            Logger.Log("File Create.");
        }

        static void File_DirCreate(Hook hook)
        {
            Logger.Log("File DirCreate.");
        }

        static void File_DirExists(Hook hook)
        {
            Logger.Log("File DirExists.");
        }

        static void File_DirRemove(Hook hook)
        {
            Logger.Log("File DirRemove.");
        }

        static void File_DirStepInto(Hook hook)
        {
            Logger.Log("File DirStepInto. " + new zString(hook.GetArgument(0)));
        }

        static void File_Eof(Hook hook)
        {
            Logger.Log("File Eof.");
        }

        static void File_Exists(Hook hook)
        {
            Logger.Log("File Exists.");
        }*/
    }
}
