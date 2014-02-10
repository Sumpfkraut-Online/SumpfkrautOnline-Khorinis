using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using WinApi;
using System.Reflection;

namespace Gothic_Untold_Chapter.Forms
{
    public partial class UpdateLoader : Form
    {
        Timer timer;
        WebClient wc;
        public UpdateLoader()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 10;
            timer.Enabled = true;
            timer.Tick += new EventHandler(tick);

            wc = new WebClient();
            while (File.Exists("update.bat"))
            {
                try
                {
                    File.Delete("update.bat");
                }
                catch (Exception ex) { }
            }
            myWriter = File.CreateText("update.bat");
            //myWriter.WriteLine("@echo off ");
        }

        public Options op;
        public ServerList sL;

        byte updateType = 0;
        int fileID = 0;
        public bool error = false;

        StreamWriter myWriter;
        bool updated;
        public void tick(object obj, EventArgs args)
        {
            if (updateType == 0)
            {
                try
                {
                    wc.DownloadFile("http://gothic.mainclain.de/mc/updates/update.xml", GetAppTempPath() + "\\update.xml");
                }
                catch (WebException ex)
                {
                    timer.Enabled = false;
                    error = true;
                    this.Close();
                    return;
                }
                op = Options.Load(GetAppTempPath()  + "\\update.xml");

                if (File.Exists(GetAppTempPath() + "\\update.xml"))
                    File.Delete(GetAppTempPath() + "\\update.xml");

                this.progressBar1.Value = 100;
                updateType = 1;
            }
            else if (updateType == 1)
            {
                if (fileID == op.Files.Length)
                {
                    updateType = 2;
                    return;
                }
                updateFile uf = op.Files[fileID];

                if (!File.Exists("../" + uf.filePath[0]) || !Datei2MD5("../" + uf.filePath[0], uf.md5))
                {
                    if (File.Exists(GetAppTempPath() + "\\files\\" + uf.filename))
                        File.Delete(GetAppTempPath() + "\\files\\" + uf.filename);
                    wc.DownloadFile("http://gothic.mainclain.de/mc/updates/files/" + uf.filename, GetAppTempPath() + "\\files\\"+ uf.filename);

                    try
                    {
                        if (!File.Exists(GetAppTempPath() + "\\files\\" + uf.filename))
                        {
                            MessageBox.Show("Datei konnte nicht gefunden werden! " + GetAppTempPath() + "\\files\\" + uf.filename);
                        }
                        else if (!Datei2MD5(GetAppTempPath() + "\\files\\" + uf.filename, uf.md5))
                        {
                            MessageBox.Show("Scheinbar alte Datei! " + GetAppTempPath() + "\\files\\" + uf.filename);
                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show("Fehler: "+ex.ToString()); }
                    
                    for (int i = 0; i < uf.filePath.Length; i++)
                    {
                        updated = true;
                        uf.filePath[i] = "../" + uf.filePath[i];
                        CreateDir(uf.filePath[i]);

                        String opath = GetAppTempPath() + "\\files\\" + uf.filename;
                       

                        myWriter.WriteLine(":schleife_" + fileID + "_"+i);
                        myWriter.WriteLine("del \"" + Path.GetFullPath(uf.filePath[i]) + "\"");
                        myWriter.WriteLine("if exist \"" + Path.GetFullPath(uf.filePath[i]) + "\" goto schleife_" + fileID + "_" + i);
                        myWriter.WriteLine("copy \"" + opath + "\" \"" + Path.GetFullPath(uf.filePath[i]) + "\"");

                    }
                    myWriter.WriteLine("del \"" + GetAppTempPath() + "\\files\\" + uf.filename + "\"");
                }

                fileID++;
                progressBar1.Value = 100 / (op.Files.Length + 1 + 1) * (fileID + 1);
            }
            else if (updateType == 2)
            {
                
                wc.DownloadFile("http://gothic.mainclain.de/mc/serverlist/serverlist.php", GetAppTempPath() + "\\serverlist.xml");
                sL = ServerList.Load(GetAppTempPath() + "\\serverlist.xml");

                if (File.Exists(GetAppTempPath() + "\\serverlist.xml"))
                    File.Delete(GetAppTempPath() + "\\serverlist.xml");

                progressBar1.Value = 100 / (op.Files.Length + 1 + 1) * (fileID + 1 + 1);
                updateType = 3;
            }
            else
            {
                
                myWriter.WriteLine("GMPStarter.exe");
                //myWriter.WriteLine("if exist \"update.bat\" del \"update.bat\"");
                myWriter.WriteLine("exit");
                myWriter.Close();

                if (updated)
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("update.bat");
                    psi.CreateNoWindow = true;
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    Process.Start(psi);
                    
                    Application.Exit();
                }
                else
                {
                    try
                    {
                        if (File.Exists("update.bat"))
                            File.Delete("update.bat");
                    }
                    catch (Exception ex) { }
                }
                timer.Enabled = false;
                this.Close();
            }
        }

        private static bool Datei2MD5(string Dateipfad, string Checksumme)
        {
            //Datei einlesen
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(Dateipfad);
            // MD5-Hash aus dem Byte-Array berechnen
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();

            //in string wandeln
            string Berechnet = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            // Vergleichen
            if (Berechnet == Checksumme.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CreateDir(String dir)
        {
            dir = Path.GetFullPath(dir);
            dir = Path.GetDirectoryName(dir);
                
            String rDir = dir;
            List<String> dirnames = new List<string>();
            while(!Directory.Exists(rDir))
            {
                dirnames.Add(rDir);
                rDir = Path.GetDirectoryName(rDir);
            }
            for (int i = 0; i < dirnames.Count; i++)
            {
                Directory.CreateDirectory(dirnames[dirnames.Count - 1 - i]);
            }
        }

        /// <summary>
        /// Creates a Temp Directory specified for the Application if don't exists.
        /// </summary>
        /// <returns>Returns specified Temp Directory.</returns>
        public static string GetAppTempPath()
        {
            if (!Directory.Exists(Path.Combine(Path.GetTempPath(), Application.ProductName)))
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Application.ProductName));
            }
            if (!Directory.Exists(Path.Combine(Path.Combine(Path.GetTempPath(), Application.ProductName), "files")))
            {
                Directory.CreateDirectory(Path.Combine(Path.Combine(Path.GetTempPath(), Application.ProductName), "files"));
            }
            return Path.Combine(Path.GetTempPath(), Application.ProductName);
        }
    }
}
