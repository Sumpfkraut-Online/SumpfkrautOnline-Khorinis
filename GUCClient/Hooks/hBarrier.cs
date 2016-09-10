using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using WinApi;
using Gothic.Objects.Sky;
using Gothic.System;
using Gothic.Objects;
using Gothic;

namespace GUC.Hooks
{
    static class hBarrier
    {
        static bool hooked = false;
        static void Hook()
        {
            if (hooked) return;
            hooked = true;

            Process.AddHook(Render, 0x6BB92A, 5);

            Logger.Log("Added barrier hooks.");
        }

        static bool inited = false;
        static void Init()
        {
            if (inited)
                return;
            inited = true;

            try
            {
                var barrier = new zCSkyControler_Outdoor(zCSkyControler.ActiveSkyController.Address).Barrier;
                var mat = barrier.FrontierMesh.GetSomethingMaterial();
                mat.AlphaFunc = 3;
                mat.SetTexture("BARRIERE.TGA");
                mat.Color.R = 0xFF;
                mat.Color.G = 0xFF;
                mat.Color.B = 0xFF;
                mat.Color.A = barrierAlpha;

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
        }

        static byte barrierAlpha = 0;
        public static byte BarrierAlpha
        {
            get { return barrierAlpha; }
            set
            {
                barrierAlpha = value;
                if (inited)
                {
                    oCGame.GetWorld().SkyControlerOutdoor.Barrier.FrontierMesh.GetSomethingMaterial().Color.A = value;
                }

                if (!hooked)
                    Hook();
            }
        }

        public static bool PlaySound = true;
        static readonly SoundInstance sound = new SoundInstance("MFX_BARRIERE_AMBIENT.WAV");
        static long nextSoundTime = 0;

        static readonly int ptrArg = Process.Alloc(4).ToInt32();
        static void Render(Hook hook)
        {
            try
            {
                if (!inited)
                    Init();

                var barrier = oCGame.GetWorld().SkyControlerOutdoor.Barrier;

                if (barrierAlpha > 0)
                {
                    float farClipZ = Process.ReadFloat(zCCamera.GetCamAddr() + 2300);
                    bool zBufferWriteEnabled = Process.ReadBool(zCRenderer.GetRendererAddress() + 1148);

                    zCRenderer.SetZBufferWriteEnabled(true);
                    zCCamera.SetFarClipZ(2000000.0f);

                    int context = GetResetRenderContext();
                    barrier.RenderLayer(context, 0, ptrArg);
                    barrier.RenderLayer(context, 1, ptrArg);

                    zCRenderer.FlushPolys();

                    zCCamera.SetFarClipZ(farClipZ);
                    zCRenderer.SetZBufferWriteEnabled(zBufferWriteEnabled);

                    if (PlaySound)
                    {
                        long now = GameTime.Ticks;
                        if (now >= nextSoundTime)
                        {
                            SoundHandler.PlaySound(sound, 2);
                            nextSoundTime = now + Randomizer.GetInt(500, 5000) * TimeSpan.TicksPerMillisecond;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        static readonly int renderContext = Process.Alloc(0x28).ToInt32();
        static int GetResetRenderContext()
        {
            int activeCam = zCCamera.GetCamAddr();
            int something = Process.ReadInt(activeCam + 2336);

            Process.Write(-1, renderContext);
            Process.Write(0/*something*/, renderContext + 4);
            Process.Write(Process.ReadInt(something + 184), renderContext + 8);
            Process.Write(activeCam, renderContext + 12);
            Process.Write(zeros, renderContext + 16);

            return renderContext;
        }
        static readonly byte[] zeros = Enumerable.Repeat<byte>(0x00, 0x28 - 16).ToArray();

        /*static int rndrCtAddr = 0;
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
        
        public static void Main(String message)
        {
              Process.Hook(Program.GUCDll, typeof(Injection).GetMethod("hook_Entry"), 0x6B9F30, 6, 4);
              Process.Hook(Program.GUCDll, typeof(Injection).GetMethod("hook_Rndr"), 0x6B9F9C, 6, 0);
              Process.Hook(Program.GUCDll, typeof(Injection).GetMethod("hook_Init"), 0x6B94F9, 6, 0);
        }*/
    }
}
