using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WinApi;
using System.Threading;

namespace GUC.Client
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        static bool hooked = false;
        public static void SetUpHooks()
        {
            if (hooked)
                return;
            hooked = true;

            // first, kick out the original splash screen completely:

            // remove creation of the splash screen
            // copy important code to the top
            Process.Write(new byte[] { 0x8B, 0xE9,
                                       0xC7, 0x05, 0xD4, 0xDC, 0x8C, 0x00, 0x30, 0x5F, 0x42, 0x00,
                                       0x33, 0xFF }, 0x004267F1);

            // jmp over everything else
            Process.Write(new byte[] { 0xEB, 0x23 }, 0x004267FF);

            Process.Write((byte)0xEB, 0x00426872); //jump over closing message

            Process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(SplashScreen).GetMethod("RemoveSplashScreen"), 0x42687F, 6, 0); // add hook to remove our splash screen
        }

        static bool remove = false;
        static Application splash = null;
        public static void Create()
        {
            if (splash != null)
                return;
            try
            {
                var appthread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        splash = new Application();
                        splash.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                        splash.Run(new SplashScreen());
                    }
                    catch (Exception e)
                    {
                        Log.Logger.LogError("SplashScreen: " + e);
                    }
                }));
                appthread.IsBackground = true;
                appthread.SetApartmentState(ApartmentState.STA);
                if (!remove)
                    appthread.Start();
            }
            catch (Exception e2)
            {
                Log.Logger.LogError("SplashCreation failed: " + e2);
            }
        }

        public static void Remove()
        {
            RemoveSplashScreen(null);
        }

        public static Int32 RemoveSplashScreen(String message)
        {
            remove = true;

            if (splash == null)
                return 0;

            splash.Dispatcher.Invoke(new Action(() => splash.Shutdown()));
            splash = null;
            return 0;
        }
    }
}
