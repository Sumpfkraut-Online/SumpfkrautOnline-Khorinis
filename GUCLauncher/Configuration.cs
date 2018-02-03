using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Security.Cryptography;
using GUC;
using System.Collections.ObjectModel;

namespace GUCLauncher
{
    static class Configuration
    {
        const string ConfigFile = "launcher.cfg";

        static ServerListItem activeProject = null;
        public static ServerListItem ActiveProject { get { return activeProject; } }
        public static void SetActiveProject(ServerListItem item)
        {
            activeProject = item;
            Save();
        }

        static string gothicPath;
        public static string GothicPath { get { return gothicPath; } }
        public static string GothicApp { get { return Path.Combine(gothicPath, @"System\Gothic2.exe"); } }
        public static string zSpyApp { get { return Path.Combine(gothicPath, @"_work\tools\zSpy\zSpy.exe"); } }

        public static int zSpyLevel = 5;

        static ObservableCollection<ServerListItem> servers = new ObservableCollection<ServerListItem>();
        public static ObservableCollection<ServerListItem> Servers { get { return servers; } }

        public static void Init()
        {
            const string LanguageFile = "launcher.languages";
            if (!File.Exists(LanguageFile))
            {
                using (var s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GUCLauncher.Resources." + LanguageFile))
                using (var fs = new FileStream(LanguageFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    s.CopyTo(fs);
                }
            }

            LangStrings.LoadFile(LanguageFile);

            Load();
        }

        #region Add & Remove Servers

        public static void AddServer(string address)
        {
            if (ServerListItem.TryGetAddress(address, out string ip, out ushort port))
            {
                servers.Add(new ServerListItem(ip, port));
                Save();
            }
        }

        public static void RemoveServer(int index)
        {
            if (index >= 0 && index < servers.Count)
            {
                servers.RemoveAt(index);
                Save();
            }
        }

        #endregion

        #region Load & Save

        static void Load()
        {
            servers.Clear();

            if (!File.Exists(ConfigFile))
                return;

            using (StreamReader sr = new StreamReader(ConfigFile))
            {
                string line = sr.ReadLine();
                if (line != null && (line = line.Trim()).StartsWith("Language="))
                {
                    if (int.TryParse(line.Substring("Language=".Length).TrimStart(), out int language))
                        LangStrings.LanguageIndex = language;
                }

                line = sr.ReadLine();
                if (line != null && (line = line.Trim()).StartsWith("Path="))
                {
                    gothicPath = Path.GetFullPath(line.Substring("Path=".Length).TrimStart());
                }

                line = sr.ReadLine();
                if (line != null && (line = line.Trim()).StartsWith("zSpy="))
                {
                    if (int.TryParse(line.Substring("zSpy=".Length).TrimStart(), out int spy))
                        zSpyLevel = spy;                        
                }

                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var item = ServerListItem.ReadNew(sr);

                    if (string.Compare(sr.ReadLine(), "active", true) == 0)
                    {
                        activeProject = item;
                    }

                    if (item != null)
                        servers.Add(item);
                }
            }
        }

        public static void Save()
        {
            using (StreamWriter sw = new StreamWriter(ConfigFile))
            {
                sw.WriteLine("Language=" + LangStrings.LanguageIndex);
                sw.WriteLine("Path=" + gothicPath);
                sw.WriteLine("zSpy=" + zSpyLevel);
                sw.WriteLine();
                foreach (ServerListItem item in servers)
                {
                    item.Write(sw);
                    if (activeProject == item)
                        sw.WriteLine("Active");
                    else
                        sw.WriteLine();
                }
            }
        }

        #endregion

        #region Check Gothic

        #region Hashes

        public enum HashFile
        {
            Gothic2,
            VDFS32g,
            VDFS32e,
            SHW32
        }

        static readonly Dictionary<HashFile, byte[]> hashDict = new Dictionary<HashFile, byte[]>()
        {
            { HashFile.Gothic2, new byte[16] { 0x3C, 0x43, 0x6B, 0xD1, 0x99, 0xCA, 0xAA, 0xA6, 0x4E, 0x97, 0x36, 0xE3, 0xCC, 0x1C, 0x9C, 0x32 } },
            { HashFile.VDFS32g, new byte[16] { 0xA6, 0xC1, 0x82, 0xA1, 0x5F, 0xB9, 0x14, 0x84, 0xB4, 0x58, 0x54, 0x71, 0xA1, 0x48, 0x4E, 0xF5 } },
            { HashFile.VDFS32e, new byte[16] { 0xF0, 0x58, 0x34, 0xF5, 0x2F, 0x5E, 0x66, 0x8B, 0x24, 0x85, 0xA3, 0x7C, 0x0C, 0x5B, 0x8E, 0xFA } },
            { HashFile.SHW32, new byte[16] { 0xBE, 0xCB, 0x4C, 0xB4, 0x04, 0x68, 0xFC, 0x7B, 0x06, 0x23, 0x65, 0x48, 0x0E, 0xD6, 0x43, 0x7B } }
        };

        public static bool ValidateFileHash(string fileName, HashFile hashFile)
        {
            if (!hashDict.TryGetValue(hashFile, out byte[] validHash))
                return false;

            byte[] computedHash;
            using (Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                computedHash = md5.ComputeHash(fs);
            }

            return computedHash.SequenceEqual(validHash);
        }

        #endregion

        static void ShowMessageBox(string text, params object[] args)
        {
            MessageBox.Show(args.Length == 0 ? text : string.Format(text, args), LangStrings.Get("Config_Search"), MessageBoxButton.OK);
        }

        public static void ShowSearchPathMessage()
        {
            ShowMessageBox(LangStrings.Get("Config_Search_Long"));
        }

        public static bool CheckGothicPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                ShowSearchPathMessage();
            }
            else
            {
                if (string.Equals(Path.GetFileName(path), "SYSTEM", StringComparison.OrdinalIgnoreCase))
                    path = Path.GetDirectoryName(path); // move up to base

                FailCode code = CheckGothicVersion(path);
                switch (code)
                {
                    case FailCode.GothicNotFound:
                    case FailCode.VDFS32NotFound:
                    case FailCode.SHW32NotFound:
                        ShowMessageBox("{0} ({1})\n{2}", LangStrings.Get("Config_NotFound"), code, LangStrings.Get("Config_Search_Long"));
                        break;
                    case FailCode.GothicWrongVersion:
                        ShowMessageBox(LangStrings.Get("Config_WrongVersion"));
                        break;
                    case FailCode.VDFS32WrongVersion:
                    case FailCode.SHW32WrongVersion:
                        ShowMessageBox("{0} ({1})", code);
                        break;
                    case FailCode.IsValid:
                        gothicPath = path;
                        Save();
                        return true;
                }
            }
            return false;
        }

        enum FailCode
        {
            IsValid,
            GothicNotFound,
            GothicWrongVersion,
            VDFS32NotFound,
            VDFS32WrongVersion,
            SHW32NotFound,
            SHW32WrongVersion,
        }

        // Fixme: english version?
        static FailCode CheckGothicVersion(string path)
        {
            string gothic2 = Path.Combine(path, "System\\Gothic2.exe");
            if (!File.Exists(gothic2))
                return FailCode.GothicNotFound;

            if (!ValidateFileHash(gothic2, HashFile.Gothic2))
                return FailCode.GothicWrongVersion;

            string vdfs32g = Path.Combine(path, "System\\vdfs32g.dll");
            if (!File.Exists(vdfs32g))
                return FailCode.VDFS32NotFound;

            if (!ValidateFileHash(vdfs32g, HashFile.VDFS32g))
                return FailCode.VDFS32WrongVersion;

            string shw32 = Path.Combine(path, "System\\shw32.dll");
            if (!File.Exists(shw32))
                return FailCode.SHW32NotFound;

            if (!ValidateFileHash(shw32, HashFile.SHW32))
                return FailCode.SHW32WrongVersion;

            return FailCode.IsValid;
        }

        #endregion
    }
}
