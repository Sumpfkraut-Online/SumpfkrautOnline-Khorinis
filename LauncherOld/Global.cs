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
using System.Windows.Shell;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace Launcher2
{
    public class G2Paths
    {
        public string Main;
        public string Sys;
        public string GUC;
        public string DLL;
        public string Data;
        
        public G2Paths()
        {
            Main = GetG2Path(new DirectoryInfo(Directory.GetCurrentDirectory()));
            if (Main == null)
            {
                System.Windows.MessageBox.Show("Der Gothic 2 Ordner konnte nicht gefunden werden!\nDer Launcher MUSS sich im Gothic 2 Ordner befinden.\nDer Unterordner ist hierbei egal.", "FEHLER", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            Sys = Main + "\\" + "System";
            GUC = Sys + "\\" + "UntoldChapter";
            DLL = GUC + "\\" + "DLL";
            Data = Main + "\\" + "Data";
        }

        private string GetG2Path(DirectoryInfo dir)
        {
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                if (subdir.Name.ToUpper() == "SYSTEM")
                {
                    foreach (FileInfo file in subdir.GetFiles())
                    {
                        if (file.Name.ToUpper() == "GOTHIC2.EXE")
                            return dir.FullName;
                    }
                }
            }
            if (dir.Parent != null)
            {
                return GetG2Path(dir.Parent);
            }
            else
            {
                return null;
            }
        }
    }

    public class ProgressHandler
    {
        private Action<string> SetWindowError;
        private Action<float> SetWindowProgress;

        public ProgressHandler(Action<float> inSetProgress, Action<string> inSetError)
        {
            SetWindowError = inSetError;
            SetWindowProgress = inSetProgress;
        }

        public void Error(string text)
        {
            stopFakeProgress = true;
            SetError(text);
        }

        public void DownloadsFinished()
        {
            SetProgress(-3.0f);
        }

        public void NewLauncher()
        {
            SetProgress(-4.0f);
        }

        public uint maxBytes;
        public uint loadedBytes;
        private float startPercent;
        private float curPercent;
        public void Add(uint addBytes)
        {
            if (stopFakeProgress == false)
            {
                stopFakeProgress = true;
                startPercent = curPercent;
                SetProgress(-1.0f);
            }

            loadedBytes += addBytes;
            float percent = (float)(loadedBytes) / (float)maxBytes * (1.0f - startPercent) + startPercent;

            if (percent - curPercent > 0.001f)
            {
                SetProgress(percent);
                curPercent = percent;
            }
        }

        public void SetMaximum(uint max)
        {
            loadedBytes = 0;
            maxBytes = max;
        }

        private bool stopFakeProgress;
        public async void StartFakeProgress(int fakeTime, float fakePercent)
        {
            curPercent = 0;
            startPercent = 0;
            loadedBytes = 0;
            maxBytes = 0;

            int max = fakeTime / 30;

            stopFakeProgress = false;
            for (int i = 0; i < max; i++)
            {
                if (stopFakeProgress)
                {
                    return;
                }

                if (i == max - 1) //reached maximum
                {
                    SetProgress(-2.0f);
                }

                float percent = (float)(i + 1) / (float)max * fakePercent;
                if (percent - curPercent > 0.001f)
                {
                    SetProgress(percent);
                    curPercent = percent;
                }

                await Task.Delay(25);
            }
        }

        private void SetError(string text)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { SetWindowError(text); }));
        }

        private void SetProgress(float percent)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { SetWindowProgress(percent); }));
        }
    }

    public static class Global
    {
        public static G2Paths Paths;
        public static ProgressHandler Progress;
        public static ServerPing Ping;

        public static void CheckAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Bitte führe das Programm als Administrator erneut aus.");
                Application.Current.Shutdown();
            }
        }

        public static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("WinApi"))
            {
                return Assembly.LoadFrom(Global.Paths.DLL + "\\" + "WinApi.dll");
            }
            return null;
        }
    }
}
