using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUC.Options;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Ionic.Zip;

namespace GMPStarter
{
    public partial class FilesLoader : Form
    {
        ClientOptions co;
        String nickname;
        String ip;
        ushort port;
        String serverpassword;
        int logLevel;
        int maxFPS;
        bool startWindowed;

        StatusMessage sm;

        public FilesLoader(ClientOptions co, String nickname, String ip, ushort port, String serverpassword, int logLevel, int maxFPS, bool startWindowed)
        {
            InitializeComponent();

            this.co = co;
            this.nickname = nickname;
            this.ip = ip;
            this.port = port;
            this.serverpassword = serverpassword;
            this.logLevel = logLevel;
            this.maxFPS = maxFPS;
            this.startWindowed = startWindowed;
            this.locked_Progress = 0;

            sm = new StatusMessage(this.ip, this.port);
            sm.startGetFiles();

            label1.Text = "Dateiliste wird gedownloaded";

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sm.isLoaded && !stateDownload)
            {
                if (sm.Files.Count == 0)
                {
                    label1.Text = "No files found";
                    this.start();
                    return;
                }

                String[] files = sm.Files.Values.ToArray();
                List<String> filesToDownload = new List<string>();

                String folder = "./Downloads/" + ip + "_" + port+"/";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                foreach (KeyValuePair<String, String> pair in sm.Files)
                {
                    Uri url = null;
                    bool isUrl = Uri.TryCreate(pair.Key, UriKind.Absolute, out url);

                    if (!isUrl || url == null)
                        continue;

                    String filename = url.LocalPath;
                    if (!File.Exists(folder + filename))
                    {
                        filesToDownload.Add(pair.Key);
                        continue;
                    }

                    if (!GUC.Updater.Updater.Datei2MD5(folder + filename, pair.Value))
                    {
                        File.Delete(folder + filename);
                        filesToDownload.Add(pair.Key);
                        continue;
                    }
                }

                if (filesToDownload.Count == 0)
                {
                    label1.Text = "No files found";
                    this.start();
                    return;
                }

                this.progressBar1.Maximum = filesToDownload.Count;
                label1.Text = "Downloade Daten";
                foreach (String file in filesToDownload)
                {
                    Uri url = new Uri(file);
                    ThreadPool.QueueUserWorkItem(downloadFile, new object[] { file, folder + url.LocalPath});
                }

                stateDownload = true;

            }
            else if (stateDownload)
            {
                lock (lockThis)
                {
                    this.progressBar1.Value = locked_Progress;

                    if (this.progressBar1.Maximum <= locked_Progress)
                    {
                        foreach (String file in sm.Files.Values)
                        {
                            if (file.ToUpper().EndsWith(".DLL"))
                            {
                                //Some Message
                                DialogResult dr = MessageBox.Show("Der Server nutzt Client-Scripte. Diese können Potenziell gefährliche Funktionen bieten (Viren etc.)\r\n Möchest du dich dennoch mit dem Server verbinden? ", "Warnung", MessageBoxButtons.YesNo);
                                if (dr != System.Windows.Forms.DialogResult.Yes)
                                {
                                    timer1.Stop();
                                    this.Close();
                                    return;
                                }
                            }
                        }

                        label1.Text = "Download finished!";
                        this.start();
                    }
                }
            }
        }

        protected bool stateDownload = false;
        protected int locked_Progress = 0;
        private System.Object lockThis = new System.Object();
        public void downloadFile(object state)
        {
            object[] array = state as object[];
            String file = (String)array[0];
            String path = (String)array[1];

            Uri url = new Uri(file);
            WebClient webClient = new WebClient();
            webClient.DownloadFile(file, path);

            if (path.Trim().ToUpper().EndsWith("ZIP"))
            {

                using (ZipFile zf = new ZipFile(path))
                {
                    zf.ExtractAll(Path.GetDirectoryName(path));
                }
                //ZipFile.(path, extractPath);
            }

            lock (lockThis)
            {
                locked_Progress += 1;
            }
        }

        public void start()
        {
            timer1.Stop();
            
            try
            {
                StarterFunctions.StartGothic(co, nickname, ip, port, serverpassword, logLevel, maxFPS, startWindowed);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error occurred while starting GUC: " + ex.ToString());
            }

            this.Close();
        }
    }
}
