using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GUC.Network;
using Gothic;
using System.IO;
using Gothic.Types;
using System.Runtime.InteropServices;
using GUC.Log;
using WinApi;
using GUC.Hooks;
using System.Reflection;

namespace GUC
{
    //zCMesh::Load(class zSTRING const &, int)
    //( zCMesh::MergeMesh(class zCMesh *, class zMAT4 const &) ? )
    //zCWorld::CompileWorld(zTBspTreeMode const &,float,int,int,zCArray<zCPolygon *> *) ???

    public static class Program
    {
        static string projectName;
        static string projectPath;
        static string serverAddress;

        public static string ProjectName { get { return projectName; } }
        public static string ProjectPath { get { return projectPath; } }
        public static string ServerAddress { get { return serverAddress; } }
        public const ushort ServerPort = 9054;

        static void SetupProject()
        {
            projectName = Environment.GetEnvironmentVariable("GUCProject");
            if (string.IsNullOrWhiteSpace(projectName))
                throw new Exception("Project name environment variable is null or empty!");

            serverAddress = Environment.GetEnvironmentVariable("ServerAddress");
            if (string.IsNullOrWhiteSpace(serverAddress))
                throw new Exception("Server address environment variable is null or empty!");

            string current = Path.GetFullPath(Environment.CurrentDirectory); // It's Gothic2/System when the process starts, Gothic2/ afterwards.

            if (File.Exists(current + "\\Gothic2.exe"))
            { // Gothic2/System/
                projectPath = Path.GetFullPath(current + "\\Multiplayer\\UntoldChapters\\" + projectName + "\\");
            }
            else if (File.Exists(current + "\\System\\Gothic2.exe"))
            { // Gothic2/
                projectPath = Path.GetFullPath(current + "\\System\\Multiplayer\\UntoldChapters\\" + projectName + "\\");
            }
            else
            {
                throw new Exception("Gothic 2 not found!");
            }

            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
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

            return Assembly.LoadFrom(projectPath + name + ".dll");
        }

        static bool mained = false;
        public static int Main(string message)
        {
            try
            {
                if (mained) return 0;
                mained = true;

                Logger.Log("GUC started...");

                SetupProject();

                SplashScreen.SetUpHooks();
                SplashScreen.Create();

                Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x0044AEDF); // skip visual vdfs init (vdfs32g.exe)
                Process.Write(new byte[] { 0xE9, 0xA3, 0x00, 0x00, 0x00 }, 0x42687F); // skip intro videos

                // add hooks
                hParser.AddHooks();
                hGame.AddHooks();
                hWeather.AddHooks();
                hPlayerVob.AddHooks();
                hView.AddHooks();

                #region Some more editing

                Process.Write(new byte[] { 0xEB, 0x15 }, 0x006B5A44); // don't start falling animation

                Process.Write(new byte[] { 0xC2, 0x08, 0x00 }, 0x00735EB0); // don't drop unconscious
                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x00736898); // don't drop weapons on death

                // remove all gothic controls
                Process.Write(new byte[] { 0xE9, 0x3E, 0x04, 0x00, 0x00 }, 0x004D3DF6);
                Process.Write(new byte[] { 0xC3, 0xE8, 0x8B, 0xE6, 0xFF, 0xFF, 0xC3 }, 0x004D5700);
                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x006C8A71); // remove freeLook controls

                Process.Write(new byte[] { 0xD8, 0x1D, 0xB4, 0x04, 0x83, 0x00 }, 0x006C873D); // reduce time gothic waits after the loading screen from 2500ms to 1000ms

                Process.Write(18000.0f, 0x008BACD0); // spawnManager : insertrange
                Process.Write(20000.0f, 0x008BACD4); // spawnManager : removerange

                // Make rain drops being blocked by vobs!
                Process.Write((byte)0xE0, 0x5E227A);

                // Blocking Call Init Scripts!
                Process.Write((byte)0xC3, 0x006C1F60);
                // Blocking Call Startup Scripts!
                Process.Write((byte)0xC3, 0x006C1C70);


                Process.Write(new byte[] { 0xDC, 0x0D, 0x30, 0xEB, 0x82, 0x00 }, 0x0069C2DA); // bleed with < 25% health

                Process.Write(new byte[] { 0xE9, 0xB0, 0x01, 0x00, 0x00 }, 0x0069C08B); // disable player AI
                Process.Write(new byte[] { 0xE9, 0x89, 0, 0, 0, 0x90 }, 0x0069BFCB); // disable focus highlighting

                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x006B0896);// don't let oCAniCtrl_Human::CreateHit check whether the target is an enemy

                Process.Write((byte)0xC3, 0x0073E480);//Blocking oCNpc::ProcessNpc (Dive Damage etc)
                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x0066CAC9);//Block Damage!

                // Blocking time!
                Process.Write((byte)0xC3, 0x00780D80);

                //Process.VirtualProtect(0x007792E0, 40);
                Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x007792E0);//Block deleting of dead characters!

                Logger.Log("Hooking & editing of gothic process completed. (for now...)");
                #endregion

                // Load Scripts
                Scripting.ScriptManager.StartScripts(Program.ProjectPath + "Scripts\\ClientScripts.dll");

                Logger.Log("Waiting...");
                SplashScreen.WaitHandle.WaitOne(3000);
                SplashScreen.WaitHandle = null;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }
        public static void Exit()
        {
            GameClient.Client.Disconnect();
            Logger.Log("Exiting...");
            Thread.Sleep(200);
            CGameManager.ExitGameVar = 1;
            zCOption.GetSectionByName("internal").GetEntryByName("gameAbnormalExit").VarValue.Set("0");
            zCOption.Save(zString.Create("Gothic.ini")); // don't dispose this zString or crashes will happen
            //Process.CDECLCALL<NullReturnCall>(0x00425F30); // ExitGameFunc
        }
    }
}
