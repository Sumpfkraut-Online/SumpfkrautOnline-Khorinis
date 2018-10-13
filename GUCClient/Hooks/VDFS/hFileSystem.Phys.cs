using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApiNew;
using System.IO;
using Gothic.Types;

namespace GUC.Hooks.VDFS
{
    static partial class hFileSystem
    {
        static void AddHooks_Physical()
        {
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

            Process.AddFastHook(DirectFileConvert, 0x442390, 0x6);
            Process.AddFastHook(File_Open, 0x7D2010, 6).SetOriCodeReturn(); // fopen
            Process.AddFastHook(File_Exists, 0x7D197A, 0xA).SetOriCodeSkip(); // fexists
        }

        static void File_Exists(RegisterMemory mem)
        {
            string path = ReadString(mem.GetArg(0));

            string projectPath = GetProjectFilePath(path);
            if (File.Exists(projectPath))
                path = projectPath;
            
            mem.EAX = (int)File.GetAttributes(path);
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

        static void File_Open(RegisterMemory mem)
        {
            int arg0 = mem.GetArg(0);
            int arg1 = mem.GetArg(1);

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
                        mem.EAX = WinApi.Process.CDECLCALL<WinApi.IntArg>(0x7D1FDF, (WinApi.IntArg)z.PTR, (WinApi.IntArg)arg1, (WinApi.IntArg)0x40); // call our own paths
                    return;
                }
                else if (File.Exists(projectPath)) // read : no such file in the project folder
                {
                    using (zString z = zString.Create(projectPath))
                        mem.EAX = WinApi.Process.CDECLCALL<WinApi.IntArg>(0x7D1FDF, (WinApi.IntArg)z.PTR, (WinApi.IntArg)arg1, (WinApi.IntArg)0x40); // call our own paths
                    return;
                }
            }
            mem.EAX = WinApi.Process.CDECLCALL<WinApi.IntArg>(0x7D1FDF, (WinApi.IntArg)arg0, (WinApi.IntArg)arg1, (WinApi.IntArg)0x40); // just call it like it was meant to be
        }

        // turns a path from Gothic into a path to the project's folder
        static string GetProjectFilePath(string path)
        {
            if (path.Length > 0)
            {
                if (path.StartsWith(Program.GothicRootPath, StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(Program.GothicRootPath.Length + 1);
                }

                if (path[0] == '\\')
                {
                    path = path.Substring(1);
                }
                return Program.ProjectPathCombine(path);
            }
            return path;
        }

        // basically only for music system
        static void DirectFileConvert(RegisterMemory mem)
        {
            zString zStr = new zString(mem.GetArg(0));
            StringBuilder sb = new StringBuilder(zStr.ToString());
            if (sb.Length > 0)
            {
                if (sb.ToString().StartsWith(Program.GothicRootPath, StringComparison.OrdinalIgnoreCase))
                {
                    sb.Remove(0, Program.GothicRootPath.Length);
                }

                if (sb[0] != '\\')
                {
                    sb.Insert(0, '\\');
                }
                sb.Insert(0, Program.ProjectPath);

                string path = sb.ToString();
                if (Directory.Exists(path))
                {
                    zStr.Set(path);
                }
            }
        }
    }
}
