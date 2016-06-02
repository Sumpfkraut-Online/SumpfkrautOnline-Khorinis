using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WinApi;
using System.Threading;
using GUC.Log;
using System.Windows.Media;

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

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (WaitHandle != null)
            {
                WaitHandle.Set();
            }
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
            Process.Write(new byte[] { 0xEB, 0x28 }, 0x004267FF);


            Process.Write(new byte[] { 0x83, 0xC4, 0x04 }, 0x426868);// add esp, 4 (original code)
            Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x42686B); // NOP everything else
            Process.AddHook(RemoveSplashScreen, 0x42686B, 5, 0); // add hook to remove our splash screen

            Logger.Log("Gothic-SplashScreen hooked.");
        }

        static Application splash = null;
        public static EventWaitHandle WaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        public static void Create()
        {
            if (splash != null)
                return;
            
            try
            {
                var appThread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        splash = new Application();
                        splash.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                        splash.Run(new SplashScreen());
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("GUC-SplashScreen Thread: " + e);
                    }
                }));
                appThread.IsBackground = true;
                appThread.SetApartmentState(ApartmentState.STA);
                appThread.Start();

                Logger.Log("GUC-SplashScreen started.");
            }
            catch (Exception e2)
            {
                Logger.LogError("Creation of GUC-SplashScreen failed: " + e2);
            }
        }

        static void RemoveSplashScreen(Hook hook)
        {
            if (splash == null)
                return;

            Logger.Log("Close SplashScreen.");
            
            splash.Dispatcher.Invoke(new Action(() => splash.Shutdown()));
            splash = null;
        }
    }
}
