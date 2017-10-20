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
            Hook h;
            h = Process.AddHook(InitFileSystem, 0x558CC8, 5);
            Process.Nop(h.OldInNewAddress, 5);

            h = Process.AddHook(DeinitFileSystem, 0x5593A6, 5);
            Process.Nop(h.OldInNewAddress, 5);

            h = Process.AddHook(VDFS_Exists, 0x449020, 6);
            Process.Write(h.OldInNewAddress, 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90);

            h = Process.AddHook(VDFS_Open, 0x449120, 6);
            Process.Write(h.OldInNewAddress, 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90);

            h = Process.AddHook(VDFS_Close, 0x4493A0, 8);
            Process.Write(h.OldInNewAddress, 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90);

            h = Process.FastHook(VDFS_Seek, 0x449490, 6);
            Process.Write(h.OldInNewAddress, 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90);

            h = Process.FastHook(VDFS_Pos, 0x449A50, 5);
            Process.Write(h.OldInNewAddress, 0xC3, 0x90, 0x90, 0x90, 0x90);

            h = Process.FastHook(VDFS_EOF, 0x449470, 5);
            Process.Write(h.OldInNewAddress, 0xC3, 0x90, 0x90, 0x00, 0x90);

            h = Process.FastHook(VDFS_Size, 0x449410, 5);
            Process.Write(h.OldInNewAddress, 0xC3, 0x90, 0x90, 0x90, 0x90);

            h = Process.FastHook(VDFS_ReadString, 0x44A8D0, 6);
            Process.Write(h.OldInNewAddress, 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90);
           
            h = Process.FastHook(VDFS_ReadSize, 0x44ABF0, 6);
            Process.Write(h.OldInNewAddress, 0xC2, 0x08, 0x00, 0x90, 0x90, 0x90);

            h = Process.AddHook(VDFS_SearchFile, 0x449E80, 6);
            Process.Write(h.OldInNewAddress, 0xC2, 0x0C, 0x00, 0x90, 0x90, 0x90);

            h = Process.AddHook(VDFS_Close, 0x448ED0, 7); // zFile_VDFS Destructor

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

            Logger.Log("Added VDFS file system hooks.");
        }

        static Dictionary<string, VDFSDirectoryInfo> vDirs { get { return VDFSArchive.vDirs; } }
        static Dictionary<string, VDFSFileInfo> vFiles { get { return VDFSArchive.vFiles; } }

        static void InitFileSystem(Hook hook, RegisterMemory mem)
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

        static void DeinitFileSystem(Hook hook, RegisterMemory mem)
        {
            Logger.Log("Deinitializing GUC FileSystem...");
            vDirs.Clear();
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

        static string GetZFilePath(int address)
        {
            string path = new zString(address + 0x60).ToString();
            if (path.Length > 0 && path[0] != '\\') // ayy lmao
            {
                return Gothic.System.zFile.s_virtPathString + path;
            }
            return path;
        }

        static void VDFS_Exists(Hook hook, RegisterMemory mem)
        {
            string path = GetPath(GetZFilePath(mem[Registers.ECX]));

            if (!string.IsNullOrWhiteSpace(path))
            {
                if ((vFiles.Count > 0 && vFiles.ContainsKey(path))
                    || File.Exists(Program.GetProjectPath(path))
                    || File.Exists(Program.GetGothicRootPath(path)))
                {
                    //Logger.Log("Exists: " + path);
                    mem[Registers.EAX] = 1;
                    return;
                }
            }
            //Logger.Log("Not Existing: " + path);
            mem[Registers.EAX] = 0;
        }

        static Dictionary<int, IFileHandle> openedFiles = new Dictionary<int, IFileHandle>(1000);
        
        static void VDFS_Open(Hook hook, RegisterMemory mem)
        {
            int self = mem[Registers.ECX];
            if (openedFiles.ContainsKey(self))
            {
                Logger.Log("File is already opened!");
                mem[Registers.EAX] = 0;
                return;
            }

            string path = GetPath(GetZFilePath(self));
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.Log("Path is null or white space!");
                mem[Registers.EAX] = 0x13F2;
                return;
            }

            IFileHandle fileHandle;
            if (vFiles.Count > 0 && vFiles.TryGetValue(path, out VDFSFileInfo vdfsFileInfo))
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
                        mem[Registers.EAX] = 0x13F2;
                        return;
                    }
                }
            }

            fileHandle.Open();
            openedFiles.Add(self, fileHandle);
            mem[Registers.EAX] = 0;
            Process.Write(self + 0x29FC, true);
            Process.Write(self + 0x8C, true);
            //Logger.Log("Open " + self.ToString("X4") + " " + path);
        }

        static void VDFS_Close(Hook hook, RegisterMemory mem)
        {
            int self = mem[Registers.ECX];

            IFileHandle handle;
            if (openedFiles.TryGetValue(self, out handle))
            {
                handle.Close();
                openedFiles.Remove(self);
                Process.Write(self + 0x29FC, false);
                Process.Write(self + 0x8C, false);
                //Logger.Log("Close " + self.ToString("X4"));
            }
            //hook.SetEAX(0);
        }

        static void VDFS_Seek(Process.HookStruct mem)
        {
            int self = mem.ECX;

            IFileHandle handle;
            if (!openedFiles.TryGetValue(self, out handle))
            {
                Logger.LogError("Seek: File is not opened!" + new zString(self + 0x60));
                return;
            }

            long pos = mem.GetArgument(0);// | ((long)hook.GetArgument(1) << 32);
                              // Logger.Log("Seek: " + pos + " " + self.ToString("X4"));

            handle.Seek(pos);
            mem.EAX = 0;
        }

        static void VDFS_EOF(Process.HookStruct mem)
        {
            int self = mem.ECX;

            IFileHandle handle;
            if (!openedFiles.TryGetValue(self, out handle))
            {
                Logger.LogError("EOF: File is not opened!" + new zString(self + 0x60));
                return;
            }
            // Logger.Log("EOF: " + self.ToString("X4"));

            mem.EAX = handle.GetSize() - handle.GetPos() <= 0 ? 1 : 0;
        }

        static void VDFS_Pos(Process.HookStruct mem)
        {
            int self = mem.ECX;

            IFileHandle handle;
            if (!openedFiles.TryGetValue(self, out handle))
            {
                Logger.LogError("Pos: File is not opened!" + new zString(self + 0x60));
                return;
            }

            long pos = handle.GetPos();
            //Logger.Log("Pos: " + self.ToString("X4") + " " + pos);
            //hook.SetEDX((int)(pos >> 32));
            mem.EAX = (int)pos;
        }

        static void VDFS_Size(Process.HookStruct mem)
        {
            int self = mem.ECX;

            IFileHandle handle;
            if (!openedFiles.TryGetValue(self, out handle))
            {
                Logger.LogError("Size: File is not opened!" + new zString(self + 0x60));
                return;
            }

            long size = handle.GetSize();
            //Logger.Log("Size: " + size);
            //hook.SetEDX((int)(size >> 32));
            mem.EAX = (int)size;
        }


        static byte[] Buffer = new byte[10000];
        // Reads until 0x0A
        static void VDFS_ReadString(Process.HookStruct mem)
        {
            int self = mem.ECX;

            IFileHandle handle;
            if (!openedFiles.TryGetValue(self, out handle))
            {
                Logger.LogError("Read: File is not opened!");
                return;
            }

            int index = 0;
            while (index < 399) // it's like this in gothic
            {
                if (handle.Read(Buffer, index, 1) == 0)
                    break;

                if (Buffer[index] == 0x0A) // \n break
                    break;

                index++;
            }
            index++;

            Buffer[index] = 0;

            int ptr = mem.GetArgument(0);
            if (ptr == 0)
                Logger.LogError("ReadString ptr is 0!");

            Process.Write(ptr, Buffer, index + 1);
            mem.EAX = 1;
        }

        static void VDFS_ReadSize(Process.HookStruct mem)
        {
            int self = mem.ECX;

            IFileHandle handle;
            if (!openedFiles.TryGetValue(self, out handle))
            {
                Logger.LogError("ReadSize: File is not opened!");
                return;
            }
            
            int ptr = mem.GetArgument(0);
            if (ptr == 0)
                Logger.LogError("ReadSize ptr is 0!");

            long count = (long)mem.GetArgument(1);// | ((long)hook.GetArgument(2) << 32);
                                                  //Logger.Log("Read: " + count + " " + self.ToString("X4") + " " + Path.GetFileName(new zString(self + 0x60).ToString()));

            if (count > 0)
            {
                if (count > Buffer.Length)
                    Buffer = new byte[count];

                int read = handle.Read(Buffer, 0, (int)count);
                Process.Write(ptr, Buffer, (int)count);

                mem.EAX = read;
                return;
            }
            
            mem.EAX = 0;
        }

        static void VDFS_SearchFile(Hook hook, RegisterMemory mem)
        {
            int self = mem[Registers.ECX];

            string fileName = new zString(mem[0]).ToString();
            string folder = GetPathFolder(new zString(mem[1]).ToString());
            int arg3 = mem[2];

            //Logger.Log("SearchFile: " + fileName + " " + folder + " " + arg3);
            if (vFiles.Count > 0)
            {
                VDFSDirectoryInfo dir;
                if (vDirs.TryGetValue(folder, out dir))
                {
                    VDFSFileInfo fi = dir.SearchFile(fileName);
                    if (fi != null)
                    {
                        new zString(self + 0x60).Set(fi.Path);
                        mem[Registers.EAX] = 0;
                        //Logger.Log("Result: '" + new zString(self + 0x60) + "'");
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
                    new zString(self + 0x60).Set(GetPath(fi.FullName));
                    mem[Registers.EAX] = 0;
                    //Logger.Log("Result: '" + new zString(self + 0x60) + "'");
                    return;
                }
            }

            dirInfo = new DirectoryInfo(Program.GetGothicPath(folder));
            if (dirInfo.Exists)
            {
                FileInfo fi = dirInfo.EnumerateFiles(fileName, SearchOption.AllDirectories).FirstOrDefault();
                if (fi != null)
                {
                    new zString(self + 0x60).Set(GetPath(fi.FullName));
                    mem[Registers.EAX] = 0;
                    //Logger.Log("Result: '" + new zString(self + 0x60) + "'");
                    return;
                }
            }

            mem[Registers.EAX] = 0x138B;
        }

        /*static void VDFS_IsOpened(Hook hook, RegisterMemory mem)
        {
            mem[Registers.EAX] = openedFiles.ContainsKey(mem[Registers.ECX]) ? 1 : 0;
        }*/

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
