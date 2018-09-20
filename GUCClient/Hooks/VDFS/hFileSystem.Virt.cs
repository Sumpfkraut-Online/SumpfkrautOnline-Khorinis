using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Injection;
using System.IO;
using Gothic.Types;
using GUC.Log;
using Gothic.System;

namespace GUC.Hooks.VDFS
{
    static partial class hFileSystem
    {
		static void AddHooks_Virtual()
        {
            Process.AddFastHook(VDFS_Exists, 0x449020, 6).SetOriCodeReturn(0);
            Process.AddFastHook(VDFS_Open, 0x449120, 6).SetOriCodeReturn(1);
            Process.AddFastHook(VDFS_Close, 0x4493A0, 8).SetOriCodeReturn(0);
            Process.AddFastHook(VDFS_Close, 0x448ED0, 7); // zFile_VDFS Destructor

            Process.AddFastHook(VDFS_Seek, 0x449490, 6).SetOriCodeReturn(1);
            Process.AddFastHook(VDFS_Pos, 0x449A50, 5).SetOriCodeReturn(0);
            Process.AddFastHook(VDFS_EOF, 0x449470, 5).SetOriCodeReturn(0);
            Process.AddFastHook(VDFS_Size, 0x449410, 5).SetOriCodeReturn(0);

            Process.AddFastHook(VDFS_ReadSize, 0x44ABF0, 6).SetOriCodeReturn(2);
            Process.AddFastHook(VDFS_ReadString, 0x44A8D0, 6).SetOriCodeReturn(1);

            Process.AddFastHook(VDFS_SearchFile, 0x449E80, 6).SetOriCodeReturn(3);
        }


        static void VDFS_Exists(RegisterMemory mem)
        {
            try
            {
                if (openedFiles.ContainsKey(mem.ECX))
                {
                    mem.EAX = 1;
                    return;
                }

                string path = new zString(mem.ECX + 0x60).ToString();

                if (path.Length > 0 && path[0] != '\\') // not virtual?
                    path = zFile.s_virtPathString + path;

                if (path[0] != '\\')
                    path = '\\' + path;

                if ((vFiles.Count > 0 && vFiles.ContainsKey(path))
                    || File.Exists(Program.ProjectPath + path)
                    || File.Exists(Program.GothicRootPath + path))
                {
                    //Logger.Log("Exists: " + path);
                    mem.EAX = 1;
                    return;
                }

                //Logger.Log("Not Existing: " + path);
                mem.EAX = 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static void VDFS_Open(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;
                if (openedFiles.ContainsKey(self))
                {
                    Logger.Log("File is already opened!");
                    mem.EAX = 0;
                    return;
                }

                IFileHandle fileHandle;
                string path = new zString(self + 0x60).ToString();
                if (path.Length > 0 && path[0] != '\\') // not virtual enough?)
                    path = zFile.s_virtPathString + path;

                if (path[0] != '\\')
                    path = '\\' + path;

                if (path.EndsWith("CAMERA.DAT")) // gothic fucks over its virtual path with threads sometimes
                    path = @"\_WORK\DATA\SCRIPTS\_COMPILED\CAMERA.DAT";

                //Logger.Log("Open " + self.ToString("X4") + " " + path);
                if (vFiles.Count > 0 && vFiles.TryGetValue(path, out VDFSFileInfo vdfsFileInfo))
                {
                    fileHandle = new VDFSFileHandle(vdfsFileInfo);
                }
                else
                {
                    FileInfo fullPath = new FileInfo(Program.ProjectPath + path);
                    if (fullPath.Exists)
                    {
                        fileHandle = new FileHandle(fullPath);
                    }
                    else
                    {
                        fullPath = new FileInfo(Program.GothicRootPath + path);

                        if (fullPath.Exists)
                        {
                            fileHandle = new FileHandle(fullPath);
                        }
                        else
                        {
                            Logger.Log("Open: File not found! '" + fullPath + "'");
                            mem.EAX = 0x13F2;
                            return;
                        }
                    }
                }

                fileHandle.Open();
                openedFiles.Add(self, fileHandle);
                mem.EAX = 0;
                Process.WriteBool(self + 0x29FC, true);
                Process.WriteBool(self + 0x8C, true);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static void VDFS_Close(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;

                if (openedFiles.TryGetValue(self, out IFileHandle handle))
                {
                    handle.Close();
                    openedFiles.Remove(self);
                    Process.WriteBool(self + 0x29FC, false);
                    Process.WriteBool(self + 0x8C, false);
                    //Logger.Log("Close " + self.ToString("X4"));
                }
                mem.EAX = 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static void VDFS_Seek(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;

                if (!openedFiles.TryGetValue(self, out IFileHandle handle))
                {
                    Logger.LogError("Seek: File is not opened!" + new zString(self + 0x60));
                    return;
                }

                long pos = mem.GetArg(0);// | ((long)hook.GetArgument(1) << 32);
                                         //Logger.Log("Seek: " + pos + " " + self.ToString("X4"));

                handle.Seek(pos);
                mem.EAX = 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static void VDFS_EOF(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;

                if (!openedFiles.TryGetValue(self, out IFileHandle handle))
                {
                    Logger.LogError("EOF: File is not opened!" + new zString(self + 0x60));
                    return;
                }
                //Logger.Log("EOF: " + self.ToString("X4"));

                mem.EAX = (handle.GetSize() - handle.GetPos()) <= 0 ? 1 : 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static void VDFS_Pos(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;

                if (!openedFiles.TryGetValue(self, out IFileHandle handle))
                {
                    Logger.LogError("Pos: File is not opened!" + new zString(self + 0x60));
                    return;
                }

                long pos = handle.GetPos();
                //Logger.Log("Pos: " + self.ToString("X4") + " " + pos);
                //hook.SetEDX((int)(pos >> 32)); // for long

                mem.EAX = (int)pos;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static void VDFS_Size(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;

                if (!openedFiles.TryGetValue(self, out IFileHandle handle))
                {
                    Logger.LogError("Size: File is not opened!" + new zString(self + 0x60));
                    return;
                }

                long size = handle.GetSize();
                //Logger.Log("Size: " + size);
                //hook.SetEDX((int)(size >> 32)); // for long

                mem.EAX = (int)size;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static byte[] Buffer = new byte[10000];
        static void VDFS_ReadSize(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;

                if (!openedFiles.TryGetValue(self, out IFileHandle handle))
                {
                    Logger.LogError("ReadSize: File is not opened!");
                    return;
                }

                int ptr = mem.GetArg(0);
                if (ptr == 0)
                    Logger.LogError("ReadSize ptr is 0!");

                long count = (long)mem.GetArg(1);// | ((long)hook.GetArgument(2) << 32);
                                                 //Logger.Log("Read: {0} {1} {2}", count, self.ToString("X4"), Path.GetFileName(new zString(self + 0x60).ToString()));

                if (count > 0)
                {
                    if (count > Buffer.Length)
                        Buffer = new byte[count];

                    int read = handle.Read(Buffer, 0, (int)count);
                    Process.WriteBytes(ptr, Buffer, (int)count);

                    mem.EAX = read;
                    return;
                }

                mem.EAX = 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        // Reads until 0x0A
        static void VDFS_ReadString(RegisterMemory mem)
        {
            try
            {
                int self = mem.ECX;

                if (!openedFiles.TryGetValue(self, out IFileHandle handle))
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

                int ptr = mem.GetArg(0);
                if (ptr == 0)
                    Logger.LogError("ReadString ptr is 0!");

                Process.WriteBytes(ptr, Buffer, index + 1);
                mem.EAX = 1;

                //Logger.Log("Read string");
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        static void VDFS_SearchFile(RegisterMemory mem)
        {
            try
            {
                zFile_File zfile = new zFile_File(mem.ECX);
                string fileName = new zString(mem.GetArg(0)).ToString();
                string folder = new zString(mem.GetArg(1)).ToString();

                //Logger.Log("SearchFile: " + fileName + " " + folder);
                if (vFiles.Count > 0)
                {
                    if (vDirs.TryGetValue(folder, out VDFSDirectoryInfo dir))
                    {
                        VDFSFileInfo fi = dir.SearchFile(fileName);
                        if (fi != null)
                        {
                            zfile.SetPath(fi.Path);
                            mem.EAX = 0;
                            return;
                        }
                    }
                }

                DirectoryInfo dirInfo = new DirectoryInfo(Program.ProjectPath + folder);
                if (dirInfo.Exists)
                {
                    FileInfo fi = dirInfo.EnumerateFiles(fileName, SearchOption.AllDirectories).FirstOrDefault();
                    if (fi != null)
                    {
                        zfile.SetPath(fi.FullName.Substring(Program.ProjectPath.Length));
                        mem.EAX = 0;
                        return;
                    }
                }

                dirInfo = new DirectoryInfo(Program.GothicRootPath + folder);
                if (dirInfo.Exists)
                {
                    FileInfo fi = dirInfo.EnumerateFiles(fileName, SearchOption.AllDirectories).FirstOrDefault();
                    if (fi != null)
                    {
                        zfile.SetPath(fi.FullName.Substring(Program.GothicRootPath.Length));
                        mem.EAX = 0;
                        return;
                    }
                }

                mem.EAX = 0x138B;
                //Logger.Log("SearchFile: " + fileName + " not found!");
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        /*static void VDFS_IsOpened(Hook hook, RegisterMemory mem)
        {
            mem[Registers.EAX] = openedFiles.ContainsKey(mem[Registers.ECX]) ? 1 : 0;
        }*/
    }
}
