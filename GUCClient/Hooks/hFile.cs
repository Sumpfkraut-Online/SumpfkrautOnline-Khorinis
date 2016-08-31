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
    static class hFile
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            Process.AddHook(FileExistHook, 0x4436C7, 0xC);
            Process.AddHook(FileOpenHook, 0x443E40, 0x5);

            Process.AddHook(VDFSExistHook, 0x449077, 0x6);
            Process.AddHook(VDFSOpenHook, 0x44920F, 0x5);

            //Process.AddHook(FileCreateHook, 0x443AB6, 0x5);
            //Process.AddHook(DirCreateHook, 0x447382, 0x6);

            Logger.Log("Added file system hooks.");
        }

        static string GetProjectPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (path[0] == '\\')
                {
                    return Program.ProjectPath + path;
                }
                else if (path.StartsWith(Program.GothicPath, true, null))
                {
                    return Program.ProjectPath + path.Substring(Program.GothicPath.Length);
                }
                else
                {
                    return Path.Combine(Program.ProjectPath, path);
                }
            }
            return "";
        }

        #region File Open

        static void FileOpenHook(Hook hook)
        {
            zString z = new zString(hook.GetEAX());
            string path = GetProjectPath(z.ToString());

            if (File.Exists(path))
            {
                z.Set(path);
            }
        }

        #endregion

        #region File Exists

        static bool fileExists = false;
        static void SetFileExists(bool value, Hook hook)
        {
            if (fileExists != value)
            {
                fileExists = value;
                if (fileExists)
                {
                    Process.Write(new byte[2] { 0x53, 0xE9 }, hook.OldInNewAddress); // push ebx

                    int jmpAddress = 0x4436DF - (hook.OldInNewAddress + 6);
                    Process.Write(jmpAddress, hook.OldInNewAddress + 2);
                }
                else
                {
                    Process.Write(hook.GetOldCode(), hook.OldInNewAddress);
                }
            }
        }

        static void FileExistHook(Hook hook)
        {
            zString zStr = new zString(hook.GetEAX());
            if (File.Exists(GetProjectPath(zStr.ToString())))
            {
                SetFileExists(true, hook);
                hook.SetEAX(0);
            }
            else
            {
                SetFileExists(false, hook);
            }
        }

        #endregion

        #region VDFS Open

        static void VDFSOpenHook(Hook hook)
        {
            zString z = new zString(hook.GetEAX());
            string path = GetProjectPath(z.ToString());

            if (File.Exists(path))
            {
                z.Set(path);
            }
        }

        #endregion

        #region VDFS Exists

        static bool vdfsExists = false;
        static void SetVdfsExists(bool value, Hook hook)
        {
            if (vdfsExists != value)
            {
                vdfsExists = value;
                if (vdfsExists)
                {
                    Process.Write(new byte[1] { /*0x83, 0xEC, 0x08,*/ 0xE9 }, hook.OldInNewAddress); // sub esp, 8

                    int jmpAddress = 0x44909B - (hook.OldInNewAddress + 5);
                    Process.Write(jmpAddress, hook.OldInNewAddress + 1);
                }
                else
                {
                    Process.Write(hook.GetOldCode(), hook.OldInNewAddress);
                }
            }
        }

        static void VDFSExistHook(Hook hook)
        {
            zString z = new zString(hook.GetEAX());
            
            if (File.Exists(Program.ProjectPath + z))
            {
                SetVdfsExists(true, hook);
                hook.SetEAX(2);
            }
            else
            {
                SetVdfsExists(false, hook);
            }
        }

        #endregion

        #region File Create

        static void FileCreateHook(Hook hook)
        {
            zString z = new zString(hook.GetEAX());
            string path = GetProjectPath(z.ToString());

            FileInfo fi = new FileInfo(path);
            if (string.Compare(fi.Name, "MSSSOFT.M3D", true) != 0)
            {
                if (!fi.Directory.Exists) fi.Directory.Create();
                z.Set(path);
            }
        }

        #endregion

        #region Dir Create

        static void DirCreateHook(Hook hook)
        {
            zString z = new zString(hook.GetEAX());
            string path = GetProjectPath(z.ToString());
            z.Set(path);
        }

        #endregion
    }
}
