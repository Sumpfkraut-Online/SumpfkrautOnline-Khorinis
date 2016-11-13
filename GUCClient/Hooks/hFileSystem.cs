using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using WinApi;
using System.IO;
using Gothic.Types;

namespace GUC.Hooks
{
    static class hFileSystem
    {
        public static void AddHooks()
        {
            Process.AddHook(BeginInit, 0x44AFD3, 7);
            Process.AddHook(EndInit, 0x44AFE2, 5);

            Process.AddHook(VDFS_Open, 0x44920F, 5);
            Process.AddHook(VDFS_Exists, 0x449077, 6);

            Process.AddHook(File_Open, 0x7D2010, 6, 2); // fopen
            Process.AddHook(File_Exists, 0x7D197A, 10, 1); // fexists

            //Process.AddHook(SearchFile, 0x446AC0, 7, 2);

            Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x44AEDF); // skip visual vdfs init (vdfs32g.exe)
            Process.Write(new byte[] { 0xE9, 0x96, 0x02, 0x00, 0x00 }, 0x00470846); // skip forced vdfs intialization

            Logger.Log("Added new file system hooks.");
        }

        const string BackupEnding = ".backup";
        static void BeginInit(Hook hook)
        {
            string vdfsPath = Program.GetGothicRootPath("vdfs.cfg");
            string backupPath = vdfsPath + BackupEnding;

            if (!File.Exists(vdfsPath))
                Logger.LogError("FileSystem BeginInit: '{0}' not found!", vdfsPath);

            // delete old backup in case it's there
            if (File.Exists(backupPath))
                File.Delete(backupPath);

            // save the original
            File.Move(vdfsPath, vdfsPath + BackupEnding);

            // write our own file
            using (FileStream fs = new FileStream(vdfsPath, FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("[VDFS]");
                sw.WriteLine(Path.Combine(Program.GothicRootPath, "data", "*.vdf"));
                sw.WriteLine(Path.Combine(Program.ProjectPath, "data", "*.vdf"));
                sw.WriteLine(Path.Combine(Program.ProjectPath, "data", "*.mod"));
                sw.WriteLine("[END]");
            }

            // set dates of project's vdfs?
        }

        static void EndInit(Hook hook)
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

        static string GetProjectFilePath(string path)
        {
            if (path.Length > 0)
            {
                if (path[0] == '\\')
                {
                    path = path.Substring(1);
                }
                else if (path.StartsWith(Program.GothicRootPath, StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(Program.GothicRootPath.Length);
                }
                return Program.GetProjectPath(path);
            }

            return path;
        }

        static void CheckChangeToProjectPath(zString path)
        {
            string projectPath = GetProjectFilePath(path.ToString());
            if (File.Exists(projectPath))
            {
                path.Set(projectPath);
            }
        }

        static void VDFS_Open(Hook hook)
        {
            zString path = new zString(hook.GetEAX());

            // check if it exists in vdfs
            if (FileExistsVirtual(path))
                return;

            // check if it's in the project's folder
            CheckChangeToProjectPath(path);
        }

        static bool FileExistsVirtual(zString path)
        {
            int vdfExistsPtr = Process.ReadInt(0x82E66C); // function ptr
            return Process.CDECLCALL<IntArg>(vdfExistsPtr, (IntArg)path.PTR, (IntArg)VDF_VIRTUAL) == VDF_VIRTUAL; // _vdf_exists
        }


        static List<byte> stringList = new List<byte>(400);
        static string ReadString(int ptr)
        {
            stringList.Clear();

            byte readByte;
            while ((readByte = Process.ReadByte(ptr++)) != 0)
                stringList.Add(readByte);

            return Encoding.Default.GetString(stringList.ToArray());
        }

        static void VDFS_Exists(Hook hook)
        {
            zString path = new zString(hook.GetEAX());

            if (FileExistsVirtual(path)) // fixme ? duplicate VDF_Exists-Check, gothic does one after this
                return;

            // check if it's in the project's folder
            CheckChangeToProjectPath(path);
        }

        static zString filePointer = zString.Create("");
        static void File_Open(Hook hook)
        {
            string path = ReadString(hook.GetArgument(0));
            if (path.EndsWith("MSSSOFT.M3D", StringComparison.OrdinalIgnoreCase)) // makes problems cause it's not using the hooked fopen method or smth
                return;

            string parms = ReadString(hook.GetArgument(1));

            string projectPath = GetProjectFilePath(path);
            if (parms.Contains('w') || parms.Contains('a')) // write or append
            {
                string dir = Path.GetDirectoryName(projectPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            else if (!File.Exists(projectPath)) // read : no such file in the project folder
            {
                return;
            }

            filePointer.Set(projectPath);
            hook.SetArgument(0, filePointer.PTR);
        }

        static void File_Exists(Hook hook)
        {
            string path = ReadString(hook.GetArgument(0));

            string projectPath = GetProjectFilePath(path);
            if (File.Exists(projectPath))
            {
                filePointer.Set(projectPath);
                hook.SetArgument(0, filePointer.PTR);
            }
        }

        static bool blockedSearchFile = false;
        // searches for a physical file
        static void SearchFile(Hook hook)
        {
            zString name = new zString(hook.GetArgument(0));
            zString path = new zString(hook.GetArgument(1));


            // check project folder first
            string projectPath = GetProjectFilePath(path.ToString());
            DirectoryInfo dir = new DirectoryInfo(projectPath);

            //Logger.LogWarning(path + " " + new zString(hook.GetArgument(0)));

            // check whether the folder exists in project directory
            if (dir.Exists)
            {
                string foundFilePath = null;

                // get search pattern / filename
                string fileName = name.ToString().TrimStart('\\', '/');
                if (!fileName.EndsWith("MSSSOFT.M3D", StringComparison.OrdinalIgnoreCase))
                {
                    // check whether there are folders in front of the file path
                    int index = fileName.IndexOfAny(new char[] { '\\', '/' });
                    if (index >= 0)
                    {
                        // extract the first folder
                        string firstFolder = fileName.Remove(index);
                        fileName = fileName.Substring(index + 1);

                        // search for the first folder
                        foreach (DirectoryInfo subdir in dir.EnumerateDirectories(firstFolder, SearchOption.AllDirectories))
                        {
                            // check whether the path is right
                            string filePath = Path.Combine(subdir.FullName, fileName);
                            if (File.Exists(filePath))
                                foundFilePath = filePath;
                        }
                    }
                    else
                    {
                        // just search for the file
                        FileInfo fi = dir.EnumerateFiles(fileName, SearchOption.AllDirectories).FirstOrDefault();
                        if (fi != null)
                            foundFilePath = fi.FullName;
                    }

                    if (foundFilePath != null)
                    {
                        if (!blockedSearchFile)
                        {
                            // block
                            Process.Write(new byte[] { 0xC2, 0x0C, 0x00 }, hook.OldInNewAddress); // retn    
                            blockedSearchFile = true;
                        }
                        
                        SetZFilePath(hook.GetECX(), foundFilePath.Substring(Program.ProjectPath.Length));
                        hook.SetEAX(0);
                        return;


                    }
                }
            }

            if (blockedSearchFile)
            {
                // unblock
                Process.Write(hook.GetOldCode(), hook.OldInNewAddress);
                blockedSearchFile = false;
            }
        }

        static void SetZFilePath(int filePtr, string path)
        {
            Logger.Log("Found file: " + path);

            using (var z = zString.Create(path.ToUpperInvariant()))
                Process.THISCALL<NullReturnCall>(filePtr, 0x4455D0, (IntArg)z.VTBL, (IntArg)z.ALLOCATER, (IntArg)z.PTR, (IntArg)z.Length, (IntArg)z.Res);

            //Logger.LogWarning(Process.THISCALL<IntArg>(filePtr, 0x449D20).Value.ToString("X4"));

            /* new zString(filePtr + 0x10).Set(Path.GetDirectoryName(path).Substring(2) + "\\"); // \something\
             new zString(filePtr + 0x74).Set(Path.GetDirectoryName(path) + "\\"); // D:\path\
             new zString(filePtr + 0x24).Set(path.Remove(1)); // D
             new zString(filePtr + 0x4C).Set(Path.GetExtension(path).Substring(1)); // txt
             new zString(filePtr + 0x38).Set(Path.GetFileNameWithoutExtension(path)); // test

             new zString(filePtr + 0x60).Set(path); // path

             Process.THISCALL<NullReturnCall>(filePtr, 0x445360); // SetCompletePath*/

        }
    }
}
