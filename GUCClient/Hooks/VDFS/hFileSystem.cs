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
            /*Process.AddHook(h => Logger.Log("Exists " + new zString(h.GetECX() + 0x60)), 0x449020, 6);

            Process.AddHook(h => Logger.Log("Open " + new zString(h.GetECX() + 0x60)), 0x449120, 6);

            Process.AddHook(h => Logger.Log("Close "), 0x4493A8, 5);

            Process.AddHook(h => Logger.Log("Read 1 "+ new zString(h.GetECX() + 0x60)), 0x44A8D0, 6, 1);
            Process.AddHook(h => Logger.Log("ReadSize " + h.GetArgument(1) + " " + new zString(h.GetECX() + 0x60)), 0x44ABF0, 6, 2);

            Process.AddHook(h => Logger.Log("Search " + new zString(h.GetArgument(0))), 0x449E80, 6, 3);

            Process.AddHook(h => Logger.Log("Destruct " + new zString(h.GetECX() + 0x60)), 0x448ED0, 7);*/

            var h = Process.AddHook(InitFileSystem, 0x558CC8, 5);
            Process.Nop(5, h.OldInNewAddress);

            h = Process.AddHook(DeinitFileSystem, 0x5593A6, 5);
            Process.Nop(5, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Exists, 0x449020, 6);
            Process.Write(new byte[6] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Open, 0x449120, 6);
            Process.Write(new byte[6] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Close, 0x4493A0, 8);
            Process.Write(new byte[8] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Seek, 0x449490, 6, 1);
            Process.Write(new byte[6] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Pos, 0x449A50, 5);
            Process.Write(new byte[5] { 0xC3, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Size, 0x449410, 5);
            Process.Write(new byte[5] { 0xC3, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Read, 0x44A8D0, 6, 1);
            Process.Write(new byte[6] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_ReadSize, 0x44ABF0, 6, 2);
            Process.Write(new byte[6] { 0xC2, 0x08, 0x00, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_SearchFile, 0x449E80, 6, 3);
            Process.Write(new byte[6] { 0xC2, 0x0C, 0x00, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            h = Process.AddHook(VDFS_Close, 0x448ED0, 7); // zFile_VDFS Destructor*/

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

            //System.Threading.Thread.Sleep(15000);
        }

        static Dictionary<string, VDFSDirectoryInfo> vDirs { get { return VDFSArchive.vDirs; } }
        static Dictionary<string, VDFSFileInfo> vFiles { get { return VDFSArchive.vFiles; } }

        static void InitFileSystem(Hook hook)
        {
            Logger.Log("Initializing GUC FileSystem...");

            long m1 = GC.GetTotalMemory(true);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            vFiles.Clear();

            // Check the project folder for vdfs
            DirectoryInfo folder = new DirectoryInfo(Program.ProjectPath + "\\data");
            if (folder.Exists)
            {
                FileInfo[] archArray = folder.GetFiles("*.vdf");
                for (int i = 0; i < archArray.Length; i++)
                {
                    new VDFSArchive(archArray[i], true);
                }
            }

            // Check the gothic folder for vdfs
            folder = new DirectoryInfo(Program.GothicPath + "\\data");
            if (folder.Exists)
            {
                FileInfo[] archArray = folder.GetFiles("*.vdf");
                for (int i = 0; i < archArray.Length; i++)
                {
                    new VDFSArchive(archArray[i], false);
                }
            }

            Gothic.System.zFile_VDFS.VDFSInitialized = true;
            watch.Stop();

            long m2 = GC.GetTotalMemory(true);

            Logger.Log("Initialized {0} VDFS-Files in {1:0}ms. {2}kB", vFiles.Count, watch.Elapsed.TotalMilliseconds, (m2 - m1) / 1000);

            Gothic.System.zFile_File.InitFileSystem();
        }

        static void DeinitFileSystem(Hook hook)
        {
            Logger.Log("Deinitializing GUC FileSystem...");
            vFiles.Clear();
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

        static string GetPathFolder(string path)
        {
            path = GetPath(path);
            if (path[path.Length - 1] == '\\' || path[path.Length - 1] == '/')
            {
                return path.Remove(path.Length - 1);
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
                    Logger.Log("Exists: " + path);
                    hook.SetEAX(1);
                    return;
                }
            }
            Logger.Log("Not Existing: " + path);
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
                hook.SetEAX(0x13F2);
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
                    fullPath = new FileInfo(Program.GetGothicRootPath(path));
                    
                    if (fullPath.Exists)
                    {
                        fileHandle = new FileHandle(fullPath);
                    }
                    else
                    {
                        Logger.Log("Open: File not found! '" + fullPath + "'");
                        hook.SetEAX(0x13F2);
                        return;
                    }
                }
            }

            fileHandle.Open();
            openedFiles.Add(self, fileHandle);
            hook.SetEAX(0);
            Process.Write(true, self + 0x29FC);
            Process.Write(true, self + 0x8C);
            Logger.Log("Open " + path);
        }

        static void VDFS_Close(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                handle.Close();
                openedFiles.Remove(self);
                Process.Write(false, self + 0x29FC);
                Process.Write(false, self + 0x8C);
                Logger.Log("Close");
            }
            //hook.SetEAX(0);
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

            long pos = (long)hook.GetArgument(0);// | ((long)hook.GetArgument(1) << 32);
            Logger.Log("Seek: " + pos);
            handle.Seek(pos);
            hook.SetEAX(0);
        }

        static void VDFS_Pos(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (!openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                Logger.LogError("Pos: File is not opened!" + new zString(self + 0x60));
                return;
            }

            long pos = handle.GetPos();
            Logger.Log("Pos: " + pos);
            //hook.SetEDX((int)(pos >> 32));
            hook.SetEAX((int)pos);
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

            long size = handle.GetSize();
            Logger.Log("Size: " + size);
            //hook.SetEDX((int)(size >> 32));
            hook.SetEAX((int)size);
        }

        // Reads until 0x0A
        static void VDFS_Read(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (!openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                Logger.LogError("Read: File is not opened!");
                return;
            }

            int index = 0;
            byte[] buf = new byte[400];
            while (index < 399)
            {
                if (handle.Read(buf, index, 1) == 0)
                    break;

                if (buf[index] == 0x0A)
                    break;

                index++;
            }
            
            Logger.Log("ReadString");

            Process.Write(buf, index + 1, hook.GetArgument(0));
            hook.SetEAX(0);
        }

        static void VDFS_ReadSize(Hook hook)
        {
            int self = hook.GetECX();

            IFileHandle handle;
            if (!openedFiles.TryGetValue(hook.GetECX(), out handle))
            {
                Logger.LogError("ReadSize: File is not opened!");
                return;
            }

            int ptr = hook.GetArgument(0);
            long count = (long)hook.GetArgument(1);// | ((long)hook.GetArgument(2) << 32);
            Logger.Log("Read: " + count);

            byte[] buffer = new byte[count];
            int read = handle.Read(buffer, 0, (int)count);
            if (read > 0)
                Process.Write(buffer, read, ptr);

            hook.SetEAX(0);
        }

        static void VDFS_SearchFile(Hook hook)
        {
            int self = hook.GetECX();

            string fileName = new zString(hook.GetArgument(0)).ToString();
            string folder = GetPathFolder(new zString(hook.GetArgument(1)).ToString());
            int arg3 = hook.GetArgument(2);

            Logger.Log("SearchFile: " + fileName + " " + folder + " " + arg3);
            if (vFiles.Count > 0)
            {
                VDFSDirectoryInfo dir;
                if (vDirs.TryGetValue(folder, out dir))
                {
                    VDFSFileInfo fi = dir.SearchFile(fileName);
                    if (fi != null)
                    {
                        new zString(hook.GetECX() + 0x60).Set(fi.Path);
                        hook.SetEAX(0);
                        return;
                    }
                }
            }

            DirectoryInfo dirInfo = new DirectoryInfo(Program.GetProjectPath(folder));
            if (dirInfo.Exists)
            {
                FileInfo fi = dirInfo.EnumerateFiles(fileName, SearchOption.AllDirectories).FirstOrDefault();
                if (fi != null)
                {
                    new zString(hook.GetECX() + 0x60).Set(GetPath(fi.FullName));
                    hook.SetEAX(0);
                    return;
                }
            }

            dirInfo = new DirectoryInfo(Program.GetGothicPath(folder));
            if (dirInfo.Exists)
            {
                FileInfo fi = dirInfo.EnumerateFiles(fileName, SearchOption.AllDirectories).FirstOrDefault();
                if (fi != null)
                {
                    new zString(hook.GetECX() + 0x60).Set(GetPath(fi.FullName));
                    hook.SetEAX(0);
                    return;
                }
            }

            hook.SetEAX(0);
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
