using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using GUC.Client.Hooks;
using GUC.Network;

namespace GUC.Client
{
    static class Injection
    {
        public static Int32 hook_Snd(String message)
        {
            int addr = Convert.ToInt32(message);

            Log.Logger.Log(WinApi.Process.ReadInt(addr).ToString("X4") + " " + WinApi.Process.ReadInt(addr + 4).ToString("X4") + " " + WinApi.Process.ReadInt(addr + 8).ToString("X4") + " " + WinApi.Process.ReadInt(addr + 12).ToString("X4") + " " + WinApi.Process.ReadInt(addr + 16).ToString("X4") + " " + WinApi.Process.ReadInt(addr + 20).ToString("X4"));

            return 0;
        }

        public static Int32 Main(String message)
        {
            try
            {
                //SplashScreen.Create();
                //SplashScreen.SetUpHooks();

                Program.SetPaths(message);
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

                WinApi.Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x0044AEDF); // skip visual vdfs init (vdfs32g.exe)

                WinApi.Process.SetWindowText("Gothic II - Untold Chapters");

                //hParser.AddHooks();
                //hGame.AddHooks();
                //hWeather.AddHooks();
                //hPlayerVob.AddHooks();

                /* #region Some more editing

                 WinApi.Process.Write(18000.0f, 0x008BACD0); // spawnManager : insertrange
                 WinApi.Process.Write(20000.0f, 0x008BACD4); // spawnManager : removerange

                 // Make rain drops being blocked by vobs!
                 WinApi.Process.Write((byte)0xE0, 0x5E227A);

                 // Blocking Call Init Scripts!
                 WinApi.Process.Write((byte)0xC3, 0x006C1F60);
                 // Blocking Call Startup Scripts!
                 WinApi.Process.Write((byte)0xC3, 0x006C1C70);


                 WinApi.Process.Write((byte)0xEB, 0x7A55D8); // disable interface buttons
                 WinApi.Process.Write(new byte[] { 0xE9, 0xB0, 0x01, 0x00, 0x00 }, 0x0069C08B); // disable player AI
                 WinApi.Process.Write(new byte[] { 0xE9, 0xA8, 0x00 }, 0x4D4D3D); // disable ingame keyboard movement
                 WinApi.Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x4D3E50); // disable x-mouse movement  
                 WinApi.Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x4D3E5C); // disable y-mouse movement  

                 // Blocking time!
                 WinApi.Process.Write((byte)0xC3, 0x00780D80);

                 //WinApi.Process.VirtualProtect(0x007792E0, 40);
                 WinApi.Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x007792E0);//Block deleting of dead characters!

                 Logger.Log("Hooking & editing of gothic process completed. (for now...)");
                 #endregion

                 GameClient.Client.Connect();*/

                WinApi.Process.Hook(Program.GUCDll, typeof(Injection).GetMethod("hook_Snd"), 0x5ED8A0, 7, 5);

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(Environment.CurrentDirectory + " " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }
            return 0;
        }

        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));

            if (name.ToUpper() == "GUC.RESOURCES")
            {
                //load from resource
                var resxStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                byte[] buffer = new byte[resxStream.Length];
                resxStream.Read(buffer, 0, (int)resxStream.Length);
                return Assembly.Load(buffer);
            }

            return Assembly.LoadFrom(Program.ProjectPath + name + ".dll");
        }
    }
}
