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
using System.Windows.Threading;
using System.IO;

namespace Launcher2
{
    public partial class MainWindow : Window
    {
        private SolidColorBrush standardColorBrush;
        private TextBlock marked;

        const int STATUS_ERROR = -1;
        const int STATUS_SEARCHING = 0;
        const int STATUS_NEEDSUPDATE = 1;
        const int STATUS_UPDATING = 2;
        const int STATUS_FINISHED = 3;
        private int STATUS;

        private bool newLauncher;
        private bool restartLauncher;

        public MainWindow()
        {
            InitializeComponent();
            standardColorBrush = new SolidColorBrush(Color.FromRgb(255, 223, 176));

            marked = BtnSettings;
            marked.Foreground = Brushes.White;
            TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;

            STATUS = STATUS_SEARCHING;
            newLauncher = false;
            restartLauncher = false;

            Global.CheckAdministrator();
            Global.Paths = new G2Paths();
            Global.Progress = new ProgressHandler(SetProgress, SetButtonStartError);
            Global.Ping = new ServerPing(ServerStatus);

            //For WinApi.dll
            AppDomain.CurrentDomain.AssemblyResolve += Global.AssemblyResolve;
        }






        /****************************************************************************************************************
                                                       WPF EVENTS
         ****************************************************************************************************************/

        private void Window_Activated(object sender, EventArgs e)
        {
            StopBlink();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            StopBlink();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckForUpdates();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (marked == BtnStart && (STATUS == STATUS_ERROR || STATUS == STATUS_UPDATING))
            {
                marked.Foreground = Brushes.Gray;
            }
            else
            {
                marked.Foreground = standardColorBrush; //alt
            }
            marked = (TextBlock)sender;
            marked.Foreground = Brushes.White; //neu
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            StopBlink();
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
            else if (e.Key == Key.Enter)
            {
                Button_MouseLeftButtonDown(marked, null);
            }
            else if (e.Key == Key.Up)
            {
                if (marked.Name == BtnStart.Name) Button_MouseEnter(BtnClose, null);
                else if (marked.Name == BtnSettings.Name) Button_MouseEnter(BtnStart, null);
                else if (marked.Name == BtnWebsite.Name) Button_MouseEnter(BtnSettings, null);
                else if (marked.Name == BtnClose.Name) Button_MouseEnter(BtnWebsite, null);
            }
            else if (e.Key == Key.Down)
            {
                if (marked.Name == BtnStart.Name) Button_MouseEnter(BtnSettings, null);
                else if (marked.Name == BtnSettings.Name) Button_MouseEnter(BtnWebsite, null);
                else if (marked.Name == BtnWebsite.Name) Button_MouseEnter(BtnClose, null);
                else if (marked.Name == BtnClose.Name) Button_MouseEnter(BtnStart, null);
            }
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((TextBlock)sender).Name == BtnStart.Name)
            {
                if (STATUS == STATUS_NEEDSUPDATE)
                {
                    Update();
                }
                else if (STATUS == STATUS_FINISHED)
                {
                    GUC.Start(info.serverIP);
                }
            }
            else if (((TextBlock)sender).Name == BtnSettings.Name)
            {
                GUC.Start("127.0.0.1");
                //GUC.StartOffline();
            }
            else if (((TextBlock)sender).Name == BtnWebsite.Name)
            {
                System.Diagnostics.Process.Start("http://forum.worldofplayers.de/forum/forums/846-SumpfkrautOnline-Khorinis");
            }
            else if (((TextBlock)sender).Name == BtnClose.Name)
            {
                Application.Current.Shutdown();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StopBlink();
            try { DragMove(); }
            catch { }
        }










        /****************************************************************************************************************
                                                       Code Funktionen
         ****************************************************************************************************************/

        private void SetButtonStartError(string statusText)
        {
            STATUS = STATUS_ERROR;

            ProgBar.Width = 0;
            TaskbarItemInfo.ProgressValue = 1.0f;
            TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Error;
            StatusText.Text = statusText;
            BtnStart.MouseLeftButtonDown -= Button_MouseLeftButtonDown;
            if (marked == BtnStart) marked = BtnClose;
            Button_MouseEnter(BtnClose, null);

            if (WindowState == WindowState.Minimized)
            {
                StartBlink();
            }
        }

        private void SetButtonStartUpdate()
        {
            STATUS = STATUS_NEEDSUPDATE;

            StatusText.Text = "Neue Version vorhanden.";
            BtnStart.MouseEnter += Button_MouseEnter;
            BtnStart.MouseLeftButtonDown += Button_MouseLeftButtonDown;
            BtnStart.Text = "Aktualisieren";
            Button_MouseEnter(BtnStart, null);
        }

        private void SetButtonStartStart()
        {
            STATUS = STATUS_FINISHED;

            StatusText.Text = "Fertig.";
            BtnStart.MouseEnter += Button_MouseEnter;
            BtnStart.MouseLeftButtonDown += Button_MouseLeftButtonDown;
            BtnStart.Text = "Spiel starten";
            Button_MouseEnter(BtnStart, null);
        }

        private InformationFile info;
        private async void CheckForUpdates()
        {
            Global.Progress.StartFakeProgress(5000, 0.5f);

            StatusText.Text = "Aktualität wird überprüft...";

            info = new InformationFile();
            await info.GetInfoFile();

            if (STATUS != STATUS_ERROR)
            {
                if (info.needsUpdate)
                {
                    SetButtonStartUpdate();
                }
                else
                {
                    SetButtonStartStart();
                }
            }
        }

        private void Update()
        {
            Global.Progress.StartFakeProgress(5000, 0.1f);

            STATUS = STATUS_UPDATING;

            StatusText.Text = "Aktualisieren...";

            BtnStart.Text = "Spiel starten";
            BtnStart.MouseLeftButtonDown -= Button_MouseLeftButtonDown;

            try
            {
                info.Update();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Source + "\n" + e.Message);
            }
        }

        private void SetProgress(float percent)
        {
            if (percent == -1.0f)
            {
                TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal; //in case it's yellow
            }
            else if (percent == -2.0f)
            {
                TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Paused; //yellow
            }
            else if (percent == -3.0f) //Downloads finished
            {
                SetButtonStartStart();
                info.FinishUpdates();

                if (newLauncher)
                    if (MessageBox.Show("Jetzt neu starten?", "Der Launcher wurde aktualisiert!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        restartLauncher = true;
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        restartLauncher = false; //don't restart
                    }
            }
            else if (percent == -4.0f) //new launcher update
            {
                newLauncher = true;
            }
            else
            {
                ProgBar.Width = percent * ProgBar.MaxWidth;
                if (STATUS == STATUS_UPDATING)
                {
                    StatusText.Text = String.Format("Aktualisiere... {0,5:F1}MB / {1,5:F1}MB", Math.Round((float)Global.Progress.loadedBytes / (float)1048576, 1), Math.Round((float)Global.Progress.maxBytes / (float)1048576, 1));
                }
                TaskbarItemInfo.ProgressValue = percent;

                if (percent == 1.0f && WindowState == WindowState.Minimized)
                {
                    StartBlink();
                }
            }
        }

        //Kram zum blinken
        private bool blinking;
        public async void StartBlink()
        {
            blinking = true;
            while (blinking)
            {
                TaskbarItemInfo.ProgressValue = 1.0f;
                await Task.Delay(800);
                TaskbarItemInfo.ProgressValue = 0.0f;
                await Task.Delay(500);
            }
        }

        public void StopBlink()
        {
            if (blinking || (TaskbarItemInfo.ProgressValue == 1.0f && TaskbarItemInfo.ProgressState == System.Windows.Shell.TaskbarItemProgressState.Normal))
            {
                blinking = false;
                TaskbarItemInfo.ProgressValue = 0.0f;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileInfo update = new FileInfo(System.AppDomain.CurrentDomain.FriendlyName + ".tmp");
            if (update.Exists && update.Length > 0)
            {
                string args = "/C choice /c:123 /t:2 > NUL & MOVE /Y \"" + update.FullName + "\" \"" + update.FullName.Trim(".tmp".ToCharArray()) + "\"";
                if (restartLauncher) // restart
                {
                    args = string.Copy(args + " & start " + System.AppDomain.CurrentDomain.FriendlyName);
                }
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    Arguments = args,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                });
            }
        }

        private void mini_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void mini_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}