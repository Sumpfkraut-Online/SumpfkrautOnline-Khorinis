using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Kernel;

namespace Launcher2
{
    public static class GUC
    {
        public static void Start(string ip)
        {
            ClientOptions co;

            string confPath = Global.Paths.GUC + "\\" + "conf";
            if (!System.IO.Directory.Exists(confPath))
            {
                System.IO.Directory.CreateDirectory(confPath);
            }

            string confFile = confPath + "\\" + "gmp.xml";
            if (!System.IO.File.Exists(confFile))
            {
                co = new ClientOptions();
            }
            else
            {
                co = ClientOptions.Load(confFile);
            }

            co.ip = ip;
            co.Save(confFile);

            String dll = Global.Paths.DLL + "\\" + "NetInject.dll";

            //zSpy starten
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.WorkingDirectory = Global.Paths.GUC + "\\Log";

            psi.FileName = Global.Paths.Main + "\\_work\\tools\\zSpy\\zSpy.exe";
            System.Diagnostics.Process.Start(psi);

            //Gothic starten
            psi = new System.Diagnostics.ProcessStartInfo();
            psi.WorkingDirectory = Global.Paths.Sys;

            //psi.Arguments = "-nomenu";
            psi.Arguments = "-zlog:5,s -zmaxframerate:60";

            psi.FileName = "Gothic2.exe";
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);

            //dll injection
            try
            {
                if (WinApi.Process.LoadLibary(process, dll) == IntPtr.Zero)
                {
                    throw new Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString());
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Source + "\n" + e.Message);
            }
            System.Windows.Application.Current.Shutdown();
        }

        public static void StartOffline()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = Global.Paths.Sys + "\\" + "Gothic2.exe"
            });
            System.Windows.Application.Current.Shutdown();
        }
    }
}
