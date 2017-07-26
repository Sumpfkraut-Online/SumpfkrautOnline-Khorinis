using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Log;
using Gothic.Objects.Meshes;
using GUC.WorldObjects;

namespace GUC.Hooks
{
    static class hModel
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited)
                return;
            inited = true;

            Process.AddHook(AdvanceAni, 0x57CE90, 0x7);
        }

        static void AdvanceAni(Hook hook, RegisterMemory mem)
        {
            var gModel = new zCModel(mem[Registers.EBX]);
            var gActiveAni = new zCModelAniActive(mem[Registers.ESI]);
            string aniName = gActiveAni.ModelAni.Name.ToString();

            Vob vob;
            if (World.Current.TryGetVobByAddress(gModel.Owner.Address, out vob))
            {
                vob.Model.ForEachActiveAniPredicate(aa =>
                {
                    if (aa.AniJob.Name == aniName)
                    {
                        // guc ani
                        float startFrame = aa.Ani.StartFrame;
                        float endFrame = aa.Ani.EndFrame;

                        float percent = gActiveAni.ModelAni.IsReversed ? (1 - aa.GetProgress()) : aa.GetProgress();

                        float actFrame = startFrame + (endFrame - startFrame) * percent;
                        gActiveAni.SetActFrame(actFrame);

                        return false;
                    }
                    return true;
                });
            }
        }
    }
}
