using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using Gothic.Objects;
using WinApi;
using GUC.Log;
using Gothic.Objects.Meshes;

namespace GUC.Hooks
{
    class VobRenderArgs : IDisposable
    {
        oCItem item = oCItem.Create();

        public bool Lighting = true;

        public int ZBias { set { item.Inv_ZBias = value; } get { return item.Inv_ZBias; } }
        public int RotX { set { item.Inv_RotX = value; } get { return item.Inv_RotX; } }
        public int RotY { set { item.Inv_RotY = value; } get { return item.Inv_RotY; } }
        public int RotZ { set { item.Inv_RotZ = value; } get { return item.Inv_RotZ; } }

        public void SetVisual(string str)
        {
            item.SetVisual(str);
        }

        public void SetVisual(zCVisual visual)
        {
            /*var npc = oCNpc.Create();
            npc.SetVisual("HUMANS.MDS");
            npc.SetAdditionalVisuals("HUM_BODY_NAKED0", 8, 0, "HUM_HEAD_PONY", 0, 0, 0);
            var vis = npc.Visual;
            Add(0, null);
            rndrWorld.AddVob(npc);*/

            Process.Write(0x713BCA, int.MaxValue);
            

            item.SetVisual(visual);
            //item.SetVisual("ITFO_BREAD.3DS");
            //item.Visual = visual;
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

            if (rndrWorld == null)
            {
                rndrWorld = zCWorld.Create();
                rndrWorld.IsInventoryWorld = true;
            }
        }

        public static void Remove(int viewAddress)
        {
            rndrDict.Remove(viewAddress);
        }


        static zCWorld rndrWorld = null;
        static void OnDrawItems(Hook hook, RegisterMemory rmem)
        {
            try
            {
                int viewAddr = rmem[Registers.ECX];
                if (rndrDict.TryGetValue(viewAddr, out VobRenderArgs args) && args != null)
                {
                    zCView view = new zCView(viewAddr);

                    //args.item.LightColorDyn.Set(0xFFFFFFFF);
                    //args.item.LightColorStat.Set(0xFFFFFFFF);
                    //args.item.LightColorStatDir.X = 0;
                    //args.item.LightColorStatDir.Y = -1;
                    //args.item.LightColorStatDir.Z = 0;

                    bool before = oCItem.LightingSwell;
                    if (before != args.Lighting)
                    {
                        oCItem.LightingSwell = args.Lighting;
                        args.item.RenderItem(rndrWorld, view, 0);
                        oCItem.LightingSwell = before;
                    }
                    else
                    {
                        args.item.RenderItem(rndrWorld, view, 0);
                    }
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
