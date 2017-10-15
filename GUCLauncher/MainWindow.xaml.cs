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
using System.Diagnostics;

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

                Configuration.Init(lvServerList.Items);
                if (Configuration.ActiveProject != null)
                    TryOpenProjectPage(Configuration.ActiveProject, Configuration.ActiveProject.Password);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("{0}: {1}\r\n{2}", e.Source, e.Message, e.StackTrace), e.GetType().ToString(), MessageBoxButton.OK);
                Application.Current.Shutdown();
            }
        }

        string ShowInputBox(string title, string input = "")
        {
            overshadow.Visibility = Visibility.Visible;
            string str = InputBox.Show(this, title, input);
            overshadow.Visibility = Visibility.Hidden;
            return str;
        }

        void ShowInfoBox(string title)
        {
            overshadow.Visibility = Visibility.Visible;
            InfoBox.Show(this, title);
            overshadow.Visibility = Visibility.Hidden;
        }

        #region Add & Remove Servers

        void bAddServer_Click(object sender, RoutedEventArgs e)
        {
            var address = ShowInputBox("Server hinzufügen");
            if (!string.IsNullOrWhiteSpace(address))
            {
                Configuration.AddServer(address);
            }
        }

        void bRemoveServer_Click(object sender, RoutedEventArgs e)
        {
            Configuration.RemoveServer(lvServerList.SelectedIndex);
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

                                item.Name = Encoding.UTF8.GetString(buf, 0, nameLen);
                                item.Players = count + "/" + slots;
                                item.HasPW = pw > 0;
                                item.Ping = new Ping().Send(item.IP, 1000).RoundtripTime.ToString();
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

        public static bool ServerWentOnline = false;
        void bRefresh_Click(object sender, RoutedEventArgs e)
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

        void bConnect_Click(object sender, RoutedEventArgs args)
        {
            try
            {
                if (lvServerList.SelectedIndex < 0)
                    return;

                ServerListItem item = (ServerListItem)lvServerList.SelectedItem;
                TryOpenProjectPage(item, item.Password);
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        #endregion

        void ShowServerList()
        {
            if (updateThread != null)
            {
                updateThread.Abort();
                updateThread.Join();
            }
            Configuration.SetActiveProject(null);
            projectGrid.Visibility = Visibility.Hidden;
            serverGrid.Visibility = Visibility.Visible;
        }

        #region Password

        void ShowPasswordPage(ServerListItem item, bool wrongPW = false)
        {
            if (!this.IsLoaded)
                return;

            var pw = ShowInputBox("Passwort benötigt!");
            if (pw == null)
                return;

            byte[] hash;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                hash = md5.ComputeHash(Encoding.Unicode.GetBytes(pw));
            }

            TryOpenProjectPage((ServerListItem)lvServerList.SelectedItem, hash);
        }

        #endregion

        #region Project page

        string dlLink = null;
        void TryOpenProjectPage(ServerListItem item, byte[] password)
        {
            if (item.HasPW && password == null)
            {
                ShowPasswordPage(item);
                return;
            }

            TcpClient client = new TcpClient();
            if (client.ConnectAsync(item.IP, item.Port).Wait(1000))
            {
                var stream = client.GetStream();
                byte[] buf = new byte[byte.MaxValue];
                buf[0] = 1;

                if (password != null)
                {
                    Array.Copy(password, 0, buf, 1, 16);
                }

                if (stream.WriteAsync(buf, 0, 17).Wait(1000))
                {
                    if (stream.ReadAsync(buf, 0, 1).Wait(1000))
                    {
                        if (buf[0] == 0)
                        {
                            // wrong password
                            client.Close();
                            ShowPasswordPage(item, true);
                            return;
                        }
                        else if (stream.ReadAsync(buf, 0, 1).Wait(1000))
                        {
                            // correct password!
                            item.Password = password;
                            Configuration.Save();

                            int byteLen = buf[0];
                            if (stream.ReadAsync(buf, 0, byteLen).Wait(1000))
                            {
                                dlLink = Encoding.UTF8.GetString(buf, 0, byteLen);
                                client.Close();
                                ShowProjectPage(item);
                                return;
                            }
                        }
                    }
                }
            }
            //Could not connect
            if (this.IsLoaded)
                ShowInfoBox("Verbindung konnte nicht hergestellt werden.");
            client.Close();
        }

        void ShowProjectPage(ServerListItem item)
        {
            try
            {
                Configuration.SetActiveProject(item);
                lProjectTitle.Content = item.Name;
                lProjectIP.Content = item.IP + " : " + item.Port;

                projectGrid.Visibility = Visibility.Visible;
                serverGrid.Visibility = Visibility.Hidden;

                progressBar.Value = 0;
                image.Source = null;
                textBlock.Text = null;
                bWebsite.IsEnabled = false;
                SetStartButton(StartButtonSetting.Disabled);

                updateThread = new Thread(CheckForUpdates);
                updateThread.Start();
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

        enum StartButtonSetting
        {
            Disabled,
            Update,
            Start
        }

        void SetStartButton(StartButtonSetting setting)
        {
            switch (setting)
            {
                case StartButtonSetting.Disabled:
                    bStart.IsEnabled = false;
                    bStart.Content = "Start";
                    bStart.Click -= ClickStart;
                    bStart.Click -= ClickUpdate;
                    break;
                case StartButtonSetting.Update:
                    bStart.IsEnabled = true;
                    bStart.Content = "Update";
                    bStart.Click -= ClickStart;
                    bStart.Click += ClickUpdate;
                    break;
                case StartButtonSetting.Start:
                    bStart.IsEnabled = true;
                    bStart.Content = "Start";
                    bStart.Click += ClickStart;
                    bStart.Click -= ClickUpdate;
                    break;
            }
        }

        void SetImage(byte[] data)
        {
            if (data == null || data.Length == 0)
                image.Source = null;
            else
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
            }
        }

        float oldPercent = 0;
        void SetPercent(float percent)
        {
            if ((int)((oldPercent - percent) * 1000) != 0)
            {
                Dispatcher.Invoke(() => progressBar.Value = percent * 100);
                oldPercent = percent;
            }
        }

        Thread updateThread;
        InfoPack current = new InfoPack();
        void CheckForUpdates(object arg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dlLink))
                {
                    Dispatcher.Invoke(() =>
                    {
                        lUpdate.Content = "Server disabled automatic updates.";
                        SetStartButton(StartButtonSetting.Start);
                        progressBar.Value = 100;
                    });
                    return;
                }

                Dispatcher.Invoke(() => lUpdate.Content = "Connecting to update server...");
                // load the information file
                float percent = 0;
                using (var response = Download.GetResponse(dlLink, value =>
                {
                    percent = value * 0.70f;
                    SetPercent(percent);
                }))
                {
                    Stream stream;
                    if (response != null && (stream = response.GetResponseStream()) != null)
                    {
                        Dispatcher.Invoke(() => lUpdate.Content = "Loading update infos...");
                        current = new InfoPack();
                        current.Read(stream, Configuration.ActiveProject.GetFolder(), UpdateUI,
                        value =>
                        {
                            SetPercent(value * (1 - percent) + percent);
                        },
                        str =>
                        {
                            Dispatcher.Invoke(() => lUpdate.Content = str);
                        });

                        Dispatcher.Invoke(() =>
                        {
                            if (current.NeedsUpdate())
                            {
                                SetStartButton(StartButtonSetting.Update);
                                lUpdate.Content = "Update needed.";
                            }
                            else
                            {
                                SetStartButton(StartButtonSetting.Start);
                                lUpdate.Content = "Finished.";
                            }
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            lUpdate.Content = "Could not connect to update server.";
                            progressBar.Value = 100;
                        });
                    }
                }
            }
            catch (Exception e)
            {
                if (!(e is ThreadAbortException || e is TaskCanceledException))
                    ShowException(e);
            }
        }

        void UpdateUI(InfoPack.UpdateUIEnum type)
        {
            switch (type)
            {
                case InfoPack.UpdateUIEnum.Website:
                    if (IsValidLink(current.Website))
                        Dispatcher.Invoke(() => bWebsite.IsEnabled = true);
                    break;
                case InfoPack.UpdateUIEnum.InfoText:
                    Dispatcher.Invoke(() => textBlock.Text = current.InfoText);
                    break;
                case InfoPack.UpdateUIEnum.Image:
                    Dispatcher.Invoke(() => SetImage(current.ImageData));
                    break;
            }
        }

        bool IsValidLink(string url)
        {
            if (!string.IsNullOrWhiteSpace(current.Website))
            {
                Uri uri;
                if (Uri.TryCreate(current.Website, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    return true;
                }
            }
            return false;
        }

        void bWebsite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(current.Website);
            }
            catch { }
        }


        void ClickUpdate(object sender, RoutedEventArgs args)
        {
            SetStartButton(StartButtonSetting.Disabled);
            progressBar.Value = 0;
            lUpdate.Content = "Preparing update...";

            updateThread = new Thread(() =>
            {
                try
                {
                    current.DoUpdate(SetPercent,
                    str =>
                    {
                        Dispatcher.Invoke(() => lUpdate.Content = str);
                    });

                    Dispatcher.Invoke(() =>
                    {
                        SetStartButton(StartButtonSetting.Start);
                    });
                }
                catch (Exception e)
                {
                    if (!(e is ThreadAbortException || e is TaskCanceledException))
                        ShowException(e);
                }
            });
            updateThread.Start();
        }

        void ClickStart(object sender, RoutedEventArgs args)
        {
            try
            {
                var project = Configuration.ActiveProject;
                GothicStarter.Start(project.GetFolder(), project.IP, project.Port, project.Password);
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        void ShowException(Exception e)
        {
            MessageBox.Show(string.Format("{0}: {1}\r\n{2}", e.Source, e.Message, e.StackTrace), e.GetType().ToString(), MessageBoxButton.OK);
        }

        void Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Click_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        bool mDown = false;
        Point lastPos;
        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point newPos = e.GetPosition(this);
            if (mDown)
            {
                this.Left -= lastPos.X - newPos.X;
                this.Top -= lastPos.Y - newPos.Y;
            }
        }

        void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                lastPos = e.GetPosition(this);
                mDown = true;
            }
        }

        void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                mDown = false;
            }
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    bConnect_Click(null, null);
                    break;
                case Key.F5:
                    bRefresh_Click(null, null);
                    break;
                case Key.Delete:
                    bRemoveServer_Click(null, null);
                    break;
            }
        }

        void lvServerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            while (obj != null && obj != lvServerList)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    bConnect_Click(null, null);
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }
    }
}
