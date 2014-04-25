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
using GMPStarter.Forms;
using GMPStarter;
using GUC.Updater;

namespace Gothic_Untold_Chapter.Forms
{
    public partial class UpdateLoader2 : Form
    {
        Timer timer = null;
        Updater updater = new Updater();

        public Form1 form = null;
        public UpdateLoader2(Form1 form)
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 10;
            timer.Enabled = true;
            timer.Tick += new EventHandler(tick);

            this.form = form;

            if (!String.IsNullOrEmpty(form.co.updateUser) &&
                !String.IsNullOrEmpty(form.co.updatePassword))
            {
                updater.setAuthData(form.co.updateUser, form.co.updatePassword);
            }
        }

        int state = 0;
        int downloadID = 0;
       
        public void tick(object obj, EventArgs args)
        {
            timer.Enabled = false;

            WebException ex = null;
            switch (state)
            {
                case 0:
                {
                    while ((ex = updater.dlUpdateList()) != null)
                    {
                        if (!Error(ex))
                            return;
                    }

                    state = 1;
                    break;
                }
                case 1://Überprüfen ob etwas geupdatet werden muss
                {
                    if (updater.IsUpdateNeeded())
                        state = 2;
                    else
                    {
                        state = 9999;//Abbrechen
                        updater.finish(false);
                    }
                    break;
                }
                case 2:
                {
                    progressBar1.Maximum = updater.filesToUpdate();
                    progressBar1.Value = downloadID+1;
                    try
                    {
                        while ((ex = updater.dlFile(downloadID)) != null)
                        {
                            if (!Error(ex))
                                return;
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                        this.Close();
                        return;
                    }

                    downloadID += 1;
                    if (updater.isFileListEnd(downloadID))
                    {
                        state = 3;
                    }
                    break;
                }
                case 3:
                {
                    updater.finish(true);
                    timer.Enabled = false;
                    this.Close();
                    Application.Exit();
                    return;
                    state = 9999;
                    break;
                }
                default:
                {
                    timer.Enabled = false;
                    this.Close();
                    break;
                }
            }

            timer.Enabled = true;
        }

        public bool Error(WebException ex)
        {
            if (ex.Message.Contains("(401)"))
            {
                AuthRequest ar = new AuthRequest();
                ar.ShowDialog();
                if (ar.loggedIn == true)
                {
                    updater.setAuthData(ar.username, ar.password);
                    form.co.updateUser = ar.username;
                    form.co.updatePassword = ar.password;
                    form.co.Save();
                    return true;
                }
                else
                {
                    MessageBox.Show("Das Updaten wurde abgebrochen!");
                    this.Close();
                    return false;
                }
            }
            else
            {
                MessageBox.Show(ex.Message + "| " + ex.Status);
                MessageBox.Show("Das Updaten wurde abgebrochen!");
                this.Close();
                return false;
            }
            return false;
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
