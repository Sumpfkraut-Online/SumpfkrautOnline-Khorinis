using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Security.Cryptography;

namespace GUCLauncher
{
    static class Configuration
    {
        const string ConfigFile = "Launcher.cfg";

        static ServerListItem activeProject = null;
        public static ServerListItem ActiveProject { get { return activeProject; } }
        public static void SetActiveProject(ServerListItem item)
        {
            activeProject = item;
            Save();
        }

        static string gothicPath;
        public static string GothicPath { get { return gothicPath; } }
        public static string GothicApp { get { return Path.Combine(gothicPath, G2App); } }
        public static string zSpyApp { get { return Path.Combine(gothicPath, @"_work\tools\zSpy\zSpy.exe"); } }

        static ItemCollection items;

        public static void Init(ItemCollection coll)
        {
            items = coll;

            Load();

            CheckGothicPath();
        }

        #region Add & Remove Servers

        public static void AddServer(string address)
        {
            string ip;
            ushort port;
            if (ServerListItem.TryGetAddress(address, out ip, out port))
            {
                items.Add(new ServerListItem(ip, port));
                Save();
            }
        }

        public static void RemoveServer(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                items.RemoveAt(index);
                Save();
            }
        }

        #endregion

        #region Load & Save

        static void Load()
        {
            if (File.Exists(ConfigFile))
            {
                using (StreamReader sr = new StreamReader(ConfigFile))
                {
                    gothicPath = Path.GetFullPath(sr.ReadLine());
                    while (!sr.EndOfStream)
                    {
                        var item = ServerListItem.ReadNew(sr);

                        if (string.Compare(sr.ReadLine(), "active", true) == 0)
                        {
                            activeProject = item;
                        }

                        if (item != null)
                            items.Add(item);
                    }
                }
            }
            else
            {
                gothicPath = "1234";
            }
        }

        public static void Save()
        {
            using (StreamWriter sw = new StreamWriter(ConfigFile))
            {
                sw.WriteLine(gothicPath);
                foreach (ServerListItem item in items)
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

        const string G2App = @"System\Gothic2.exe";

        static void CheckGothicPath()
        {
            System.Windows.Forms.FolderBrowserDialog dlg = null;
            string path = gothicPath;

            while (true)
            {
                if (!File.Exists(Path.Combine(path, G2App)))
                {
                    if (path != null)
                    {
                        MessageBox.Show("Gothic 2 konnte nicht gefunden werden. Bitte wähle dein Gothic 2 DNDR - Verzeichnis aus.", "Gothic 2 Ordner suchen", MessageBoxButton.OK);
                    }

                    if (dlg == null)
                    {
                        dlg = new System.Windows.Forms.FolderBrowserDialog();
                        dlg.SelectedPath = Directory.Exists(path) ? path : Directory.GetCurrentDirectory();
                        dlg.Description = "Gothic 2 Ordner suchen";
                    }

                    if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        Application.Current.Shutdown();
                        return;
                    }

                    path = dlg.SelectedPath;
                }

                if (IsGothic2(Path.Combine(path, G2App)))
                {
                    break;
                }
                else
                {
                    MessageBox.Show("Chosen application is not Gothic2 2.6.0.0!", "Wrong version!", MessageBoxButton.OK);
                    path = null;
                }
            }

            if (string.Compare(gothicPath, path, true) != 0)
            {
                gothicPath = path;
                Save();
            }
        }

        static readonly byte[] GothicMD5 = new byte[16] { 0x3C, 0x43, 0x6B, 0xD1, 0x99, 0xCA, 0xAA, 0xA6, 0x4E, 0x97, 0x36, 0xE3, 0xCC, 0x1C, 0x9C, 0x32 };
        public static bool IsGothic2(string fileName)
        {
            byte[] hash;
            using (Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                hash = md5.ComputeHash(fs);
            }

            if (hash.SequenceEqual(GothicMD5))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
