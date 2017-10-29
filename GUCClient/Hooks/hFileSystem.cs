using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using WinApi;
using System.IO;
using Gothic.Types;
using System.Runtime.InteropServices;

namespace GUC.Hooks
{
    static class hFileSystem
    {
        // helper class for FindFirst -> FindNext -> FindClose
        class FindFileInfo
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
            static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

            [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
            static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool FindClose(IntPtr hFindFile);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            struct WIN32_FIND_DATA
            {
                public uint dwFileAttributes;
                public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
                public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
                public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
                public uint nFileSizeHigh;
                public uint nFileSizeLow;
                public uint dwReserved0;
                public uint dwReserved1;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string cFileName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
                public string cAlternateFileName;
            }

            // Gothic called FindFirst, check the project folder first
            public static FindFileInfo First(string originalPath)
            {
                string projectPath = GetProjectFilePath(originalPath);
                if (!string.IsNullOrWhiteSpace(projectPath))
                {
                    IntPtr result = FindFirstFile(projectPath, out Data);
                    if (result.ToInt32() != -1)
                    { // found something
                        return new FindFileInfo(result, originalPath);
                    }
                }
                return null;
            }

            // Gothic called FindNext
            public bool Next()
            {
                // check if there's a result with the current handle
                bool result = FindNextFile(handlePtr, out Data);

                if (!result && searchingProjectFolder) // no result & haven't searched the original path yet?
                {
                    // continue searching in the original path
                    this.handlePtr = FindFirstFile(this.originalPath, out Data);
                    if (this.HandlePtr != -1)
                    {
                        searchingProjectFolder = false;
                        return true;
                    }
                }
                return result;
            }

            // Gothic called FindClose
            public bool Close()
            {
                // close the project folder handle
                bool result = FindClose(this.projectPtr);

                // close the current handle (gothic folder), if it's active
                if (!this.searchingProjectFolder)
                    result = FindClose(this.handlePtr);

                return result;
            }

            IntPtr projectPtr;
            IntPtr handlePtr;
            public int HandlePtr { get { return this.handlePtr.ToInt32(); } }
            string originalPath;
            bool searchingProjectFolder;

            public FindFileInfo(IntPtr handle, string path)
            {
                this.projectPtr = handle;
                this.handlePtr = handle;
                this.originalPath = path;
                this.searchingProjectFolder = true;
            }

            static WIN32_FIND_DATA Data;
            public static string CurrentFileName { get { return string.Copy(Data.cFileName); } }
            static byte[] DataArr;
            static IntPtr DataPtr;

            static FindFileInfo()
            {
                Data = new WIN32_FIND_DATA();
                int size = Marshal.SizeOf(Data);
                DataArr = new byte[size];
                DataPtr = Marshal.AllocHGlobal(size);
            }

            // write the last WIN32_FIND_DATA
            public static void WriteData(int ptr)
            {
                Marshal.StructureToPtr(Data, DataPtr, true);
                Marshal.Copy(DataPtr, DataArr, 0, DataArr.Length);
                Process.Write(ptr, DataArr);
            }
        }

        // Dictionary of FindFile-Handles
        static Dictionary<int, FindFileInfo> FindFileTable = new Dictionary<int, FindFileInfo>();

        static void File_FindFirst(Hook hook, RegisterMemory mem)
        {
            int pathPtr = Process.ReadInt(mem[Registers.EBP] + 0x8); // char*
            int dataPtr = mem[Registers.EBP] - 0x140; // WIN32_FIND_DATA

            string path = ReadString(pathPtr);

            // check if the project folder returns us a handle
            FindFileInfo info = FindFileInfo.First(path);
            if (info != null)
            {
                // files found in the project folder!
                FindFileTable.Add(info.HandlePtr, info);
                FindFileInfo.WriteData(dataPtr);
                mem[Registers.EAX] = info.HandlePtr;

                hook.SetSkipOldCode(true);
            }
            else // nothing found
            {
                hook.SetSkipOldCode(false);
            }
        }

        static void File_FindNext(Hook hook, RegisterMemory mem)
        {
            int ebp = mem[Registers.EBP];
            int handlePtr = Process.ReadInt(ebp + 0x8); // findfile handle
            int dataPtr = ebp - 0x140; // WIN32_FIND_DATA

            FindFileInfo info;
            if (FindFileTable.TryGetValue(handlePtr, out info))
            {
                // this is one of our handles
                bool result = info.Next();
                FindFileInfo.WriteData(dataPtr);
                mem[Registers.EAX] = result ? 1 : 0;

                hook.SetSkipOldCode(true);
            }
            else
            {
                hook.SetSkipOldCode(false);
            }
        }

        static void File_FindClose(Hook hook, RegisterMemory mem)
        {
            int handlePtr = mem[0]; // findfile handle

            FindFileInfo info;
            if (FindFileTable.TryGetValue(handlePtr, out info))
            {
                // this is one of our handles
                mem[Registers.EAX] = info.Close() ? 1 : 0;
                FindFileTable.Remove(handlePtr);

                hook.SetSkipOldCode(true);

            }
            else
            {
                hook.SetSkipOldCode(false);
            }
        }

        public static void AddHooks()
        {
            Process.AddHook(BeginInit, 0x44AFD3, 7);
            Process.AddHook(EndInit, 0x44AFE2, 5);

            Process.AddHook(VDFS_Open, 0x44920F, 5); // open
            Process.AddHook(VDFS_Exists, 0x44909B, 6); // Exists

            var h = Process.AddHook(File_Open, 0x7D2010, 6); // fopen
            Process.Write(h.OldInNewAddress, (byte)0xC3);

            h = Process.AddHook(File_Exists, 0x7D197A, 0xA); // fexists
            h.SetSkipOldCode(true);

            Process.Write(0x44AEDF, 0xE9, 0x8C, 0x00, 0x00, 0x00); // skip visual vdfs init (vdfs32g.exe)
            Process.Write(0x00470846, 0xE9, 0x96, 0x02, 0x00, 0x00); // skip forced vdfs intialization

            Process.AddHook(File_FindFirst, 0x7D2517, 0xA);
            Process.AddHook(File_FindNext, 0x7D25E3, 0xA);
            Process.AddHook(File_FindClose, 0x7D269B, 0xA);

            Logger.Log("Added new file system hooks.");
        }


        const string BackupEnding = ".guc_backup";
        public static void BeginInit(Hook hook, RegisterMemory mem)
        {
            string vdfsPath = Program.GetGothicRootPath("vdfs.cfg");
            string backupPath = vdfsPath + BackupEnding;

            if (!File.Exists(vdfsPath))
                Logger.LogError("FileSystem BeginInit: '{0}' not found!", vdfsPath);

            // if there's still an old file, it's probably the original. So only move if it's not there.
            if (!File.Exists(backupPath))
            {
                // save the original
                File.Move(vdfsPath, vdfsPath + BackupEnding);
            }

            // write our own file
            using (FileStream fs = new FileStream(vdfsPath, FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("// Generated by the GUC.\n// You might find the original under the name 'vdfs.cfg.guc_backup'.");
                sw.WriteLine("[VDFS]");
                sw.WriteLine(Path.Combine(Program.GothicRootPath, "data", "*.vdf"));
                sw.WriteLine(Path.Combine(Program.ProjectPath, "data", "*.vdf"));
                sw.WriteLine(Path.Combine(Program.ProjectPath, "data", "*.mod"));
                sw.WriteLine("[END]");
            }

            // fixme ? set dates of project's vdfs so they're preferred
        }

        static void EndInit(Hook hook, RegisterMemory mem)
        {
            string vdfsPath = Program.GetGothicRootPath("vdfs.cfg");
            string backupPath = vdfsPath + BackupEnding;

            if (!File.Exists(backupPath))
                Logger.LogError("FileSystem EndInit: '{0}' not found!", backupPath);

            // delete our own file
            if (File.Exists(vdfsPath))
                File.Delete(vdfsPath);

            // put the backup back in place
            File.Move(backupPath, vdfsPath);
        }

        const int VDF_VIRTUAL = 1;
        const int VDF_PHYSICAL = 2;

        // turns a path from Gothic into a path to the project's folder
        static string GetProjectFilePath(string path)
        {
            if (path.Length > 0)
            {
                if (path.StartsWith(Program.GothicRootPath, StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(Program.GothicRootPath.Length);
                }

                if (path[0] == '\\')
                {
                    path = path.Substring(1);
                }

                return Program.GetProjectPath(path);
            }

            return path;
        }

        static void VDFS_Open(Hook hook, RegisterMemory mem)
        {
            zString path = new zString(mem[Registers.EAX]);
            string pathStr = path.ToString();

            if (pathStr.EndsWith("CAMERA.DAT")) // fuck this
                path.Set(@"_WORK\DATA\SCRIPTS\_COMPILED\CAMERA.DAT");

            // check if it exists in vdfs
            int filePtr = mem[Registers.ESI];
            if (Process.THISCALL<BoolArg>(filePtr, 0x449020) && Process.ReadInt(filePtr + 0x2A00) == VDF_VIRTUAL)
                return;

            // check if it's in the project's folder
            string projectPath = GetProjectFilePath(pathStr);
            if (File.Exists(projectPath))
                path.Set(projectPath);
        }

        // reads a char* from the process
        static string ReadString(int ptr)
        {
            if (ptr == 0)
                return "";

            byte[] arr;
            using (MemoryStream ms = new MemoryStream(400))
            {
                byte readByte;
                while ((readByte = Process.ReadByte(ptr++)) != 0)
                    ms.WriteByte(readByte);

                arr = ms.ToArray();
            }

            return Encoding.Default.GetString(arr.ToArray());
        }

        static void VDFS_Exists(Hook hook, RegisterMemory mem)
        {
            string path = ReadString(mem[Registers.EDI]);
            int result = mem[Registers.EAX];

            if (result != 0)
                return; // gothic has found something

            if (string.IsNullOrWhiteSpace(path))
                return;

            path = GetProjectFilePath(path);
            if (File.Exists(path))
                mem[Registers.EAX] = VDF_PHYSICAL;
        }
        
        static void File_Open(Hook hook, RegisterMemory mem)
        {
            int arg0 = mem[0];
            int arg1 = mem[1];

            string path = ReadString(arg0);
            if (!path.EndsWith("MSSSOFT.M3D", StringComparison.OrdinalIgnoreCase)) // makes problems 
            {
                string parms = ReadString(arg1);

                string projectPath = GetProjectFilePath(path);
                if (parms.Contains('w') || parms.Contains('a')) // write or append
                {
                    string dir = Path.GetDirectoryName(projectPath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (zString z = zString.Create(projectPath))
                        mem[Registers.EAX] = Process.CDECLCALL<IntArg>(0x7D1FDF, (IntArg)z.PTR, (IntArg)arg1, (IntArg)0x40); // call our own paths
                    return;
                }
                else if (File.Exists(projectPath)) // read : no such file in the project folder
                {
                    using (zString z = zString.Create(projectPath))
                        mem[Registers.EAX] = Process.CDECLCALL<IntArg>(0x7D1FDF, (IntArg)z.PTR, (IntArg)arg1, (IntArg)0x40); // call our own paths
                    return;
                }
            }
            mem[Registers.EAX] = Process.CDECLCALL<IntArg>(0x7D1FDF, (IntArg)arg0, (IntArg)arg1, (IntArg)0x40); // just call it like it was meant to be
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        static extern uint GetFileAttributes(string lpFileName);

        static void File_Exists(Hook hook, RegisterMemory mem)
        {
            string path = ReadString(mem[0]);

            string projectPath = GetProjectFilePath(path);
            if (File.Exists(projectPath))
                path = projectPath;

            int attr = (int)GetFileAttributes(path);
            mem[Registers.EAX] = attr;
        }
    }
}
