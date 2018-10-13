using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using Gothic.Objects;
using GUC.Log;
using Gothic.Objects.Meshes;
using Gothic.System;
using GUC.Types;
using Gothic.Objects.EventManager;
using GUC.WorldObjects;

namespace GUC.Hooks
{
    class VobRenderArgs : IDisposable
    {
        public static void Init()
        {
            if (rndrWorld != null)
                return;

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
            camera.SetFOV(45 * 4.0f / 3.0f, 45);

            lightVob = zCVobLight.Create();
            lightVob.SetRange(50000, true);
            rndrWorld.AddVob(lightVob);
        }

        string visual;
        zCVob renderVob = zCVob.Create();
        zCVob zenTree = null;

        public bool Lighting = true;

        /// <summary> Camera is Z - 100 </summary>
        public Vec3f Offset;
        public Angles Rotation;

        bool render;
        public void SetVisual(string visualName)
        {
            if (string.Equals(visual, visualName, StringComparison.OrdinalIgnoreCase))
                return;

            this.render = true;
            this.visual = visualName;
            if (zenTree != null)
            {
                zenTree.ReleaseVobSubTree();
                zenTree = null;
            }

            if (!string.IsNullOrWhiteSpace(visualName))
            {
                if (visualName.EndsWith(".ZEN", StringComparison.OrdinalIgnoreCase))
                {
                    renderVob.SetVisual(new zCVisual(0));

                    Gothic.zCOption.ChangeDir(0xE); // world directory
                    zenTree = rndrWorld.MergeVobSubTree(visualName);
                    if (zenTree.Address != 0)
                    {
                        zenTree.AddRefVobSubtree();
                        rndrWorld.RemoveVobSubTree(zenTree);
                        return;
                    }
                    else
                    {
                        Logger.LogWarning("Could not load " + visualName);
                        zenTree = null;
                    }
                }
                else
                {
                    renderVob.SetVisual(zCVisual.LoadVisual(visualName));
                    return;
                }
            }
            renderVob.SetVisual(new zCVisual(0));
            render = false;
        }

        public void SetVisual(zCVisual visual)
        {
            renderVob.SetVisual(visual);
            render = visual != null && visual.Address != 0;
        }

        public void Dispose()
        {
            renderVob?.Dispose();
            renderVob = null;
        }

        static bool inited = false;
        public static void AddHooks()
        {
            if (inited)
                return;
            inited = true;

            WinApiNew.Process.AddFastHook(OnDrawItems, 0x7A6750, 5);

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
        static zCVobLight lightVob = null;
        static void OnDrawItems(WinApiNew.RegisterMemory rmem)
        {
            try
            {
                int viewAddr = rmem.ECX;
                if (rndrDict.TryGetValue(viewAddr, out VobRenderArgs args) && args != null && args.render)
                {
                    var vob = args.zenTree ?? args.renderVob;
                    zCView view = new zCView(viewAddr);
                    camera.SetRenderTarget(view);

                    vob.SetPositionWorld(args.Offset.X, args.Offset.Y, args.Offset.Z + 100.0f);
                    lightVob.SetPositionWorld(args.Offset.X, args.Offset.Y, args.Offset.Z); // light in your face
                    args.Rotation.SetMatrix(vob.TrafoObjToWorld);
                    vob.LastTimeDrawn = -1;

                    vob.GroundPoly = 0;
                    rndrWorld.AddVob(vob);

                    bool evMan = zCEventManager.DisableEventManagers;
                    zCEventManager.DisableEventManagers = true;

                    zCRenderer.SetAlphaBlendFunc(zCRenderer.AlphaBlendFuncs.None);
                    rndrWorld.Render(camera);
                    
                    zCEventManager.DisableEventManagers = evMan;

                    rndrWorld.RemoveVobSubTree(vob);

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
