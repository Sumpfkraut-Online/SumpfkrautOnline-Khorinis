using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using System.Reflection;
using GUC.Client.Hooks;
using GUC.Network;
using WinApi;

namespace GUC.Client
{
    static class Injection
    {
        public static Int32 Main(String message)
        {
            try
            {
                //SplashScreen.Create();
                //SplashScreen.SetUpHooks();

                Program.SetPaths(message);
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

                Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x0044AEDF); // skip visual vdfs init (vdfs32g.exe)

                Process.SetWindowText("Gothic II - Untold Chapters");

                hParser.AddHooks();
                hGame.AddHooks();
                hWeather.AddHooks();
                hPlayerVob.AddHooks();

                #region Some more editing

                Process.Write(new byte[] { 0xEB, 0x15 }, 0x006B5A44); // don't start falling animation

                // remove all gothic controls
                Process.Write(new byte[] { 0xE9, 0x3E, 0x04, 0x00, 0x00 }, 0x004D3DF6);
                Process.Write(new byte[] { 0xC3, 0xE8, 0x8B, 0xE6, 0xFF, 0xFF, 0xC3 }, 0x004D5700);

                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x006C8A71); // remove freeLook controls

                Process.Write(new byte[] { 0xD8, 0x1D, 0xB4, 0x04, 0x83, 0x00}, 0x006C873D); // reduce time gothic waits after the loading screen from 2500ms to 1000ms

                Process.Write(18000.0f, 0x008BACD0); // spawnManager : insertrange
                Process.Write(20000.0f, 0x008BACD4); // spawnManager : removerange

                // Make rain drops being blocked by vobs!
                Process.Write((byte)0xE0, 0x5E227A);

                // Blocking Call Init Scripts!
                Process.Write((byte)0xC3, 0x006C1F60);
                // Blocking Call Startup Scripts!
                Process.Write((byte)0xC3, 0x006C1C70);
                
                Process.Write(new byte[] { 0xE9, 0xB0, 0x01, 0x00, 0x00 }, 0x0069C08B); // disable player AI

                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x006B0896);// don't let oCAniCtrl_Human::CreateHit check whether the target is an enemy
                
                Process.Write((byte)0xC3, 0x0073E480);//Blocking oCNpc::ProcessNpc (Dive Damage etc)
                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x0066CAC9);//Block Damage!

                // Blocking time!
                Process.Write((byte)0xC3, 0x00780D80);

                //Process.VirtualProtect(0x007792E0, 40);
                Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x007792E0);//Block deleting of dead characters!

                Logger.Log("Hooking & editing of gothic process completed. (for now...)");
                #endregion

                GameClient.Client.Connect();

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
