using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using Gothic.Objects;
using WinApi;
using GUC.Log;

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

        static void OnDrawItems(Hook hook, RegisterMemory rmem)
        {
            try
            {
                int viewAddr = rmem[Registers.ECX];
                zCVob vob;
                if (VobRenderList.TryGetValue(viewAddr, out vob))
                {
                    if (rndrWorld == null)
                    {
                        rndrWorld = zCWorld.Create();
                        rndrWorld.IsInventoryWorld = true;
                    }

                    zCView view = new zCView(viewAddr);

                    //add some light
                    Process.Write(vob.Address + zCVob.VarOffsets.lightColorStat, 0xFF, 0xFF, 0xFF, 0xFF);
                    Process.Write(vob.Address + zCVob.VarOffsets.lightDirectionStat + 8, 1.0f);

                    //draw the vob
                    new oCItem(vob.Address).RenderItem(rndrWorld, view, 0);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Hook DrawItems: " + e.ToString());
            }
        }
    }
}
