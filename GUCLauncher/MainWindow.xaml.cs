using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Windows.Threading;
using FilePacker;

namespace GUCLauncher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                foreach (TabItem item in tabControl.Items)
                {
                    item.Visibility = Visibility.Hidden;
                }

                LoadServerList();
            }
            catch (Exception e)
            {
                File.WriteAllText("exceptions.txt", e.ToString());
            }
        }

        public class ServerListItem : INotifyPropertyChanged
        {
            string featured;
            public string Featured
            {
                get { return this.featured; }
                set { this.featured = value; Refresh("Featured"); }
            }

            string name;
            public string Name
            {
                get { return this.name; }
                set { this.name = value; Refresh("Name"); }
            }

            string ip;
            public string IP
            {
                get { return this.ip; }
                set { this.ip = value; Refresh("IP"); }
            }

            ushort port;
            public ushort Port
            {
                get { return this.port; }
                set { this.port = value; Refresh("Port"); }
            }

            string ping;
            public string Ping
            {
                get { return this.ping; }
                set { this.ping = value; Refresh("Ping"); }
            }

            string players;
            public string Players
            {
                get { return this.players; }
                set { this.players = value; Refresh("Players"); }
            }

            bool hasPW;
            public bool HasPW
            {
                get { return this.hasPW; }
                set { this.hasPW = value; Refresh("Locked"); }
            }

            byte[] password;
            public byte[] Password
            {
                get { return this.password; }
                set
                {
                    if (value != null && value.Length != 16)
                        throw new ArgumentException("value.Length");
                    this.password = value;
                }
            }
            public void SetPassword(string pw)
            {
                if (pw == null)
                {
                    this.password = null;
                }
                else
                {
                    using (MD5 md5 = new MD5CryptoServiceProvider())
                    {
                        this.password = md5.ComputeHash(Encoding.Unicode.GetBytes(pw));
                    }
                }
            }

            public string Locked { get { return HasPW ? "Resources/lock.png" : string.Empty; } }

            public ServerListItem(string ip, ushort port)
            {
                this.ip = ip;
                this.port = port;
            }

            public ServerListItem(string ip, ushort port, string pw) : this(ip, port)
            {
                SetPassword(pw);
            }

            public event PropertyChangedEventHandler PropertyChanged;
            void Refresh(string val)
            {
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(val));
            }
        }

        static bool TryGetAddress(string str, out string address, out ushort port)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                string[] strs = str.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length == 2)
                {
                    address = strs[0].Trim();
                    if (!string.IsNullOrWhiteSpace(address) && ushort.TryParse(strs[1].Trim(), out port) && port > 1024)
                    {
                        return true;
                    }
                }
            }
            address = null;
            port = 0;
            return false;
        }

        #region Add & Remove Servers

        private void bAddServer_Click(object sender, RoutedEventArgs e)
        {
            string address;
            ushort port;
            if (TryGetAddress(tbAddIP.Text, out address, out port))
            {
                ServerListItem item = new ServerListItem(address, port);
                lvServerList.Items.Add(item);
                SaveServerList();
            }
        }

        private void bRemoveServer_Click(object sender, RoutedEventArgs e)
        {
            if (lvServerList.Items.Count > 0 && lvServerList.SelectedIndex >= 0)
            {
                lvServerList.Items.RemoveAt(lvServerList.SelectedIndex);
                SaveServerList();
            }
        }

        #endregion

        #region Serverlist Saving

        const string ServerFileName = "GUCServers.txt";

        void LoadServerList()
        {
            try
            {
                if (File.Exists(ServerFileName))
                {
                    using (StreamReader sr = new StreamReader(ServerFileName))
                    {
                        while (!sr.EndOfStream)
                        {
                            string addressLine = sr.ReadLine();
                            string passwordLine = sr.ReadLine();

                            string address;
                            ushort port;
                            if (TryGetAddress(addressLine, out address, out port))
                            {
                                ServerListItem item = new ServerListItem(address, port);
                                item.Password = string.IsNullOrEmpty(passwordLine) ? null : Convert.FromBase64String(passwordLine);
                                lvServerList.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("exceptions.txt", e.ToString());
                Close();
            }
        }

        void SaveServerList()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ServerFileName))
                {
                    foreach (ServerListItem item in lvServerList.Items)
                    {
                        sw.WriteLine(item.IP + ":" + item.Port);
                        sw.WriteLine(item.Password == null ? "" : Convert.ToBase64String(item.Password));
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("exceptions.txt", e.ToString());
                Close();
            }
        }

        #endregion

        #region Refresh Server List

        static readonly byte[] NetIDRefresh = new byte[1] { 0 };
        static void Refresh(object obj)
        {
            ServerListItem item = (ServerListItem)obj;

            TcpClient client = new TcpClient();

            try
            {
                if (client.ConnectAsync(item.IP, 9054).Wait(1000))
                {
                    var stream = client.GetStream();
                    if (stream.WriteAsync(NetIDRefresh, 0, 1).Wait(1000))
                    {
                        byte[] buf = new byte[byte.MaxValue];
                        if (stream.ReadAsync(buf, 0, 4).Wait(1000))
                        {
                            int count = buf[0];
                            int slots = buf[1];
                            int pw = buf[2];
                            int nameLen = buf[3];

                            if (stream.ReadAsync(buf, 0, nameLen).Wait(1000))
                            {
                                client.Close();

                                item.Ping = new Ping().Send(item.IP, 1000).RoundtripTime.ToString();
                                item.Name = Encoding.UTF8.GetString(buf, 0, nameLen);

                                item.Players = count + "/" + slots;
                                item.HasPW = pw > 0;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("exceptions.txt", e.ToString());
            }

            client.Close();
            item.Ping = "Offline";
        }

        private void bRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (ServerListItem item in lvServerList.Items)
                {
                    ThreadPool.QueueUserWorkItem(Refresh, item);
                }

            }
            catch (Exception e2)
            {
                lvServerList.Items.Add(e2);
            }
        }

        #endregion

        #region Start Project

        ServerListItem selectedProject = null;
        private void bConnect_Click(object sender, RoutedEventArgs e)
        {
            if (lvServerList.SelectedIndex < 0)
                return;

            selectedProject = (ServerListItem)lvServerList.SelectedItem;

            lPWWrong.Content = null;
            TryOpenProjectPage();
        }

        #endregion

        void ShowServerList()
        {
            tabControl.SelectedIndex = 0;
        }

        #region Password

        void ShowPasswordPage()
        {
            lPWTitle.Content = selectedProject.Name;
            lPWIP.Content = selectedProject.IP;

            tabControl.SelectedIndex = 2;
        }

        void bPasswordOK_Click(object sender, RoutedEventArgs e)
        {
            selectedProject.SetPassword(tbPWInput.Text);
            SaveServerList();
            TryOpenProjectPage();
        }

        #endregion

        #region Project page

        string dlLink = null;
        void TryOpenProjectPage()
        {
            if (selectedProject.HasPW && selectedProject.Password == null)
                ShowPasswordPage();

            TcpClient client = new TcpClient();
            if (client.ConnectAsync(selectedProject.IP, 9054).Wait(1000))
            {
                var stream = client.GetStream();
                byte[] buf = new byte[byte.MaxValue];
                buf[0] = 1;
                if (selectedProject.Password != null)
                {
                    Array.Copy(selectedProject.Password, 0, buf, 1, 16);
                }

                if (stream.WriteAsync(buf, 0, 17).Wait(1000))
                {
                    if (stream.ReadAsync(buf, 0, 1).Wait(1000))
                    {
                        if (buf[0] == 0)
                        {
                            // falsches PW
                            lPWWrong.Content = "Falsches Passwort!";
                            client.Close();
                            ShowPasswordPage();
                            return;
                        }
                        else if (stream.ReadAsync(buf, 0, 1).Wait(1000))
                        {
                            int byteLen = buf[0];
                            if (stream.ReadAsync(buf, 0, byteLen).Wait(1000))
                            {
                                dlLink = Encoding.UTF8.GetString(buf, 0, byteLen);
                                client.Close();
                                ShowProjectPage();
                                return;
                            }
                        }
                    }
                }
            }
            //Could not connect
            Title = "Could not connect";
            client.Close();
        }

        void ShowProjectPage()
        {
            try
            {
                lProjectTitle.Content = selectedProject.Name;
                lProjectIP.Content = selectedProject.IP;

                tabControl.SelectedIndex = 1;

                progressBar.Value = 0;

                ThreadPool.QueueUserWorkItem(CheckForUpdates);
            }
            catch (Exception e)
            {
                File.WriteAllText("exceptions.txt", e.ToString());
            }
        }

        #endregion

        void bBack_Click(object sender, RoutedEventArgs e)
        {
            ShowServerList();
        }

        void SetProgress(double value)
        {
            Dispatcher.Invoke(() => progressBar.Value = value, DispatcherPriority.Render);
        }

        void SetProgressText(string text)
        {
            Dispatcher.Invoke(() => lUpdate.Content = text, DispatcherPriority.Render);
        }

        void SetWebsite(string text)
        {

        }

        void SetInfoText(string text)
        {
            Dispatcher.Invoke(() => textBlock.Text = text, DispatcherPriority.Render);
        }

        void SetImage(byte[] data)
        {
            Dispatcher.Invoke(() =>
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    try
                    {
                        // try to create the image
                        BitmapImage bmi = new BitmapImage();
                        bmi.BeginInit();
                        bmi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        bmi.CacheOption = BitmapCacheOption.OnLoad;
                        bmi.UriSource = null;
                        bmi.StreamSource = ms;
                        bmi.EndInit();
                        bmi.Freeze();

                        // update the image
                        image.Source = bmi;
                    }
                    catch
                    {
                    }
                }
            }, DispatcherPriority.Render);
        }
        
        void CheckForUpdates(object arg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dlLink))
                {
                    SetProgressText("Server disabled automatic updates.");
                    SetProgress(100);
                    return;
                }

                SetProgressText("Connecting to update server...");
                // load the information file
                Stream stream;
                double percent = 0;
                if (Download.TryGetStream(dlLink, out stream, value => { percent = value * 0.9; SetProgress(percent); }))
                {
                    SetProgressText("Loading update infos...");
                    InfoPack info = new InfoPack();
                    info.Read(stream, value => SetProgress(value * (100 - percent) + percent));
                    SetInfoText(info.InfoText);
                    SetImage(info.ImageData);

                    SetProgressText("Finished.");
                }
                else
                {
                    SetProgressText("Could not connect to update server.");
                    SetProgress(100);
                    return;
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("exceptions.txt", e.ToString());
            }
        }
    }
}
