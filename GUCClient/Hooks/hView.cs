using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using Gothic.Objects;
using WinApi;
using GUC.Log;
using Gothic.Objects.EventManager;
using Gothic.System;
using GUC.Types;

namespace GUC.Hooks
{
    class hView
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited)
                return;
            inited = true;
            
            Process.AddHook(OnDrawItems, 0x7A6750, 5);

            Logger.Log("Added zCView hooks.");
        }

        public static Dictionary<int, zCVob> VobRenderList = new Dictionary<int, zCVob>();

        static zCWorld rndrWorld = null;
        static zCVob camVob = null;
        static zCCamera camera = null;

        static void OnDrawItems(Hook hook, RegisterMemory rmem)
        {
            try
            {
                int viewAddr = rmem[Registers.ECX];
                if (VobRenderList.TryGetValue(viewAddr, out zCVob vob))
                {
                    zCView view = new zCView(viewAddr);
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

                    //RenderItem(rndrWorld, view, 0);

                    var oldCam = zCCamera.ActiveCamera;

                    int light = zCRenderer.PlayerLightInt;

                    vob.SetPositionWorld(0, 0, 0);
                    vob.GroundPoly = 0;
                    rndrWorld.AddVob(vob);

                    zCRenderer.PlayerLightInt = 5000;

                    //RenderItemPlaceCamera
                    //Process.THISCALL<NullReturnCall>(rndrVob.Address, 0x00713800, camera, new IntArg(0));
                    var vec = (Vec3f)camVob.Direction;
                    vec *= 150;
                    vob.SetPositionWorld(vec.X, vec.Y, vec.Z);

                    vec = (Vec3f)camVob.Position - (Vec3f)vob.Position;
                    vob.SetHeadingWorld(vec.X, vec.Y, vec.Z);

                    camera.SetRenderTarget(view);
                    vob.LastTimeDrawn = -1;

                    zCEventManager.DisableEventManagers = true;

                    zCRenderer.SetAlphaBlendFunc(zCRenderer.AlphaBlendFuncs.None);

                    rndrWorld.Render(camera);

                    zCEventManager.DisableEventManagers = false;

                    rndrWorld.RemoveVobSubTree(vob);

                    zCCamera.ActiveCamera = oldCam;
                    zCRenderer.PlayerLightInt = light;

                }
            }
            catch (Exception e)
            {
                Logger.LogError("Hook DrawItems: " + e.ToString());
            }
        }
    }
}
