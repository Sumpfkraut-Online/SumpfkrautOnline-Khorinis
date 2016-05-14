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
using Gothic.Objects.Sky;

namespace GUC.Client
{
    static class Injection
    {
        public static int hook_Init(string message)
        {
            try
            {
                var barrier = new zCSkyControler_Outdoor(zCSkyControler.ActiveSkyController.Address).Barrier;
                var mat = barrier.FrontierMesh.GetSomethingMaterial();
                mat.AlphaFunc = 3;
                mat.SetTexture("BARRIERE.TGA");
                mat.Color.R = 0xFF;
                mat.Color.G = 0xFF;
                mat.Color.B = 0xFF;
                mat.Color.A = 0xFF;

                var mesh = barrier.FrontierMesh;
                int numPolys = mesh.NumPolygons;

                float maxHeight = mesh.BBox3D.Max.Y;
                float minHeight = maxHeight * 0.925000011920929f;

                for (int i = 0; i < numPolys; i++)
                {
                    var poly = mesh.GetPolygon(i);
                    var numVerts = poly.NumVertices;

                    for (int j = 0; j < numVerts; j++)
                    {
                        var feat = poly.GetVertFeature(j);
                        var vert = poly.GetVertex(j);

                        int alpha;
                        if (vert.Position.Y <= minHeight)
                        {
                            alpha = (int)(vert.Position.Y * 0.0001250000059371814f * 255f);
                        }
                        else
                        {
                            alpha = (int)((maxHeight - vert.Position.Y) * 255.0f / (maxHeight - minHeight));
                        }

                        if (alpha > 255)
                            alpha = 255;
                        if (alpha < 0)
                            alpha = 0;

                        feat.Color.R = 255;
                        feat.Color.G = 255;
                        feat.Color.B = 255;
                        feat.Color.A = (byte)alpha;

                        feat.maybeDefaultColor.R = 255;
                        feat.maybeDefaultColor.G = 255;
                        feat.maybeDefaultColor.B = 255;
                        feat.maybeDefaultColor.A = (byte)alpha;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }
        
        static int rndrCtAddr = 0;
        public static int hook_Entry(string message)
        {
            try
            {
                int addr = Convert.ToInt32(message);
                rndrCtAddr = Process.ReadInt(addr + 4);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }

        static float MasterTime { get { return Process.ReadFloat(0x99B3DC); } }

        static float Flt901530 = 0;
        static int dword901528 = 0;
        static int dword901534 = 0;
        static float dword90152C = 0;

        static bool DoBarrierThings(oCBarrier barrier, bool show)
        {
            if (show)
                barrier.DwordDC = true;

            if (barrier.DwordDC)
            {
                bool b1 = false;
                if (barrier.DwordE4 != 0)
                {
                    b1 = true;
                }
                else
                {
                    barrier.FrontierMesh.GetSomethingMaterial().Color.A = 120;
                    if (MasterTime - Flt901530 > 1.0f)
                    {
                        ++barrier.Alpha;
                        Flt901530 = MasterTime;
                    }

                    if (barrier.Alpha > 120)
                    {
                        barrier.Alpha = 120;
                        barrier.DwordE4 = 0;
                        dword901528 = 1;
                        dword901534 = 1;
                        dword90152C = MasterTime;
                        b1 = true;
                    }
                }

                if (b1)
                {
                    if (dword901528 == 0)
                    {
                        if (barrier.DwordE8 != 0)
                        {
                            barrier.FrontierMesh.GetSomethingMaterial().Color.A = (byte)barrier.Alpha;
                            if (MasterTime - Flt901530 > 1.0f)
                            {
                                --barrier.Alpha;
                                Flt901530 = MasterTime;
                            }
                            if (barrier.Alpha < 1)
                            {
                                barrier.Alpha = 0;
                                barrier.DwordE4 = 1;
                                barrier.DwordE8 = 0;
                                barrier.DwordDC = false;
                            }
                        }
                        return barrier.DwordDC;
                    }
                }

                if (MasterTime - dword90152C > 25000.0f)
                {
                    barrier.DwordE8 = 1;
                    dword901528 = 0;
                    dword901534 = 0;
                    dword90152C = MasterTime;
                }

                if (barrier.DwordE8 != 0)
                {
                    barrier.FrontierMesh.GetSomethingMaterial().Color.A = (byte)barrier.Alpha;
                    if (MasterTime - Flt901530 > 1.0f)
                    {
                        --barrier.Alpha;
                        Flt901530 = MasterTime;
                    }
                    if (barrier.Alpha < 1)
                    {
                        barrier.Alpha = 0;
                        barrier.DwordE4 = 1;
                        barrier.DwordE8 = 0;
                        barrier.DwordDC = false;
                    }
                }
            }
            return barrier.DwordDC;
        }


        static float Flt86F290 = 0;
        static Random rand = new Random();

        public static int hook_Rndr(string message)
        {
            try
            {
                var skyCtrl = new zCSkyControler_Outdoor(zCSkyControler.ActiveSkyController.Address);
                var barrier = skyCtrl.Barrier;

                float diff = Flt86F290 - Process.ReadFloat(0x99B3D8);
                Flt86F290 = diff;
                if (diff < 0)
                {
                    Flt86F290 = (float)(rand.NextDouble() * 9.155552864074707d + 1200000.0d);
                    skyCtrl.bFadeInOut = true;
                }
                
                if (skyCtrl.bFadeInOut)
                {
                    var result = DoBarrierThings(barrier, skyCtrl.bFadeInOut);
                    var ptr = Process.Alloc(4);

                    Process.THISCALL<NullReturnCall>(barrier.Address, 0x6B9CF0, (IntArg)rndrCtAddr, (IntArg)0, (IntArg)ptr.ToInt32()); // oCBarrier::RenderLayer
                    Process.THISCALL<NullReturnCall>(barrier.Address, 0x6B9CF0, (IntArg)rndrCtAddr, (IntArg)1, (IntArg)ptr.ToInt32()); // oCBarrier::RenderLayer

                    Process.Free(ptr, 4);

                    Process.THISCALL<NullReturnCall>(Process.ReadInt(Gothic.System.zCRenderer.zrenderer), 0x64DD10); // zCRenderer::FlushPolys
                    
                    skyCtrl.bFadeInOut = result;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
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

                Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x0044AEDF); // skip visual vdfs init (vdfs32g.exe)

                Process.Write(new byte[] { 0xE9, 0xA3, 0x00, 0x00, 0x00 }, 0x42687F); // skip intro videos

                Process.Hook(Program.GUCDll, typeof(Injection).GetMethod("hook_Entry"), 0x6B9F30, 6, 4);
                Process.Hook(Program.GUCDll, typeof(Injection).GetMethod("hook_Rndr"), 0x6B9F9C, 6, 0);
                Process.Hook(Program.GUCDll, typeof(Injection).GetMethod("hook_Init"), 0x6B94F9, 6, 0);

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

                Process.Write(new byte[] { 0xD8, 0x1D, 0xB4, 0x04, 0x83, 0x00 }, 0x006C873D); // reduce time gothic waits after the loading screen from 2500ms to 1000ms

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
                
                Scripting.ScriptManager.StartScripts(Program.ProjectPath + "Scripts\\ClientScripts.dll"); // Load Scripts

                string[] serverAddress = Environment.GetEnvironmentVariable("ServerAddress").Split(':');
                if (serverAddress.Length != 2 || string.IsNullOrWhiteSpace(serverAddress[0]) || string.IsNullOrWhiteSpace(serverAddress[1]))
                    throw new Exception("ServerAddress has wrong format!");
                
                string ip = serverAddress[0];
                ushort port = Convert.ToUInt16(serverAddress[1]);
                GameClient.Client.Connect(ip, port, Constants.VERSION);

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
