using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinApi;
using WinApi.Kernel;
using System.IO;
using Gothic_Untold_Chapter.Forms;
using WinApi.FileFormat;
using GUC.Options;

namespace GMPStarter
{
    public partial class Form1 : Form
    {
        public ClientOptions co;

        public void initDefaultFolders()
        {
            if(!Directory.Exists("./conf"))
                Directory.CreateDirectory("./conf");
            if (!Directory.Exists("./DLL"))
                Directory.CreateDirectory("./DLL");
            if (!Directory.Exists("./Log"))
                Directory.CreateDirectory("./Log");
            if (!Directory.Exists("./tempScripts"))
                Directory.CreateDirectory("./tempScripts");
            if (!Directory.Exists("./temp_guc"))
                Directory.CreateDirectory("./temp_guc");
            if (!Directory.Exists("./Data"))
                Directory.CreateDirectory("./Data");
        }
        public Form1()
        {
            InitializeComponent();

            initDefaultFolders();

            try
            {
                co = ClientOptions.Load("./conf/gmp.xml");
            }
            catch (Exception ex)
            {
                co = new ClientOptions();
                co.Save("./conf/gmp.xml");
            }
            textBox1.Text = co.name;
            textBox2.Text = co.ip;
            textBox3.Text = co.port+"";
            textBox4.Text = co.password;

            textBox5.Text = co.fps + "";
            checkBox2.Checked = co.startWindowed;

            if (co.loglevel >= 0)
                zLogLevel.Value = co.loglevel;
        }
        
        private void mBStart_Click(object sender, EventArgs e)
        {
            if (!Datei2MD5("../Gothic2.exe", "3c436bd199caaaa64e9736e3cc1c9c32"))// &&  !Datei2MD5("../Gothic2.exe", "b75d03422af54286f1f4ed846b8fd4b8")
            {
                MessageBox.Show("Falsche Gothic Version !");
                return;
            }

                if (textBox1.Text.Trim().Length != 0)
                    co.name = textBox1.Text;
                if (textBox2.Text.Trim().Length != 0)
                    co.ip = textBox2.Text;
                if (textBox3.Text.Trim().Length != 0)
                {
                    try { co.port = Convert.ToUInt16(textBox3.Text); }
                    catch (Exception ex) { }
                }
                co.password = textBox4.Text;

                co.loglevel = zLogLevel.Value;
                if (textBox5.Text.Trim().Length == 0)
                    co.fps = -1;
                else
                {
                    try { co.fps = Convert.ToUInt16(textBox5.Text); }
                    catch (Exception ex) { }
                }
                co.startWindowed = checkBox2.Checked;
                co.Save("./conf/gmp.xml");


            String dll =  "UntoldChapter/DLL/NetInject.dll";
            String RakNetDLL = "UntoldChapter/DLL/RakNet.dll";
            
            if (!System.IO.File.Exists("../"+dll))
                throw new Exception(dll+" nicht gefunden");
            if (!System.IO.File.Exists("../"+RakNetDLL))
                throw new Exception(RakNetDLL + " nicht gefunden");

            System.Diagnostics.ProcessStartInfo psi = null;
            //zSpy starten
            if (zLogLevel.Value != -1 && System.IO.File.Exists("..\\..\\_work\\tools\\zSpy\\zSpy.exe"))
            {
                psi = new System.Diagnostics.ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.WorkingDirectory = Environment.CurrentDirectory+"\\Log";

                psi.FileName = "..\\..\\..\\_work\\tools\\zSpy\\zSpy.exe";
                WinApi.Process.Start(psi);
            }


            //Starten...
            psi = new System.Diagnostics.ProcessStartInfo();
            psi.WorkingDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
            psi.Arguments = "-nomenu";

            //if (checkBox1.Checked)
            //    psi.Arguments += " -zreparse";
            if (checkBox2.Checked)
                psi.Arguments += " -zwindow";
            
            if (zLogLevel.Value != -1)
                psi.Arguments += " -zlog:" + zLogLevel.Value + ",s";

            //if (checkBox4.Checked)
            //    psi.Arguments += " -vdfs:physicalfirst";
            if (textBox5.TextLength >= 2 && textBox5.TextLength <= 3)
                psi.Arguments += " -zMaxFrameRate:" + textBox4.Text;
            else
                throw new Exception("Die eingegebene Framerate ist ungültig. Sie sollte im Bereich 24-100 liegen, da diese Option eventuelles Ruckeln im Spiel vermeiden soll.");

            //Weitere:  -ztexconvert  -zautoconvertdata  -zconvertall  -vdfs:physicalfirst

            psi.FileName = "Gothic2.exe";
            WinApi.Process process = WinApi.Process.Start(psi);

            if (process.LoadLibary(dll) == IntPtr.Zero)
                Error.GetLastError();
            
        }

        private bool Datei2MD5(string Dateipfad, string Checksumme)
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

        UpdateLoader2 uL;
        private void Form1_Shown(object sender, EventArgs e)
        {


            if (!co.autoupdate)
                return;
            uL = new UpdateLoader2(this);
            uL.ShowDialog();
            //if (uL.error)
            //    MessageBox.Show("Kein Zugriff auf den Master-Server");

        }
    }
}
