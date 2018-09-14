using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using Gothic.Objects;
using WinApi;
using GUC.Log;
using Gothic.Objects.Meshes;
using Gothic.System;
using GUC.Types;
using Gothic.Objects.EventManager;

namespace GUC.Hooks
{
    class VobRenderArgs : IDisposable
    {
        public VobRenderArgs()
        {
            if (rndrWorld == null)
            {
                rndrWorld = zCWorld.Create();
                rndrWorld.IsInventoryWorld = true;
                rndrWorld.DrawVobBBox3D = false;
                rndrWorld.BspTreeMode = 0;
                rndrWorld.ActiveSkyControler.FillBackground = false;

                camera = zCCamera.Create();
                camera.SetFarClipZ(int.MaxValue);

                camVob = zCVob.Create();
                rndrWorld.AddVob(camVob);
                camera.CamVob = camVob;
            }
        }
        oCItem item = oCItem.Create();

        public bool Lighting = true;

        public Vec3f Offset;
        public Angles Rotation;
        
        public void SetVisual(string visualName)
        {
            if (visualName.EndsWith(".ZEN"))
            {
                item.SetVisual(new zCVisual(0));
                item.SetCollDetDyn(false);
                item.SetCollDetStat(false);
                Gothic.zCOption.ChangeDir(0xE); // world directory
                rndrWorld.AddVob(item);
                item.SetPositionWorld(0, 0, 0);

                zCVob zenVob = rndrWorld.MergeVobSubTree(visualName);
                if (zenVob.Address != 0)
                {
                    zenVob.SetCollDetDyn(false);
                    zenVob.SetCollDetStat(false);
                    zenVob.SetPositionWorld(0, 0, 0);
                    item.TrafoObjToWorld.Set(zenVob.TrafoObjToWorld);
                    zenVob.AddRefVobSubtree();
                    rndrWorld.RemoveVobSubTree(zenVob);
                    rndrWorld.AddVobAsChild(zenVob, item);
                    rndrWorld.RemoveVobSubTree(item);
                }
                else
                {
                    Logger.LogWarning("Could not load " + visualName);
                }
            }
            else
            {
                zCVisual vis = zCVisual.LoadVisual(visualName);
                item.SetVisual(vis);
            }
        }

        public void SetVisual(zCVisual visual)
        {           
            item.SetVisual(visual);
        }

        public void Dispose()
        {
            item?.Dispose();
            item = null;
        }

        static bool inited = false;
        public static void AddHooks()
        {
            if (inited)
                return;
            inited = true;

            Process.AddHook(OnDrawItems, 0x7A6750, 5);

            Logger.Log("Added VobRender hooks.");
        }

        static Dictionary<int, VobRenderArgs> rndrDict = new Dictionary<int, VobRenderArgs>();
        public static void Add(int viewAddress, VobRenderArgs args)
        {
            rndrDict.Remove(viewAddress);
            rndrDict.Add(viewAddress, args);
        }

        public static void Remove(int viewAddress)
        {
            rndrDict.Remove(viewAddress);
        }


        static zCWorld rndrWorld = null;
        static zCVob camVob = null;
        static zCCamera camera = null;
        static void OnDrawItems(Hook hook, RegisterMemory rmem)
        {
            return;
            try
            {
                int viewAddr = rmem[Registers.ECX];
                if (rndrDict.TryGetValue(viewAddr, out VobRenderArgs args) && args != null)
                {
                    var vob = args.item;
                    zCView view = new zCView(viewAddr);

                    var oldCam = zCCamera.ActiveCamera;

                    int light = zCRenderer.PlayerLightInt;

                    vob.SetPositionWorld(0, 0, 0);
                    vob.GroundPoly = 0;
                    rndrWorld.AddVob(vob);

                    zCRenderer.PlayerLightInt = 50000;

                    //RenderItemPlaceCamera
                    //Process.THISCALL<NullReturnCall>(rndrVob.Address, 0x00713800, camera, new IntArg(0));
                    vob.SetPositionWorld(args.Offset.X, args.Offset.Y, args.Offset.Z + 180.0f);
                    
                    args.Rotation.SetMatrix(vob.TrafoObjToWorld);
                    

                    camera.SetRenderTarget(view);
                    vob.LastTimeDrawn = -1;

                    zCEventManager.DisableEventManagers = true;

                    zCRenderer.SetAlphaBlendFunc(zCRenderer.AlphaBlendFuncs.None);

                    rndrWorld.Render(camera);

                    zCEventManager.DisableEventManagers = false;
                    
                    rndrWorld.RemoveVobSubTree(vob);

                    zCCamera.ActiveCamera = oldCam;
                    zCRenderer.PlayerLightInt = light;
                    
                    view.Blit();
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Hook DrawItems: " + e.ToString());
            }
        }
    }
}
